using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using MailSenderHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class ProjetosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjetosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atualizar([Bind("IdProjeto, Regramento, Repasse, ComprasServico, ComprasMaterial, Bolsista, SatisfacaoMetadeProjeto, SatisfacaoFimProjeto, Relatorios, PrestacaoContas")] ProjetoIndicadores indicadores)
        {
            if (ModelState.IsValid)
            {
                Projeto projeto = _context.Projeto.FirstOrDefault(p => p.Id == indicadores.IdProjeto);

                // Gerar id do indicador
                string idIndicador = $"proj_ind_{Guid.NewGuid()}";
                indicadores.Id = idIndicador;

                // Atrelar o projeto ao indicador
                indicadores.Projeto = projeto; 

                // Criar o indicador da macroentrega no DB
                await CriarIndicadores(indicadores);

                await _context.SaveChangesAsync();
            }
            else
            {
                return View("Error");
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Cria os indicadores no Banco de Dados
        /// </summary>
        /// <param name="indicadoresProjeto"></param>
        /// <returns></returns>
        private async Task CriarIndicadores(ProjetoIndicadores indicadoresProjeto)
        {
            await _context.AddAsync(indicadoresProjeto);
            await _context.SaveChangesAsync();
        }

        public IActionResult PopularBase()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                string[] _base;
                using (StreamReader file = new StreamReader("~/Base.csv"))
                {
                    _base = file.ReadToEnd().Split("\n");
                }

                return View(nameof(Index));
            }
            else
            {
                return View("Forbidden");
            }
        }

        // GET: Projetos
        [HttpGet]
        public IActionResult Index(string casa, string sortOrder = "", string searchString = "", string ano = "")
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel; // Já vi não funcionar, porquê? :: hhenriques1999
                ViewData["NivelUsuario"] = usuario.Nivel;

                List<Empresa> empresas = _context.Empresa.ToList();
                ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");

                SetarFiltros(sortOrder, searchString);
                IQueryable<Projeto> projetos = ObterProjetosPorCasa(casa);
                projetos = PeriodizarProjetos(ano, projetos);
                CategorizarStatusProjetos(projetos);
                GerarIndicadores(casa, _context);
                return View(projetos.ToList());
            }
            else
            {
                return View("Forbidden");
            }
        }

        [HttpGet("Projetos/indicadores-projetos")]
        public async Task<IActionResult> IndicadoresDeProjeto()
        {
            List<Projeto> listaProjetos = await _context.Projeto.ToListAsync();
            //int SatisfacaoMetadeProjetoMax = listaProjetos.Max<Projeto>(p => p.Indicador.ValorSatisfacaoMetadeProjeto);

            //int SatisfacaoMetadeProjetoMin = listaProjetos.Min<Projeto>(p => p.Indicador.ValorSatisfacaoMetadeProjeto);

            return View("indicador_projetos", listaProjetos);
        }

        /// <summary>
        /// Configura os filtros para a ordenação e busca de acordo com os parâmetros passados
        /// </summary>
        /// <param name="sortOrder">Filtro de ordenação</param>
        /// <param name="searchString">Filtro de termo de busca</param>
        private void SetarFiltros(string sortOrder = "", string searchString = "")
        {
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

            if (string.IsNullOrEmpty(sortOrder) && HttpContext.Session.Keys.Contains("_asdas"))
            {
                ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewData["DateSortParm"] = sortOrder == "TipoContratacao" ? "tipo_desc" : "TipoContratacao";
            }
            else
            {
                ViewData["CurrentFilter"] = sortOrder;
                HttpContext.Session.SetString("____", sortOrder);
            }
        }

        /// <summary>
        /// Categoriza na View (ViewBag) os projetos como Ativos ou Encerrados
        /// </summary>
        /// <param name="projetos">Projetos a serem categorizados</param>
        private void CategorizarStatusProjetos(IQueryable<Projeto> projetos)
        {
            ViewBag.Ativos = projetos.Where(p => p.Status == StatusProjeto.EmExecucao || p.Status == StatusProjeto.Contratado).ToList();
            ViewBag.Encerrados = projetos.Where(p => p.Status == StatusProjeto.Concluido || p.Status == StatusProjeto.Cancelado).ToList();
        }

        /// <summary>
        /// Retorna uma lista de projetos até um ano limite especificado
        /// </summary>
        /// <param name="ano">Ano limite</param>
        /// <param name="lista">Lista contendo projetos a serem filtrados e retornados</param>
        /// <returns></returns>
        private IQueryable<Projeto> PeriodizarProjetos(string ano, IQueryable<Projeto> lista)
        {
            if (ano.Equals("Todos") || string.IsNullOrEmpty(ano))
            {
                return lista;
            }

            int ano_limite = Convert.ToInt32(ano);
            return lista.Where(s => s.DataEncerramento.Year >= ano_limite);
        }

        /// <summary>
        /// Define na ViewBag indicadores para o módulo de projetos
        /// </summary>
        /// <param name="casa"></param>
        /// <param name="_context"></param>
        private void GerarIndicadores(string casa, ApplicationDbContext _context)
        {
            ViewBag.n_projs = _context.Projeto.Where(p => p.Status == StatusProjeto.EmExecucao).Count();
            ViewBag.n_empresas = _context.Projeto.Count();
            ViewBag.n_revenue = _context.Projeto.
                Where(p => p.Status == StatusProjeto.EmExecucao).
                Select(p => p.ValorAporteRecursos).
                Sum();
            ViewBag.n_pesquisadores = _context.Users.Count();
            ViewBag.Embrapii_projs = _context.Projeto.ToList<Projeto>();
            ViewBag.Embrapii_prosps = _context.Prospeccao.ToList<Prospeccao>();
        }

        /// <summary>
        /// Retorna uma lista contendo todos os projetos, filtrados pelo Instituto especificado
        /// </summary>
        /// <param name="casa">Nome do Instituto</param>
        /// <returns></returns>
        private IQueryable<Projeto> ObterProjetosPorCasa(string casa)
        {
            Instituto enum_casa;

            if (string.IsNullOrEmpty(casa))
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Casa")))
                {
                    enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
                }
                else { enum_casa = Instituto.Super; }
            }
            else
            {
                if (Enum.IsDefined(typeof(Instituto), casa))
                {
                    HttpContext.Session.SetString("_Casa", casa);
                    enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
                }
                else
                {
                    enum_casa = Instituto.Super;
                }
            }

            ViewData["Area"] = casa;

            IQueryable<Projeto> lista = enum_casa == Instituto.Super ?
                _context.Projeto :
                _context.Projeto.Where(p => p.Casa.Equals(enum_casa));

            return lista;
        }

        /// <summary>
        /// Retorna um modal específico a partir dos parâmetros especificados
        /// </summary>
        /// <param name="idProjeto">ID do Projeto</param>
        /// <param name="tipo">Tipo de Modal a ser retornado</param>
        /// <returns></returns>
        public IActionResult RetornarModal(string idProjeto, string tipo)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;
                ViewData["NivelUsuario"] = usuario.Nivel;

                if (!string.IsNullOrEmpty(tipo))
                {
                    return ViewComponent($"Modal{tipo}Projeto", new { id = idProjeto });
                }
                else
                {
                    return ViewComponent("null"); // View n existe é mais pra ""debug""
                }
            }
            else
            {
                return View("Forbidden");
            }

        }

        /// <summary>
        /// Retorna o modal de detalhes dado o ID de um projeto
        /// </summary>
        /// <param name="idProjeto">ID do projeto</param>
        /// <returns></returns>
        public IActionResult RetornarModalDetalhes(string idProjeto)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return ViewComponent($"ModalDetalhesProjeto", new { id = idProjeto });
            }
            else
            {
                return View("Forbidden");
            }
        }

        /// <summary>
        /// Verifica se um projeto existe no banco de dados
        /// </summary>
        /// <param name="id">ID do projeto</param>
        /// <returns></returns>
        private bool ValidarProjetoId(string id)
        {
            if (id == null)
            {
                return false;
            }

            Projeto projeto = _context.Projeto.FirstOrDefault(p => p.Id == id);

            if (projeto == null)
            {
                return false;
            }

            return true;
        }

        // GET: Projetos/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
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

                Projeto projeto = await _context.Projeto
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (projeto == null)
                {
                    return NotFound();
                }

                return View(projeto);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // GET: Projetos/Create
        [HttpGet]
        public IActionResult Create()
        {

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                List<Empresa> empresas = _context.Empresa.ToList();
                ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
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

        // POST: Projetos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeProjeto,MembrosEquipe,Casa,AreaPesquisa,DataInicio,DataEncerramento,Estado,FonteFomento,Inovacao,Status,DuracaoProjetoEmMeses,ValorTotalProjeto,ValorAporteRecursos,Empresa,Indicadores")] Projeto projeto)
        {
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

            List<Empresa> empresas = _context.Empresa.ToList();
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");

            if (ModelState.IsValid)
            {
                projeto.Empresa = _context.Empresa.FirstOrDefault(e => e.Id == projeto.Empresa.Id);

                //projeto.Usuario = usuario;

                _context.Add(projeto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            } 
            else
            {
                return NotFound();
            }
        }

        // GET: Projetos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;
                List<Empresa> empresas = _context.Empresa.ToList();
                ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
                ViewData["Equipe"] = new SelectList(_context.Users.ToList(), "Id", "UserName");

                if (id == null)
                {
                    return NotFound();
                }

                Projeto projeto = await _context.Projeto.FindAsync(id);

                if (projeto == null)
                {
                    return NotFound();
                }

                //ConfigurarEquipe(projeto);

                return View(projeto);
            }
            else
            {
                return View("Forbidden");
            }
        }

        /*private static void ConfigurarEquipe(Projeto projeto)
        {
            if (projeto.Equipe.Count() < 2)
            {
                if (projeto.Equipe.Count() == 0)
                {
                    projeto.Equipe = new List<Usuario>() { new Usuario(), new Usuario() };
                }
                else
                {
                    projeto.Equipe.Add(new Usuario());
                }
            }
        }*/

        //POST: Projetos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Casa,NomeProjeto, MembrosEquipe,AreaPesquisa,DataInicio,DataEncerramento,Estado,FonteFomento,Inovacao,Status,DuracaoProjetoEmMeses,ValorTotalProjeto,ValorAporteRecursos,Indicadores")] Projeto projeto)
        {
            if (id != projeto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Projeto.Update(projeto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjetoExists(projeto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            return View(projeto);
        }

        /*private List<Usuario> ObterUsuarios(Projeto projeto)
            {
                List<Usuario> usuarios_reais = new List<Usuario>();

                for (int i = 0; i < projeto.Equipe.Count(); i++)
                {
                    usuarios_reais.Add(_context.Users.First(p => p.Id == projeto.Equipe[i].Id));
                }

                return usuarios_reais;
            }*/

        // GET: Projetos/Delete/5
        public async Task<IActionResult> Delete(string id)
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

                Projeto projeto = await _context.Projeto
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (projeto == null)
                {
                    return NotFound();
                }

                return View(projeto);
            }
            else
            {
                return View("Forbidden");
            }
        }


        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarMacroentrega(string id)
        {
            ProjetoIndicadores indicador = await _context.ProjetoIndicadores.FindAsync(id);
            _context.ProjetoIndicadores.Remove(indicador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Projetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Projeto projeto = await _context.Projeto.FindAsync(id);
            _context.Projeto.Remove(projeto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica se um projeto existe no Banco de Dados
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool ProjetoExists(string id)
        {
            return _context.Projeto.Any(e => e.Id == id);
        }

        private void DetachLocal(Func<Usuario, bool> predicate)
        {
            Usuario local = _context.Set<Usuario>().Local.Where(predicate).FirstOrDefault();
            if (!(local is null))
            {
                _context.Entry(local).State = EntityState.Detached;
            }
        }


        [HttpPost]
        public IActionResult CarregarProjetos(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            if (files.Count < 1)
            {
                return Content("Não foram submetidos dados");
            }

            foreach (IFormFile formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using StreamReader file = new StreamReader(formFile.OpenReadStream());
                    CriarProjetos(file);
                }
            }

            return View("Index", "Home");
        }

        private void CriarProjetos(StreamReader file)
        {
            List<string> lines = file.ReadToEnd().Split(Environment.NewLine).Skip(1).ToList();

            foreach (string line in lines)
            {
                string[] dados = line.Split("|");
                _ = double.TryParse(Regex.Replace(dados[10], @"[^\d]", ""), out double valorTotal);
                _ = double.TryParse(Regex.Replace(dados[11], @"[^\d]", ""), out double valorAporte);

                Projeto projeto = new Projeto
                {
                    Id = $"Proj_{DateTime.Now.Ticks}",
                    Casa = Enum.Parse<Instituto>(dados[0], true),
                    NomeProjeto = dados[3],
                    Empresa = new Empresa { Nome = dados[2] },
                    AreaPesquisa = Enum.IsDefined(typeof(LinhaPesquisa), dados[1]) ? Enum.Parse<LinhaPesquisa>(dados[1]) : LinhaPesquisa.Indefinida,
                    Estado = Enum.IsDefined(typeof(Estado), dados[1].Replace(" ", "")) ? Enum.Parse<Estado>(dados[1]) : Estado.Estrangeiro,
                    ValorAporteRecursos = valorAporte > 0 ? valorAporte : valorTotal,
                    ValorTotalProjeto = valorTotal,
                    Inovacao = Enum.IsDefined(typeof(TipoInovacao), dados[8]) ? Enum.Parse<TipoInovacao>(dados[8]) : TipoInovacao.Processo,
                    Status = Enum.IsDefined(typeof(StatusProjeto), dados[15]) ? Enum.Parse<StatusProjeto>(dados[15]) : StatusProjeto.EmExecucao,
                    FonteFomento = Enum.IsDefined(typeof(TipoContratacao), dados[7]) ? Enum.Parse<TipoContratacao>(dados[7]) : TipoContratacao.Indefinida,
                    DuracaoProjetoEmMeses = Convert.ToInt32(dados[17])
                };

                _context.Add(projeto);
                _context.SaveChanges();
            }

        }

        /// <summary>
        /// Retorna um JSON contendo a lista de usuários, ?a partir do usuário logado?
        /// </summary>
        /// <returns></returns>
        public JsonResult ListaUsuarios()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario ativo = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
                List<string> usuarios = _context.Users.AsEnumerable().
                                            Where(u => u.Casa == ativo.Casa).
                                            Where(usuario => usuario.EmailConfirmed == true).
                                            Where(usuario => usuario.Nivel != Nivel.Dev && usuario.Nivel != Nivel.Externos).
                                            Select((usuario) => usuario.Email.ToString().ToLower()).
                                            ToList();

                return Json(usuarios);
            }
            else
            {
                return Json("403 Forbidden");
            }
        }
    }
}
