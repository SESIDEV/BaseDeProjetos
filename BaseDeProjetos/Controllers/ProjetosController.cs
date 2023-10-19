using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class ProjetosController : SGIController
    {
        private readonly ApplicationDbContext _context;

        public ProjetosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("/Projetos/RetornarDadosGraficoCFF/{idProjeto}")]
        public async Task<IActionResult> RetornarDadosGraficoCFF(string idProjeto)
        {
            List<object[]> dataPercentualFisico = new List<object[]>();
            List<object[]> dataPercentualFinanceiro = new List<object[]>();

            Dictionary<string, object> dadosGrafico = new Dictionary<string, object>();

            var projeto = await _context.Projeto.FirstOrDefaultAsync(p => p.Id == idProjeto);

            var curvaFisicoFinanceiraSort = projeto.CurvaFisicoFinanceira.OrderBy(p => p.Data);

            foreach (var cff in curvaFisicoFinanceiraSort)
            {
                long timestamp = Helpers.Helpers.DateTimeToUnixTimestamp(cff.Data); // Multiply by 1000 to convert to JavaScript timestamp
                object[] percentualFisico = new object[] { timestamp, cff.PercentualFisico };
                object[] percentualFinanceiro = new object[] { timestamp, cff.PercentualFinanceiro };

                dataPercentualFisico.Add(percentualFisico);
                dataPercentualFinanceiro.Add(percentualFinanceiro);
            }

            dadosGrafico["dadosPercentualFisico"] = dataPercentualFisico;
            dadosGrafico["dadosPercentualFinanceiro"] = dataPercentualFinanceiro;

            var serializedResult = JsonConvert.SerializeObject(dadosGrafico);

            return Ok(serializedResult);
        }

        /// <summary>
        /// Adicionar um indicador e atrela ao projeto
        /// </summary>
        /// <param name="indicadores"></param>
        /// <returns></returns>
        [HttpPost("/Projetos/AdicionarIndicador/{idProjeto}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarIndicador(string idProjeto, [Bind("Regramento, Repasse, ComprasServico, ComprasMaterial, Bolsista, SatisfacaoMetadeProjeto, SatisfacaoFimProjeto, Relatorios, PrestacaoContas")] ProjetoIndicadores indicadores)
        {
            if (ModelState.IsValid)
            {
                Projeto projeto = _context.Projeto.FirstOrDefault(p => p.Id == idProjeto);

                // Gerar id do indicador
                string idIndicador = $"proj_ind_{Guid.NewGuid()}";
                indicadores.Id = idIndicador;

                // Atrelar o projeto ao indicador
                if (projeto.Indicadores != null)
                {
                    if (projeto.Indicadores.Count == 0)
                    {
                        projeto.Indicadores = new List<ProjetoIndicadores> { indicadores };
                    }
                    else
                    {
                        projeto.Indicadores.Add(indicadores);
                    }
                }

                // Criar o indicador no DB
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
        /// Edita uma CFF e atrela ao projeto
        /// </summary>
        /// <param name="cff">Objeto CFF</param>
        /// <returns></returns>
        [HttpPost("/Projetos/EditarCFF/{idCFF}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCFF(int idCFF, [Bind("Id, Data, PercentualFisico, PercentualFinanceiro")] CurvaFisicoFinanceira cff)
        {
            if (idCFF != cff.Id)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                _context.Update(cff);

                await _context.SaveChangesAsync();
            }
            else
            {
                return View("Error");
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Adicionar uma CFF e atrela ao projeto
        /// </summary>
        /// <param name="cff">Objeto CFF</param>
        /// <returns></returns>
        [HttpPost("/Projetos/AdicionarCFF/{idProjeto}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarCFF(string idProjeto, [Bind("Id, Data, PercentualFisico, PercentualFinanceiro")] CurvaFisicoFinanceira cff)
        {
            if (ModelState.IsValid)
            {
                Projeto projeto = _context.Projeto.FirstOrDefault(p => p.Id == idProjeto);

                // Atrelar o projeto ao indicador
                if (projeto.CurvaFisicoFinanceira != null)
                {
                    if (projeto.CurvaFisicoFinanceira.Count == 0)
                    {
                        projeto.CurvaFisicoFinanceira = new List<CurvaFisicoFinanceira> { cff };
                    }
                    else
                    {
                        projeto.CurvaFisicoFinanceira.Add(cff);
                    }
                }

                // Criar o indicador no DB
                await CriarCFF(cff);

                await _context.SaveChangesAsync();
            }
            else
            {
                return View("Error");
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Cria a curva físico financeira no banco
        /// </summary>
        /// <param name="cff">Objeto CurvaFisicoFinanceira</param>
        /// <returns></returns>
        private async Task CriarCFF(CurvaFisicoFinanceira cff)
        {
            await _context.AddAsync(cff);
            await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Index(string casa, string sortOrder = "", string searchString = "", string ano = "")
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
                var usuarios = await _context.Users.ToListAsync();
                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel; // Já vi não funcionar, porquê? :: hhenriques1999
                ViewData["NivelUsuario"] = usuario.Nivel;
                ViewData["IdUsuario"] = usuario.Id;
                ViewData["Usuarios"] = usuarios;

                List<Empresa> empresas = await _context.Empresa.ToListAsync();
                ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");

                SetarFiltros(sortOrder, searchString);
                IQueryable<Projeto> projetos = ObterProjetosPorCasa(casa);
                projetos = PeriodizarProjetos(ano, projetos);
                CategorizarStatusProjetos(projetos);
                await GerarIndicadores(_context);
                return View(projetos.ToList());
            }
            else
            {
                return View("Forbidden");
            }
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
        private async Task GerarIndicadores(ApplicationDbContext _context)
        {
            ViewBag.n_projs = _context.Projeto.Where(p => p.Status == StatusProjeto.EmExecucao).Count();
            ViewBag.n_empresas = _context.Projeto.Count();
            ViewBag.n_revenue = _context.Projeto.
                Where(p => p.Status == StatusProjeto.EmExecucao).
                Select(p => p.ValorAporteRecursos).
                Sum();
            ViewBag.n_pesquisadores = _context.Users.Count();
            ViewBag.Embrapii_projs = await _context.Projeto.ToListAsync();
            ViewBag.Embrapii_prosps = await _context.Prospeccao.ToListAsync();
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
        /// Retorna um modal específico a partir dos parâmetros especificados
        /// </summary>
        /// <param name="idProjeto">ID do CFF</param>
        /// <returns></returns>
        public IActionResult RetornarModalEditCFF(int idCFF)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                return ViewComponent("ModalEditCFFProjeto", new { id = idCFF });
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

        [HttpGet("Projetos/RetornarDadosGrafico")]
        public async Task<IActionResult> RetornarDadosGrafico()
        {
            List<ProjetoGraficoViewModel> projetosGrafico = new List<ProjetoGraficoViewModel>();

            var projetos = await _context.Projeto.ToListAsync();

            // Por instituto obter os tipos de projeto e adicionar ao ViewModel
            foreach (Instituto instituto in Enum.GetValues(typeof(Instituto)))
            {
                projetosGrafico.Add(new ProjetoGraficoViewModel()
                {
                    Casa = instituto.GetDisplayName(),
                    ProjetosAgenciaFomento = projetos.Where(p => p.FonteFomento == TipoContratacao.AgenciaFomento && p.Casa == instituto).ToList().Select(p => p.ToDto()).ToList(),
                    ProjetosANP = projetos.Where(p => p.FonteFomento == TipoContratacao.ANP && p.Casa == instituto).ToList().Select(p => p.ToDto()).ToList(),
                    ProjetosContratacaoDireta = projetos.Where(p => p.FonteFomento == TipoContratacao.ContratacaoDireta && p.Casa == instituto).ToList().Select(p => p.ToDto()).ToList(),
                    ProjetosEditalInovacao = projetos.Where(p => p.FonteFomento == TipoContratacao.EditalInovacao && p.Casa == instituto).ToList().Select(p => p.ToDto()).ToList(),
                    ProjetosEmbrapii = projetos.Where(p => p.FonteFomento == TipoContratacao.Embrapii && p.Casa == instituto).ToList().Select(p => p.ToDto()).ToList(),
                    ProjetosIndefinido = projetos.Where(p => p.FonteFomento == TipoContratacao.Indefinida && p.Casa == instituto).ToList().Select(p => p.ToDto()).ToList(),
                });
            }

            return Ok(JsonConvert.SerializeObject(projetosGrafico));
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
        public async Task<IActionResult> Create([Bind("Id,NomeProjeto,MembrosEquipe,Casa,AreaPesquisa,DataInicio,DataEncerramento,Estado,FonteFomento,Inovacao,Status,DuracaoProjetoEmMeses,ValorTotalProjeto,ValorAporteRecursos,Empresa,Indicadores,UsuarioId,SatisfacaoClienteParcial,SatisfacaoClienteFinal,ProponenteId,CustoHH,CustoHM")]
            Projeto projeto,
            string membrosSelect)
        {
            ViewbagizarUsuario(_context);

            List<Usuario> usuarios = await ObterListaDeMembrosSelecionados(membrosSelect);

            AtribuirEquipeProjeto(projeto, usuarios);

            AtribuirCustoHH(projeto);

            CriarSelectListNaViewbag();

            if (ModelState.IsValid)
            {
                projeto.Empresa = _context.Empresa.FirstOrDefault(e => e.Id == projeto.Empresa.Id);

                _context.Add(projeto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            else
            {
                return NotFound();
            }
        }

        public static int DiferencaMeses(DateTime dataFim, DateTime dataInicio)
        {
            return Math.Abs((dataFim.Month - dataInicio.Month) + 12 * (dataFim.Year - dataInicio.Year));
        }

        private void AtribuirCustoHH(Projeto projeto)
        {
            projeto.CustoHH = 0;

            // Soma do custo da equipe (membros)
            foreach (var relacao in projeto.EquipeProjeto)
            {
                if (relacao.Usuario != null)
                {
                    projeto.CustoHH += DiferencaMeses(projeto.DataEncerramento, projeto.DataInicio) * relacao.Usuario.Cargo.Salario;
                }
            }

            // Soma do custo do líder
            if (projeto.Usuario != null)
            {
                if (projeto.Usuario.Cargo != null)
                {
                    projeto.CustoHH += DiferencaMeses(projeto.DataEncerramento, projeto.DataInicio) * projeto.Usuario.Cargo.Salario;
                }
            }
        }

        private async Task<List<Usuario>> ObterListaDeMembrosSelecionados(string membrosSelect)
        {
            List<string> membrosEmails = new List<string>();

            // TODO: Repensar a forma como o frontend implementa essa funcionalidade
            if (!string.IsNullOrEmpty(membrosSelect))
            {
                membrosEmails.AddRange(membrosSelect.Split(';').ToList());
            }

            List<Usuario> usuarios = await _context.Users.Where(u => membrosEmails.Contains(u.Email)).ToListAsync();
            return usuarios;
        }

        private void CriarSelectListNaViewbag()
        {
            List<Empresa> empresas = _context.Empresa.ToList();
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
        }

        private static void AtribuirEquipeProjeto(Projeto projeto, List<Usuario> usuarios)
        {
            List<EquipeProjeto> equipe = new List<EquipeProjeto>();

            foreach (var usuario in usuarios)
            {
                equipe.Add(new EquipeProjeto { IdUsuario = usuario.Id, IdTrabalho = projeto.Id });
            }

            projeto.EquipeProjeto = equipe;
        }

        // GET: Projetos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);
                List<Empresa> empresas = _context.Empresa.ToList();
                ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
                ViewData["Equipe"] = new SelectList(_context.Users.ToList(), "Id", "UserName");

                if (id == null)
                {
                    return View("Error");
                }

                Projeto projeto = await _context.Projeto.FindAsync(id);

                if (projeto == null)
                {
                    return View("Error");
                }

                return View(projeto);
            }
            else
            {
                return View("Forbidden");
            }
        }

        //POST: Projetos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,
            [Bind("Id,Casa,NomeProjeto,MembrosEquipe,AreaPesquisa,DataInicio,DataEncerramento,Estado,FonteFomento,Inovacao,Status,DuracaoProjetoEmMeses,ValorTotalProjeto,ValorAporteRecursos,Indicadores,UsuarioId,SatisfacaoClienteParcial,SatisfacaoClienteFinal,ProponenteId,CustoHH,CustoHM")]
            Projeto projeto,
            string membrosSelect)
        {
            List<EquipeProjeto> equipe = new List<EquipeProjeto>();
            var projetoExistente = await _context.Projeto.AsNoTracking().Include(p => p.Usuario).Include(p => p.EquipeProjeto).Include(p => p.Indicadores).FirstOrDefaultAsync(p => p.Id == projeto.Id);

            if (projetoExistente == null)
            {
                return View("Error");
            }

            foreach (var relacao in projetoExistente.EquipeProjeto)
            {
                _context.EquipeProjeto.Remove(relacao);
            }

            await _context.SaveChangesAsync();

            _context.Entry(projetoExistente).CurrentValues.SetValues(projeto);

            List<string> membrosEmails = new List<string>();

            // TODO: Repensar a forma como o frontend implementa essa funcionalidade
            if (!string.IsNullOrEmpty(membrosSelect))
            {
                membrosEmails.AddRange(membrosSelect.Split(';').ToList());
            }

            List<Usuario> usuarios = await _context.Users.Where(u => membrosEmails.Contains(u.Email)).ToListAsync();

            foreach (var usuario in usuarios)
            {
                var equipeProjeto = new EquipeProjeto { IdUsuario = usuario.Id, IdTrabalho = projeto.Id };
                equipe.Add(equipeProjeto);
            }

            projetoExistente.EquipeProjeto = equipe;

            /*
             * Salvamos as alterações de equipe no banco pois enviamos apenas o ID do Projeto e do Usuário
             * Se não efetuarmos o salvamento, o método seguinte AtribuirCustoHH apenas enxergará os IDs
             * e não os objetos para Projeto e Usuário pois o relacionamento estará ""fraco""
             * Sem sombra de dúvidas que existe uma forma "mais correta" de implementar essa funcionalidade.
             * Mas acho desnecessário ir no banco em código para puxar o Projeto e o Usuário novamente pelos IDs
            */
            await _context.SaveChangesAsync();

            AtribuirCustoHH(projetoExistente);

            if (id != projeto.Id)
            {
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    throw ex;
                }
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            else
            {
                return View("Error");
            }
        }

        // GET: Projetos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

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
        public async Task<IActionResult> DeletarIndicador(string id)
        {
            ProjetoIndicadores indicador = await _context.ProjetoIndicadores.FindAsync(id);
            _context.ProjetoIndicadores.Remove(indicador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarCurvaFisicoFinanceira(int id)
        {
            CurvaFisicoFinanceira cff = await _context.CurvaFisicoFinanceira.FindAsync(id);
            _context.CurvaFisicoFinanceira.Remove(cff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Obtem os membros em CSV do projeto dado o id
        /// </summary>
        /// <returns>A equipe de um projeto separadas por ponto e virgula</returns>
        [HttpGet("Projetos/RetornarMembrosCSV/{idProjeto}")]
        public async Task<IActionResult> RetornarMembrosCSV(string idProjeto)
        {
            string membros = string.Join(";", _context.Projeto.FirstOrDefault(p => p.Id == idProjeto)?.EquipeProjeto.Select(relacao => relacao.Usuario.Email));

            Dictionary<string, string> dados = new Dictionary<string, string> { { "data", membros } };

            return Ok(JsonConvert.SerializeObject(dados));
        }

        // POST: Projetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Projeto projeto = await _context.Projeto.FindAsync(id);

            foreach (var relacao in projeto.EquipeProjeto)
            {
                _context.EquipeProjeto.Remove(relacao);
            }

            foreach (var indicador in projeto.Indicadores)
            {
                _context.ProjetoIndicadores.Remove(indicador);
            }

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
                ViewbagizarUsuario(_context);
                List<string> usuarios = _context.Users.AsEnumerable()
                                            .Where(u => u.Casa == UsuarioAtivo.Casa)
                                            .Where(usuario => usuario.EmailConfirmed == true)
                                            .Where(usuario => usuario.Nivel != Nivel.Dev && usuario.Nivel != Nivel.Externos)
                                            .Select((usuario) => usuario.Email.ToString().ToLower())
                                            .ToList();

                return Json(usuarios);
            }
            else
            {
                return Json("403 Forbidden");
            }
        }
    }
}