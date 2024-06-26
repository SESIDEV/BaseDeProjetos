﻿using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class EmpresasController : SGIController
    {
        private readonly ApplicationDbContext _context;
        private readonly DbCache _cache;

        public EmpresasController(ApplicationDbContext context, DbCache cache)
        {
            _context = context;
            _cache = cache;
        }

        private static string ReplaceBase64Data(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string pattern = @"data:image/[^;]+;base64,";

            return Regex.Replace(input, pattern, "");
        }

        [HttpGet("Empresas/ReprocessarImagens")]
        public async Task<IActionResult> ReprocessarImagens()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var empresas = await _context.Empresa.ToListAsync();

                foreach (var empresa in empresas)
                {
                    if (!string.IsNullOrEmpty(empresa.Logo) && !empresa.Logo.StartsWith("http"))
                    {
                        await ReprocessarImagemEmpresa(empresa);
                    }
                    await _context.SaveChangesAsync();
                    CacheHelper.CleanupEmpresasCache(_cache);
                }
                return View("Processamento");
            }
            else
            {
                return View("Forbidden");
            }
        }

        private async Task ReprocessarImagemEmpresa(Empresa empresa)
        {
            if (empresa.Logo != null)
            {
                string logoSemHeader = ReplaceBase64Data(empresa.Logo);

                try
                {
                    byte[] bytesImagem = Convert.FromBase64String(logoSemHeader);
                    MemoryStream streamLogo = new MemoryStream(bytesImagem);
                    Image imagemSource = Image.FromStream(streamLogo);

                    const int Tamanho = 96;
                    Bitmap imagemFinal = new Bitmap(Tamanho, Tamanho);
                    Graphics graphics = Graphics.FromImage(imagemFinal);

                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    graphics.Clear(Color.White);

                    if (imagemSource.Width > imagemSource.Height)
                    {
                        int x = 0;
                        int y = (Tamanho - (imagemSource.Height * Tamanho / imagemSource.Width)) / 2;
                        graphics.DrawImage(imagemSource, new Rectangle(x, y, Tamanho, imagemSource.Height * Tamanho / imagemSource.Width));
                    }
                    else
                    {
                        int x = (Tamanho - (imagemSource.Width * Tamanho / imagemSource.Height)) / 2;
                        int y = 0;
                        graphics.DrawImage(imagemSource, new Rectangle(x, y, imagemSource.Width * Tamanho / imagemSource.Height, Tamanho));
                    }

                    streamLogo.Position = 0;
                    imagemFinal.Save(streamLogo, ImageFormat.Png);
                    streamLogo.Position = 0;

                    string resultadoDimensionamentoB64 = "";

                    try
                    {
                        resultadoDimensionamentoB64 = Convert.ToBase64String(streamLogo.ToArray());
                        empresa.Logo = resultadoDimensionamentoB64;
                    }
                    catch (FormatException)
                    {
                        empresa.Logo = "";
                    }
                }
                catch (ArgumentException)
                {
                    empresa.Logo = "";
                }
                catch (FormatException)
                {
                    empresa.Logo = "";
                }
            }
        }


        // GET: Empresas
        [Route("Empresas/Index/{aba?}")]
        public async Task<IActionResult> Index(string searchString = "", string aba = "", int numeroPagina = 1, int tamanhoPagina = 30)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ParametrosFunil parametrosFunil = new ParametrosFunil
                {
                    Aba = aba,
                    SearchString = searchString,
                    NumeroPagina = numeroPagina,
                    TamanhoPagina = tamanhoPagina
                };

                SetarParametrosFunilSession(parametrosFunil);
                ViewbagizarUsuario(_context, _cache);

                ViewBag.searchString = searchString;
                ViewBag.TamanhoPagina = tamanhoPagina;

                var empresas = await FiltrarEmpresas(_context, _cache, searchString);

                int qtdEmpresas = empresas.Count();
                int qtdPaginasTodo = (int)Math.Ceiling((double)qtdEmpresas / tamanhoPagina);

                List<Empresa> empresasPagina = ObterEmpresasPorPagina(empresas, numeroPagina, tamanhoPagina);

                var pager = new Pager(qtdEmpresas, numeroPagina, tamanhoPagina, 50); // 50 paginas max

                var model = new EmpresasViewModel
                {
                    Empresas = empresasPagina,
                    Pager = pager,
                };

                var prospeccoes = await _cache.GetCachedAsync("Prospeccoes:Empresas", () => _context.Prospeccao.Select(p => new ProspeccaoEmpresasDTO { Empresa = p.Empresa, Id = p.Id, NomeProspeccao = p.NomeProspeccao, Status = p.Status, Usuario = p.Usuario }).ToListAsync());
                ViewBag.Prospeccoes = prospeccoes;

                ViewBag.ProspeccoesAtivas = prospeccoes.Where(P => P.Status.All(S => S.Status != StatusProspeccao.NaoConvertida &&
                                                                                        S.Status != StatusProspeccao.Convertida &&
                                                                                        S.Status != StatusProspeccao.Suspensa) &&
                                                                                        P.Status.OrderBy(k => k.Data).LastOrDefault().Status != StatusProspeccao.Planejada).ToList();

                ViewBag.ProspeccoesPlanejadas = prospeccoes.Where(P => P.Status.All(S => S.Status == StatusProspeccao.Planejada)).ToList();

                ViewBag.OutrasProspeccoes = prospeccoes.Where(P => P.Status.Any(S => S.Status == StatusProspeccao.NaoConvertida ||
                                  S.Status == StatusProspeccao.Convertida ||
                                  S.Status == StatusProspeccao.Suspensa)).ToList();

                ViewBag.Projetos = await _context.Projeto.ToListAsync();

                ViewBag.Contatos = await _context.Pessoa.ToListAsync();
                //Filtros e ordenadores
                if (string.IsNullOrEmpty(searchString) && HttpContext.Session.Keys.Contains("_CurrentFilter"))
                {
                    ViewData["CurrentFilter"] = HttpContext.Session.GetString("_CurrentFilter");
                }
                else
                {
                    ViewData["CurrentFilter"] = searchString;
                    HttpContext.Session.SetString("_CurrentFilter", searchString);
                }

                if (numeroPagina > qtdPaginasTodo)
                {
                    var paginaVazia = new EmpresasViewModel
                    {
                        Empresas = new List<Empresa>(),
                        Pager = pager
                    };
                    return View(paginaVazia);
                }

                return View(model);
            }
            else
            {
                return View("Forbidden");
            }
        }

        private void SetarParametrosFunilSession(ParametrosFunil parametrosFunil)
        {
            SetarAbaNaSession(parametrosFunil.Aba);
            SetarSearchStringNaSession(parametrosFunil.SearchString);
            SetarNumeroPaginaSession(parametrosFunil.NumeroPagina);
            SetarTamanhoPaginaSession(parametrosFunil.TamanhoPagina);
        }


        private void SetarTamanhoPaginaSession(int tamanhoPagina)
        {
            HttpContext.Session.SetInt32("tamanhoPagina", tamanhoPagina);
        }

        private void SetarNumeroPaginaSession(int numeroPagina)
        {
            HttpContext.Session.SetInt32("numeroPagina", numeroPagina);
        }

        private void SetarSearchStringNaSession(string searchString)
        {
            if (searchString != null)
            {
                HttpContext.Session.SetString("searchString", searchString);
            }
        }

        private void SetarAbaNaSession(string aba)
        {
            if (aba != null)
            {
                HttpContext.Session.SetString("aba", aba);
            }
        }

        private List<Empresa> ObterEmpresasPorPagina(List<Empresa> empresas, int numeroPagina, int tamanhoPagina)
        {
            return empresas.Skip((numeroPagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();
        }

        /// <summary>
        /// Verifica e retorna ?um json? caso o CNPJ passado como string para a rota exista
        /// </summary>
        /// <param name="cnpj">CNPJ da Empresa a ser buscada</param>
        /// <returns></returns>
        public async Task<JsonResult> SeExisteCnpj(string cnpj)
        {
            if (HttpContext.User.Identity.IsAuthenticated) // Não expor o banco a ataques
            {
                var empresas = await _context.Empresa.Select(e => new { e.CNPJ }).ToListAsync();
                var procurar_dado = empresas.Where(x => x.CNPJ == cnpj).FirstOrDefault();
                if (procurar_dado != null) { return Json(1); } else { return Json(0); }
            }
            else
            {
                return Json("403 Forbidden");
            }
        }

        /// <summary>
        /// Retorna os dados de um CNPJ utilizando a ?API da Receita Federal?
        /// </summary>
        /// <param name="query">Conteúdos da query a ser realizada para a API</param>
        /// <returns></returns>
        public async Task<string> DadosAPI(string query)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                HttpClient client = new HttpClient { BaseAddress = new Uri("https://receitaws.com.br/v1/cnpj/") };
                var response = await client.GetAsync(query);
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }
            else
            {
                return "403 Forbidden";
            }
        }

        /// <summary>
        /// Obtém uma lista de empresas de acordo com o termo de busca
        /// </summary>
        /// <param name="searchString">Busca a ser realizada</param>
        /// <param name="empresas">Lista de empresas filtradas</param>
        /// <returns></returns>
        private static async Task<List<Empresa>> FiltrarEmpresas(ApplicationDbContext context, DbCache cache, string searchString)
        {
            var empresas = await cache.GetCachedAsync("AllEmpresas", () => context.Empresa.ToListAsync());

            if (string.IsNullOrEmpty(searchString))
            {
                return empresas;
            }
            else
            {
                searchString = searchString.ToLower();

                string searchStringCNPJSemCaracteres = searchString.Replace("/", "").Replace(".", "").Replace("-", "");

                var empresasFiltradas = empresas.Where(e =>
                    e.RazaoSocial != null && e.RazaoSocial.ToLower().Contains(searchString.ToLower()) ||
                    e.Nome != null && e.Nome.ToLower().Contains(searchString.ToLower()) ||
                    e.CNPJ != null && e.CNPJ.Contains(searchString) ||
                    e.CNPJ != null && e.CNPJ.Contains(searchStringCNPJSemCaracteres)).OrderBy(e => e.Nome).ToList();
                return empresasFiltradas;
            }
        }

        /// <summary>
        /// Retorna um modal de acordo com os parâmetros passados
        /// </summary>
        /// <param name="idEmpresa">ID da Empresa</param>
        /// <param name="tipo">Tipo de Modal a ser retornado</param>
        /// <returns></returns>
        public IActionResult RetornarModal(int idEmpresa, string tipo)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (tipo != null || tipo != "")
                {
                    return ViewComponent($"Modal{tipo}Empresa", new { id = idEmpresa });
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                if (id == null)
                {
                    return NotFound();
                }

                var empresas = await _cache.GetCachedAsync("AllEmpresas", () => _context.Empresa.ToListAsync());
                Empresa empresa = empresas.FirstOrDefault(m => m.Id == id);

                if (empresa == null)
                {
                    return NotFound();
                }

                return View(empresa);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                return View();
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Logo,RazaoSocial,CNPJ,Segmento,Estado,Industrial,Nome")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                await ReprocessarImagemEmpresa(empresa);
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                CacheHelper.CleanupEmpresasCache(_cache);

                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa"), SearchString = empresa.Nome });
            }
            else
            {
                throw new InvalidDataException("Dados de empresa inválidos");
            }
        }

        private static void LimparStringLogo(Empresa empresa)
        {
            empresa.Logo = empresa.Logo.Replace("data:image/png;base64,data:image/png;base64", "");
            empresa.Logo = empresa.Logo.Replace("data:image/png;base64", "");
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                if (id == null)
                {
                    return NotFound();
                }

                var empresas = await _cache.GetCachedAsync("AllEmpresas", () => _context.Empresa.ToListAsync());
                Empresa empresa = empresas.FirstOrDefault(m => m.Id == id);

                if (empresa == null)
                {
                    return NotFound();
                }
                return View(empresa);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,RazaoSocial,CNPJ,Segmento,Estado,Industrial,Nome")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                throw new ArgumentException($"Id da empresa não está presente: {id} x {empresa.Id}");
            }

            if (ModelState.IsValid)
            {
                try
                {   // O CONTROLLER PRECISA RECEBER O CAMPO 'ESTADO' E VERIFICAR A QUAL [Name] ELE PERTENCE NA BASE ENUM #########################################
                    await ReprocessarImagemEmpresa(empresa);
                    empresa.CNPJ = Regex.Replace(empresa.CNPJ, "[^0-9]", "");
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                    CacheHelper.CleanupEmpresasCache(_cache);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!await EmpresaExists(empresa.Id))
                    {
                        throw new ArgumentException($"Empresa já existe: {e}");
                    }
                    else
                    {
                        throw e;
                    }
                }
                return RedirectToAction("Index", "Empresas",
                    new
                    {
                        aba = HttpContext.Session.GetString("aba"),
                        searchString = HttpContext.Session.GetString("searchString"),
                        numeroPagina = HttpContext.Session.GetInt32("numeroPagina"),
                        tamanhoPagina = HttpContext.Session.GetInt32("tamanhoPagina")
                    });
            }
            else
            {
                throw new ArgumentException($"Modelo está invalido: {ModelState.IsValid}");
            }
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                if (id == null)
                {
                    return NotFound();
                }

                var empresas = await _cache.GetCachedAsync("AllEmpresas", () => _context.Empresa.ToListAsync());
                Empresa empresa = empresas.FirstOrDefault(m => m.Id == id);

                if (empresa == null)
                {
                    return NotFound();
                }

                return View(empresa);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Empresa empresa = await _context.Empresa.FindAsync(id);
            IQueryable<Pessoa> pessoasRemove = _context.Pessoa.Where(p => p.EmpresaId == empresa.Id);
            _context.Pessoa.RemoveRange(pessoasRemove);
            _context.Empresa.Remove(empresa);
            await _context.SaveChangesAsync();
            CacheHelper.CleanupEmpresasCache(_cache);
            
            return RedirectToAction("Index", "Empresas",
                    new
                    {
                        aba = HttpContext.Session.GetString("aba"),
                        searchString = HttpContext.Session.GetString("searchString"),
                        numeroPagina = HttpContext.Session.GetInt32("numeroPagina"),
                        tamanhoPagina = HttpContext.Session.GetInt32("tamanhoPagina")
                    });
        }

        /// <summary>
        /// Verifica se uma empresa existe no Banco de Dados
        /// </summary>
        /// <param name="id">ID da empresa</param>
        /// <returns></returns>
        private async Task<bool> EmpresaExists(int id)
        {
            var empresas = await _cache.GetCachedAsync("Empresas:DTO", () => _context.Empresa.Select(e => new EmpresaDTO { Id = e.Id, CNPJ = e.CNPJ }).ToListAsync());
            return empresas.Any(e => e.Id == id);
        }

        /// <summary>
        /// Retorna um JSON com os dados das Empresas cadastradas
        /// </summary>
        /// <param name="estrangeiras">Parâmetro para definir se as empresa a serem retornadas ?são as estrangeiras ou não?</param>
        /// <returns></returns>
        public async Task<string> PuxarEmpresas(bool estrangeiras = false) // Apenas um ou outro por enquanto
        {
            var empresas = await _cache.GetCachedAsync("AllEmpresas", () => _context.Empresa.ToListAsync());

            List<Dictionary<string, object>> listaFull = new List<Dictionary<string, object>>();

            foreach (var empresa in empresas)
            {
                bool aprovado = false;

                if (estrangeiras)
                {
                    if (empresa.CNPJ == "00000000000000" || empresa.Estado == Estado.Estrangeiro)
                    {
                        aprovado = true;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (empresa.CNPJ != "00000000000000")
                    {
                        aprovado = true;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (aprovado)
                {
                    Dictionary<string, object> dict = new Dictionary<string, object>
                    {
                        ["Id"] = empresa.Id,
                        ["RazaoSocial"] = empresa.RazaoSocial,
                        ["NomeFantasia"] = empresa.Nome,
                        ["Segmento"] = empresa.Segmento.GetDisplayName(),
                        ["Estado"] = empresa.Estado.GetDisplayName(),
                        ["CNPJ"] = empresa.CNPJ
                    };
                    listaFull.Add(dict);
                }
            }

            return JsonSerializer.Serialize(listaFull);
        }
    }
}