using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Empresas
        public IActionResult Index(string searchString = "")
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

				ViewBag.usuarioFoto = usuario.Foto;
				ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                ViewBag.Prospeccoes = _context.Prospeccao.Where(P => P.Status.All(S => S.Status != StatusProspeccao.NaoConvertida &&
                                                                                        S.Status != StatusProspeccao.Convertida &&
                                                                                        S.Status != StatusProspeccao.Suspensa)).ToList();

                ViewBag.ProspeccoesAtivas = _context.Prospeccao.Where(P => P.Status.All(S => S.Status != StatusProspeccao.NaoConvertida &&
                                                                                        S.Status != StatusProspeccao.Convertida &&
                                                                                        S.Status != StatusProspeccao.Suspensa)
                                                                        && P.Status.OrderBy(k => k.Data).LastOrDefault().Status != StatusProspeccao.Planejada).ToList();

                ViewBag.ProspeccoesPlanejadas = _context.Prospeccao.Where(P => P.Status.All(S => S.Status == StatusProspeccao.Planejada)).ToList();

                ViewBag.Contatos = _context.Pessoa.ToList();
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

                var empresas = FiltrarEmpresas(searchString, _context.Empresa.OrderBy(e => e.Nome).ToList());

                return View(empresas);
            }
            else
            {
                return View("Forbidden");
            }
        }

        public JsonResult SeExisteCnpj(string cnpj)
        {
            if (HttpContext.User.Identity.IsAuthenticated) // Não expor o banco a ataques
            {
                var procurar_dado = _context.Empresa.Where(x => x.CNPJ == cnpj).FirstOrDefault();
                if (procurar_dado != null) { return Json(1); } else { return Json(0); }
            }
            else
            {
                return Json("403 Forbidden");
            }

        }
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
        private static List<Empresa> FiltrarEmpresas(string searchString, List<Empresa> lista)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                lista = lista.Where(e =>
                e.Nome.ToLower().Contains(searchString.ToLower()) ||
                e.NomeFantasia.ToLower().Contains(searchString.ToLower()) ||
                e.CNPJ.ToLower().Contains(searchString.ToLower())).ToList();

            }

            return lista;
        }

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
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                if (id == null)
                {
                    return NotFound();
                }

                Empresa empresa = await _context.Empresa
                    .FirstOrDefaultAsync(m => m.Id == id);
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
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

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
        public async Task<IActionResult> Create([Bind("Id,Logo,Nome,CNPJ,Segmento,Estado,Industrial,NomeFantasia,CNAE")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                if (id == null)
                {
                    return NotFound();
                }

                Empresa empresa = await _context.Empresa.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Nome,CNPJ,Segmento,Estado,Industrial,NomeFantasia,CNAE")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {   // O CONTROLLER PRECISA RECEBER O CAMPO 'ESTADO' E VERIFICAR A QUAL [Name] ELE PERTENCE NA BASE ENUM #########################################
                    empresa.CNPJ = Regex.Replace(empresa.CNPJ, "[^0-9]", "");
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                if (id == null)
                {
                    return NotFound();
                }

                Empresa empresa = await _context.Empresa
                    .FirstOrDefaultAsync(m => m.Id == id);
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
            IQueryable<Pessoa> pessoas_a_remover = _context.Pessoa.Where(p => p.empresa.Id == empresa.Id);
            _context.Pessoa.RemoveRange(pessoas_a_remover);
            _context.Empresa.Remove(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresa.Any(e => e.Id == id);
        }

        public string PuxarEmpresas(bool estrangeiras = false) //APENAS UM OU OUTRO POR ENQUANTO
        {
            List<Empresa> lista_emp = _context.Empresa.ToList();

            List<Dictionary<string, object>> listaFull = new List<Dictionary<string, object>>();

            foreach (var empresa in lista_emp)
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
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict["Id"] = empresa.Id;
                    dict["RazaoSocial"] = empresa.Nome;
                    dict["NomeFantasia"] = empresa.NomeFantasia;
                    dict["Segmento"] = empresa.Segmento.GetDisplayName();
                    dict["Estado"] = empresa.Estado.GetDisplayName();
                    dict["CNPJ"] = empresa.CNPJ;
                    listaFull.Add(dict);
                }
            }

            return JsonSerializer.Serialize(listaFull);
        }

        /*public async Task<IActionResult> RetornarKPIsEmbrapii()
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> RetornarKPIsFraunhofer()
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> IncluirCronograma()
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> AtualizarCronograma()
        {
            throw new NotImplementedException();
        }*/
    }
}