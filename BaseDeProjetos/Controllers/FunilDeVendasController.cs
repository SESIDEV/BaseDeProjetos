using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("TestesBaseDeProjetos1")]

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class FunilDeVendasController : SGIController
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _mailer;

        private readonly DbCache _cache;

        public FunilDeVendasController(ApplicationDbContext context, IEmailSender mailer, DbCache cache)
        {
            _context = context;
            _mailer = mailer;
            _cache = cache;
        }

        // GET: FunilDeVendas
        [Route("FunilDeVendas/Index/{casa?}/{aba?}/{ano?}")]
        public async Task<IActionResult> Index(string casa, string aba, string sortOrder = "", string searchString = "", string ano = "", int numeroPagina = 1, int tamanhoPagina = 20)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                var prospeccoes = await _cache.GetCachedAsync("AllProspeccoes", () => _context.Prospeccao.ToListAsync());

                ViewBag.searchString = searchString;
                ViewBag.TamanhoPagina = tamanhoPagina;

                if (string.IsNullOrEmpty(casa))
                {
                    casa = UsuarioAtivo.Casa.ToString();
                }

                var empresas = await _cache.GetCachedAsync("EmpresasFunil", () => _context.Empresa.Select(e => new EmpresasFunilDTO { Id = e.Id, Nome = e.Nome }).ToListAsync());
                prospeccoes = await ObterProspeccoesFunilFiltradas(casa, aba, sortOrder, searchString, ano, UsuarioAtivo);

                int qtdProspeccoes = prospeccoes.Count();
                int qtdPaginasTodo = (int)Math.Ceiling((double)qtdProspeccoes / (double)tamanhoPagina);

                List<Prospeccao> prospeccoesPagina = ObterProspeccoesPorPagina(prospeccoes, numeroPagina, (int)tamanhoPagina);

                var pager = new Pager(qtdProspeccoes, numeroPagina, (int)tamanhoPagina, 50); // 50 paginas max

                var model = new ProspeccoesViewModel
                {
                    Prospeccoes = prospeccoesPagina,
                    Pager = pager,
                };

                var usuarios = await _cache.GetCachedAsync("Usuarios:Funil", () => _context.Users.Select(u => new UsuariosFunilDTO { Id = u.Id, UserName = u.UserName, Email = u.Email }).ToListAsync());

                ViewData["Usuarios"] = usuarios;
                ViewData["Empresas"] = new SelectList(empresas, "Id", "Nome");
                ViewData["Equipe"] = new SelectList(usuarios, "Id", "UserName");

                var prospeccoesParaFiltragemAgregadas = await _cache.GetCachedAsync("AllProspeccoes", () => _context.Prospeccao.ToListAsync());
                ViewData["ProspeccoesAgregadas"] = prospeccoesParaFiltragemAgregadas.Where(p => p.Status.OrderBy(k => k.Data).Last().Status == StatusProspeccao.Agregada).ToList();

                if (!string.IsNullOrEmpty(aba))
                {
                    return View(model);
                }
                else
                {
                    ViewData["ListaProspeccoes"] = prospeccoes.ToList();
                    ViewData["ProspeccoesAtivas"] = prospeccoes.Where(
                        p => p.Status.OrderBy(k => k.Data).All(
                            pa => pa.Status == StatusProspeccao.ContatoInicial || pa.Status == StatusProspeccao.Discussao_EsbocoProjeto || pa.Status == StatusProspeccao.ComProposta)).ToList();
                    ViewData["ProspeccoesTotais"] = prospeccoes.Where(p => p.Status.OrderBy(k => k.Data).Any(pa => pa.Status != StatusProspeccao.Planejada)).ToList();
                    ViewData["ProspeccoesNaoPlanejadas"] = prospeccoes.Where(p => p.Status.OrderBy(k => k.Data).Any(f => f.Status != StatusProspeccao.Planejada)).ToList();
                    ViewData["ProspeccoesAvancadas"] = prospeccoes.Where(
                        p => p.Status.Any(k => k.Status == StatusProspeccao.ComProposta)).Where(
                            p => p.Status.Any(k => k.Status > StatusProspeccao.ComProposta)).Where(
                                p => (p.Status.First().Data - p.Status.FirstOrDefault(
                                    s => s.Status == StatusProspeccao.ComProposta).Data) > TimeSpan.Zero).ToList(); // filtrar lista para obter datas positivas (maior que zero)
                    return View(model);
                }

            }
            else
            {
                return View("Forbidden");
            }

        }

        /// <summary>
        /// Obtém as prospecções específicas de uma página
        /// </summary>
        /// <param name="prospeccoes">Lista contendo prospecções a serem paginadas</param>
        /// <param name="numeroPagina">Número da página que se deseja obter as prospeções</param>
        /// <param name="tamanhoPagina">Quantidade de prospecções por página</param>
        /// <returns></returns>
        private List<Prospeccao> ObterProspeccoesPorPagina(List<Prospeccao> prospeccoes, int numeroPagina, int tamanhoPagina)
        {
            return prospeccoes.Skip((numeroPagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToList();
        }

        /// <summary>
        /// Retorna uma lista de prospecções filtradas de acordo com os parâmetros
        /// </summary>
        /// <param name="casa">Casa das prospecções</param>
        /// <param name="aba">Aba da View/Status as quais as prospecções devem obedecer</param>
        /// <param name="sortOrder">Forma de ordenação das prospecções</param>
        /// <param name="searchString">Parâmetro de busca para filtro das prospecções</param>
        /// <param name="ano">Ano das prospecções</param>
        /// <param name="usuario">Usuário das prospecções</param>
        /// <returns></returns>
        private async Task<List<Prospeccao>> ObterProspeccoesFunilFiltradas(string casa, string aba, string sortOrder, string searchString, string ano, Usuario usuario)
        {
            List<Prospeccao> prospeccoes = await FunilHelpers.DefinirCasaParaVisualizar(casa, usuario, _context, HttpContext, ViewData);

            if (!string.IsNullOrEmpty(ano))
            {
                FunilHelpers.PeriodizarProspecções(ano, prospeccoes);
            }

            prospeccoes = FunilHelpers.OrdenarProspecções(sortOrder, prospeccoes); //SORT ORDEM ALFABETICA
            prospeccoes = FunilHelpers.FiltrarProspecções(searchString, prospeccoes); // APENAS NA BUSCA

            FunilHelpers.SetarFiltrosNaView(HttpContext, ViewData, sortOrder, searchString);

            if (!string.IsNullOrEmpty(aba))
            {
                prospeccoes = FunilHelpers.RetornarProspeccoesPorStatus(prospeccoes, usuario, aba, HttpContext);
            }

            return prospeccoes;
        }

        /// <summary>
        /// Planeja uma prospecção quando criada à partir do Módulo de Empresas
        /// </summary>
        /// <param name="id">ID da Empresa</param>
        /// <param name="userId">ID do Usuário</param>
        /// <returns></returns>
        public async Task<IActionResult> Planejar(int id, string userId)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                var empresas = await _cache.GetCachedAsync("AllEmpresas", () => _context.Empresa.ToListAsync());
                Prospeccao prosp = new Prospeccao
                {
                    Id = $"prosp_{DateTime.Now.Ticks}",
                    Empresa = empresas.FirstOrDefault(e => e.Id == id),
                    Usuario = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userId),
                    Casa = UsuarioAtivo.Casa,
                    LinhaPequisa = LinhaPesquisa.Indefinida,
                    CaminhoPasta = ""
                };
                prosp.Status = new List<FollowUp>
                {
                    new FollowUp
                    {
                        OrigemID = prosp.Id,
                        Data = DateTime.Today,
                        Anotacoes = $"Incluído no plano de prospecção de {User.Identity.Name}",
                        Status = StatusProspeccao.Planejada
                    }
                };

                await _context.AddAsync(prosp);
                await _context.SaveChangesAsync();
                await CacheHelper.CleanupProspeccoesCache(_cache);
                return RedirectToAction("Index", "Empresas");
            }
            else
            {
                return View("Forbidden");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Atualizar([Bind("OrigemID, Data, Status, Anotacoes, MotivoNaoConversao")] FollowUp followup)
        {
            ViewbagizarUsuario(_context);

            if (ModelState.IsValid)
            {
                Prospeccao prospeccao_origem = await _context.Prospeccao.FirstOrDefaultAsync(p => p.Id == followup.OrigemID);
                followup.Origem = prospeccao_origem;

                await CriarFollowUp(followup);

                bool enviou = MailHelper.NotificarProspecção(followup, _mailer);
                await _context.SaveChangesAsync();
                await CacheHelper.CleanupProspeccoesCache(_cache);
            }

            return RedirectToAction(nameof(Index), new { casa = UsuarioAtivo.Casa });
        }

        /// <summary>
        /// Cria um followup no Banco de Dados
        /// </summary>
        /// <param name="followup">Followup específico a ser criado no Banco de Dados</param>
        private async Task CriarFollowUp(FollowUp followup)
        {
            await _context.AddAsync(followup);
            await _context.SaveChangesAsync();
        }

        // POST: FunilDeVendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Status, MembrosEquipe, Empresa, Contato, Casa, CaminhoPasta, Tags, Origem, Ancora, Agregadas")] Prospeccao prospeccao)
        {
            ViewbagizarUsuario(_context);

            if (ModelState.IsValid)
            {
                try
                {
                    prospeccao = await ValidarEmpresa(prospeccao);
                }
                catch (Exception e)
                {
                    return CapturarErro(e);
                }
                prospeccao.Contato.empresa = prospeccao.Empresa; // ????
                await VincularUsuario(prospeccao, HttpContext, _context);

                prospeccao.Status[0].Origem = prospeccao;

                bool enviou = MailHelper.NotificarProspecção(prospeccao.Status[0], _mailer);

                await _context.AddAsync(prospeccao);
                await _context.SaveChangesAsync();
                await CacheHelper.CleanupProspeccoesCache(_cache);
                return RedirectToAction(nameof(Index), new { casa = UsuarioAtivo.Casa });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return RedirectToAction(nameof(Index), new { casa = UsuarioAtivo.Casa });
        }

        /// <summary>
        /// Vincula o usuário logado a uma prospecção
        /// </summary>
        /// <param name="prospeccao">Prospecção que se deseja vincular ao usuário logado</param>
        /// <returns></returns>
        internal static async Task VincularUsuario(Prospeccao prospeccao, HttpContext httpContext, ApplicationDbContext context)
        {
            string userId = httpContext.User.Identity.Name;
            Usuario user = await context.Users.FirstAsync(u => u.UserName == userId);
            prospeccao.Usuario = user;
        }

        // TODO: Conceito SOLID quebrado, desmembrar?
        /// <summary>
        /// Valida e cadastra uma empresa a uma prospecção
        /// </summary>
        /// <param name="prospeccao">Prospecção a ter uma empresa cadastrada</param>
        /// <returns></returns>
        /// <exception cref="Exception">Erro a ser lançado caso não seja possível cadastrar a empresa/seja uma empresa inválida</exception>
        public async Task<Prospeccao> ValidarEmpresa(Prospeccao prospeccao)
        {
            if (prospeccao.Empresa.Nome != null && prospeccao.Empresa.CNPJ != null && prospeccao.Empresa.Id == -1)
            {
                Empresa atual = new Empresa { Estado = prospeccao.Empresa.Estado, CNPJ = prospeccao.Empresa.CNPJ, Nome = prospeccao.Empresa.Nome, Segmento = prospeccao.Empresa.Segmento };
                if (atual.Nome != " " && atual.CNPJ != " ")
                {
                    prospeccao.Empresa = atual;
                }
                else
                {
                    throw new Exception("Ocorreu um erro no registro da empresa. \n As informações da empresa não foram submetidas ao banco. \n Contacte um administrador do sistema");
                }
            }
            else
            {
                var empresas = await _cache.GetCachedAsync("AllEmpresas", () => _context.Empresa.ToListAsync());
                var empresa = empresas.FirstOrDefault(e => e.Id == prospeccao.Empresa.Id);

                if (empresa == null)
                {
                    throw new Exception("Ocorreu um erro no registro da empresa. \n A empresa selecionada não foi encontrada. \n Contacte um administrador do sistema");
                }
                else
                {
                    prospeccao.Empresa = empresa;
                }
            }

            return prospeccao;
        }

        /// <summary>
        /// Cria uma selectlist para a View(?)
        /// </summary>
        private async Task CriarSelectListsDaView()
        {
            var empresas = await _cache.GetCachedAsync("EmpresasFunilUnique", () => _context.Empresa.Select(e => new EmpresasFunilComUniqueDTO { EmpresaUnique = e.EmpresaUnique, Id = e.Id, Nome = e.Nome }).ToListAsync());
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
            ViewData["Equipe"] = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
        }

        // POST: FunilDeVendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Empresa, Contato, Casa, Usuario, MembrosEquipe, ValorProposta, ValorEstimado, Status, CaminhoPasta, Tags, Origem, Ancora, Agregadas")] Prospeccao prospeccao)
        {
            ViewbagizarUsuario(_context);

            if (id != prospeccao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    prospeccao = await EditarDadosDaProspecção(id, prospeccao);
                    await _context.SaveChangesAsync();
                    await CacheHelper.CleanupProspeccoesCache(_cache);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunilHelpers.ProspeccaoExists(prospeccao.Id, _context))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Outro erro de banco, lançar para depuração
                    }
                }
                return RedirectToAction("Index", "FunilDeVendas", new { casa = UsuarioAtivo.Casa });
            }
            return View(prospeccao);
        }

        /// <summary>
        /// Método auxiliar para editar dados de uma prospecção
        /// </summary>
        /// <param name="id">Inutilizado</param>
        /// <param name="prospeccao">Prospecção a ter seus dados editados</param>
        /// <returns></returns>
        private async Task<Prospeccao> EditarDadosDaProspecção(string id, Prospeccao prospeccao)
        {
            var empresas = await _cache.GetCachedAsync("AllEmpresas", () => _context.Empresa.ToListAsync());
            Empresa empresaAntigo = empresas.First(e => e.Id == prospeccao.Empresa.Id);
            prospeccao.Empresa = empresaAntigo;
            var usuarios = await _cache.GetCachedAsync("AllUsuarios", () => _context.Users.ToListAsync());
            Usuario lider = usuarios.First(p => p.Id == prospeccao.Usuario.Id);
            prospeccao.Usuario = lider;

            Prospeccao prospAntiga = await _context.Prospeccao.AsNoTracking().FirstAsync(p => p.Id == prospeccao.Id);

            // tudo abaixo compara a versão antiga com a nova que irá para o Update()

            if (prospAntiga.Ancora == true && prospeccao.Ancora == false)
            { // verifica se a âncora foi cancelada
                FunilHelpers.RepassarStatusAoCancelarAncora(_context, prospeccao);
            }
            else if (prospeccao.Ancora == true && string.IsNullOrEmpty(prospeccao.Agregadas))
            { // verifica se o campo agg está vazio
                throw new InvalidOperationException("Não é possível adicionar uma Âncora sem nenhuma agregada.");
            }
            else if (prospAntiga.Agregadas != prospeccao.Agregadas)
            { // verifica se alguma agregada foi alterada
                FunilHelpers.AddAgregadas(_context, prospAntiga, prospeccao);
                FunilHelpers.DelAgregadas(_context, prospAntiga, prospeccao);
            }

            _context.Update(prospeccao);
            return prospeccao;
        }

        public async Task<IActionResult> EditarFollowUp(int? id) // Retornar view
        {
            ViewbagizarUsuario(_context);

            if (id == null)
            {
                return NotFound();
            }

            FollowUp followup = await _context.FollowUp.FindAsync(id);
            ViewData["origem"] = followup.OrigemID;
            ViewData["prosp"] = followup.Origem;

            if (followup == null)
            {
                return NotFound();
            }
            return View(followup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarFollowUp(int id, [Bind("Id", "OrigemID", "Status", "Anotacoes", "Data", "Vencimento")] FollowUp followup)
        {
            ViewbagizarUsuario(_context);

            if (id != followup.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Update(followup);
                await _context.SaveChangesAsync();
                await CacheHelper.CleanupProspeccoesCache(_cache);
            }

            return RedirectToAction("Index", "FunilDeVendas", new { casa = UsuarioAtivo.Casa });
        }

        // GET: FunilDeVendas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                if (id == null)
                {
                    return NotFound();
                }

                Prospeccao prospeccao = await _context.Prospeccao
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (prospeccao == null)
                {
                    return NotFound();
                }

                return View(prospeccao);
            }
            else
            {
                return View("Forbidden");
            }
        }

        public async Task<IActionResult> RemoverFollowUp(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (id is null)
                {
                    return NotFound();
                }

                FollowUp followup = await _context.FollowUp
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (followup == null)
                {
                    return NotFound();
                }

                return await RemoverFollowupAutenticado(followup);
            }
            else
            {
                return View("Forbidden");
            }
        }

        private async Task<IActionResult> RemoverFollowupAutenticado(FollowUp followup)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //Verifica se o usuário está apto para remover o followup
                ViewbagizarUsuario(_context);
                var prospeccoes = await _cache.GetCachedAsync("AllProspeccoes", () => _context.Prospeccao.ToListAsync());
                Prospeccao prospeccao = prospeccoes.Find(p => p.Id == followup.OrigemID);

                if (VerificarCondicoesRemocao(prospeccao, UsuarioAtivo, followup.Origem.Usuario) || UsuarioAtivo.Nivel == Nivel.Dev)
                {
                    _context.FollowUp.Remove(followup);
                    await _context.SaveChangesAsync();
                    await CacheHelper.CleanupProspeccoesCache(_cache);
                    return RedirectToAction("Index", "FunilDeVendas", new { casa = UsuarioAtivo.Casa });
                }
                else
                {
                    // TODO: Colocar isso no frontend em vez de jogar uma exceção na cara do usuário
                    throw new InvalidOperationException("Provável erro na função verificarCondicoesRemocao() no Controller");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        /// <summary>
        /// Valida as condições de remoção de uma prospecção
        /// </summary>
        /// <param name="prospeccao">Prospecção a se validada</param>
        /// <param name="dono">Usuario líder da prospecção</param>
        /// <param name="ativo">Usuário ativo (HttpContext)</param>
        /// <returns></returns>
        internal static bool VerificarCondicoesRemocao(Prospeccao prospeccao, Usuario ativoNaSessao, Usuario donoProsp)
        {
            return prospeccao.Status.Count() > 1 && ativoNaSessao == donoProsp;
        }

        // POST: FunilDeVendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            ViewbagizarUsuario(_context);

            var prospeccoes = await _cache.GetCachedAsync("AllProspeccoes", () => _context.Prospeccao.Include(f => f.Status).ToListAsync());

            var prospeccao = prospeccoes.Where(prosp => prosp.Id == id).First(); // o First converte de IQuerable para objeto Prospeccao

            if (prospeccao.Ancora)
            {
                FunilHelpers.RepassarStatusAoCancelarAncora(_context, prospeccao);
            }

            _context.Remove(prospeccao);
            await _context.SaveChangesAsync();
            await CacheHelper.CleanupProspeccoesCache(_cache);
            return RedirectToAction(nameof(Index), new { casa = UsuarioAtivo.Casa });
        }

        /// <summary>
        /// Função auxiliar de captura e retorno de erro para View de Erro
        /// </summary>
        /// <param name="e">Exceção</param>
        /// <returns></returns>
        private IActionResult CapturarErro(Exception e)
        {
            ErrorViewModel erro = new ErrorViewModel
            {
                Mensagem = e.Message
            };
            return View("Error", erro);
        }

        /// <summary>
        /// Retorna um modal de acordo com os parâmetros
        /// </summary>
        /// <param name="idProsp">ID da prospecção</param>
        /// <param name="tipo">Tipo de Modal a ser retornado</param>
        /// <returns></returns>
        public async Task<IActionResult> RetornarModal(string idProsp, string tipo)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await CriarSelectListsDaView();
                if (tipo != null || tipo != "")
                {
                    return ViewComponent($"Modal{tipo}Prosp", new { id = idProsp });
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

        /// <summary>
        /// Retorna o modal de editar followup
        /// </summary>
        /// <param name="idProsp">ID da Prospecção a ter um followup editado</param>
        /// <param name="idFollowup">ID do Followup a ser editado</param>
        /// <param name="tipo">Tipo de Modal</param>
        /// <returns></returns>
        public async Task<IActionResult> RetornarModalEditFollowup(string idProsp, string idFollowup, string tipo)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await CriarSelectListsDaView();
                if (tipo != null || tipo != "")
                {
                    return ViewComponent($"Modal{tipo}Prosp", new { id = idProsp, id2 = idFollowup });
                }
                else
                {
                    return ViewComponent("Error");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        /// <summary>
        /// Retorna os dados de todas as prospeções cadastradas no sistema em formato JSON.
        /// OBS: Método permite acesso não autenticado vide tag: [AllowAnonymous]
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> PuxarDadosProspeccoes()
        {
            List<Prospeccao> lista_prosp = await _context.Prospeccao.ToListAsync();

            List<Dictionary<string, object>> listaFull = new List<Dictionary<string, object>>();

            foreach (var p in lista_prosp)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    ["idProsp"] = p.Id,
                    ["Líder"] = p.Usuario.UserName,
                    ["Membros"] = p.MembrosEquipe,
                    ["Status"] = p.Status.OrderBy(k => k.Data).LastOrDefault().Status.GetDisplayName(),
                    ["Data"] = p.Status.OrderBy(k => k.Data).LastOrDefault().Data.ToString("MM/yyyy"),
                    ["Empresa"] = p.Empresa.Nome,
                    ["CNPJ"] = p.Empresa.CNPJ,
                    ["Segmento"] = p.Empresa.Segmento.GetDisplayName(),
                    ["Estado"] = p.Empresa.Estado.GetDisplayName(),
                    ["Casa"] = p.Casa.GetDisplayName(),
                    ["Origem"] = p.Origem.GetDisplayName(),
                    ["TipoContratacao"] = p.TipoContratacao.GetDisplayName(),
                    ["LinhaPesquisa"] = p.LinhaPequisa.GetDisplayName(),
                    ["ValorEstimado"] = p.ValorEstimado,
                    ["ValorProposta"] = p.ValorProposta,
                    ["ValorFinal"] = p.ValorProposta,
                };

                if (string.IsNullOrEmpty(p.NomeProspeccao))
                {
                    dict["Titulo"] = "Sem título";
                }
                else
                {
                    dict["Titulo"] = p.NomeProspeccao;
                }

                if (p.ValorProposta == 0)
                {
                    dict["ValorFinal"] = p.ValorEstimado;
                }

                listaFull.Add(dict);
            }

            return Json(listaFull);
        }

        public async Task<IActionResult> PuxarDadosProspeccoes2() //para puxar em selectList
        {
            List<Prospeccao> lista_prosp = await _context.Prospeccao.ToListAsync();

            List<Dictionary<string, object>> listaFull = new List<Dictionary<string, object>>();

            foreach (var p in lista_prosp)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                if (p.NomeProspeccao != null)
                {
                    dict["idProsp"] = p.Id;
                    dict["Titulo"] = p.NomeProspeccao + " [" + p.Empresa.Nome + "]";
                }
                else
                {
                    continue;
                }

                listaFull.Add(dict);
            }

            return Json(listaFull);
        }

        public string PuxarDadosUsuarios()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Helpers.Helpers.PuxarDadosUsuarios(_context);
            }
            else
            {
                return "403 Forbidden";
            }
        }

        /// <summary>
        /// Wrapper para função helper de retorno de tags de prospecções
        /// </summary>
        /// <returns></returns>
        public string PuxarTagsProspecoes()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Helpers.Helpers.PuxarTagsProspecoes(_context);
            }
            else
            {
                return "403 Forbidden";
            }
        }
    }
}