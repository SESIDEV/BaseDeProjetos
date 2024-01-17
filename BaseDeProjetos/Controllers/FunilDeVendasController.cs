using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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

        public int GerarQuantidadeProsp(List<Prospeccao> prospeccoes, Func<Prospeccao, bool> filtro)
        {
            return prospeccoes.Count(filtro);
        }

        public int GerarQuantidadeTipoContratacao(List<Prospeccao> prospeccoes, StatusProspeccao status, TipoContratacao tipo)
        {
            return GerarQuantidadeProsp(prospeccoes, p => p.Status.Any(p => p.Status == status) && p.TipoContratacao == tipo );
        }

        [Route("FunilDeVendas/GerarGraficoBarraTipoContratacao/{casa}")]
        public async Task<IActionResult> GerarGraficoBarraTipoContratacao(string casa)
        {

            Instituto enumCasa;

            if (!Enum.TryParse<Instituto>(casa, out enumCasa))
            {
                throw new ArgumentException("A casa selecionada é inválida");
            }

            Dictionary<string, int> statusprospeccao = new Dictionary<string, int>();
            List<Prospeccao> prospeccoes = _context.Prospeccao.Where(p => p.Casa == enumCasa).ToList();

            foreach (int tipo in Enum.GetValues(typeof(TipoContratacao)))
            {
                int prospContatoInicial = GerarQuantidadeTipoContratacao(prospeccoes, StatusProspeccao.ContatoInicial, (TipoContratacao)tipo);

                int prospEmDiscussao = GerarQuantidadeTipoContratacao(prospeccoes, StatusProspeccao.Discussao_EsbocoProjeto, (TipoContratacao)tipo);

                int prospComProposta = GerarQuantidadeTipoContratacao(prospeccoes, StatusProspeccao.ComProposta, (TipoContratacao)tipo);

                statusprospeccao[$"ContatoInicial_{tipo}"] = prospContatoInicial;

                statusprospeccao[$"EmDiscussao_{tipo}"] = prospEmDiscussao;

                statusprospeccao[$"ComProposta_{tipo}"] = prospComProposta;
            }
            DadosBarra dadosBarra_contatoInicial = new DadosBarra
            {
                ContratacaoDireta = statusprospeccao["ContatoInicial_0"],
                EditaisInovacao = statusprospeccao["ContatoInicial_1"],
                AgenciaFomento = statusprospeccao["ContatoInicial_2"],
                Embrapii = statusprospeccao["ContatoInicial_3"],
                Definir = statusprospeccao["ContatoInicial_4"],
                ParceiroEdital = statusprospeccao["ContatoInicial_5"],
                ANP_ANEEL = statusprospeccao["ContatoInicial_6"]
            };
            DadosBarra dadosBarra_EmDiscussao = new DadosBarra
            {
                ContratacaoDireta = statusprospeccao["EmDiscussao_0"],
                EditaisInovacao = statusprospeccao["EmDiscussao_1"],
                AgenciaFomento = statusprospeccao["EmDiscussao_2"],
                Embrapii = statusprospeccao["EmDiscussao_3"],
                Definir = statusprospeccao["EmDiscussao_4"],
                ParceiroEdital = statusprospeccao["EmDiscussao_5"],
                ANP_ANEEL = statusprospeccao["EmDiscussao_6"]
            };
            DadosBarra dadosBarra_ComProposta = new DadosBarra
            {
                ContratacaoDireta = statusprospeccao["ComProposta_0"],
                EditaisInovacao = statusprospeccao["ComProposta_1"],
                AgenciaFomento = statusprospeccao["ComProposta_2"],
                Embrapii = statusprospeccao["ComProposta_3"],
                Definir = statusprospeccao["ComProposta_4"],
                ParceiroEdital = statusprospeccao["ComProposta_5"],
                ANP_ANEEL = statusprospeccao["ComProposta_6"]
            };
            GraficoBarraTipoContratacao graficoBarraTipoContratacao = new GraficoBarraTipoContratacao 
            { 
                ContatoInicial = dadosBarra_contatoInicial,
                EmDiscussao = dadosBarra_EmDiscussao,
                ComProposta = dadosBarra_ComProposta 
            };
            return Ok(JsonConvert.SerializeObject(graficoBarraTipoContratacao));
        }


        [Route("FunilDeVendas/GerarIndicadoresProsp/{casa}")]
        public async Task<IActionResult> GerarIndicadoresProsp(string casa)
        {
            if (!Enum.TryParse(casa, out Instituto enumCasa))
            {
                throw new ArgumentException("A casa selecionada é inválida");
            }

            // TODO: Pensar em otimizações
            List<Prospeccao> prospeccoesDaCasa = await ObterProspeccoesTotais(enumCasa);

            //todas prospepccoes que possui status em proposta e status inicial, duracao de dias 
            List<TimeSpan> intervaloDatas = new List<TimeSpan>();

            var prospeccoesComPropostaEContatoInicial = prospeccoesDaCasa.Select(p => new { p.Status }).Where(p => p.Status.Any(p => p.Status == StatusProspeccao.ComProposta) && p.Status.Any(p => p.Status == StatusProspeccao.ContatoInicial)).ToList();
            int empresasProspectadas = prospeccoesDaCasa.Select(p => new { p.Empresa }).Distinct().Count();

            for (int i = 0; i < prospeccoesComPropostaEContatoInicial.Count; i++)
            {
                var dataContatoInicial = prospeccoesComPropostaEContatoInicial[i].Status.Where(p => p.Status == StatusProspeccao.ContatoInicial).First().Data;
                var dataComProposta = prospeccoesComPropostaEContatoInicial[i].Status.Where(p => p.Status == StatusProspeccao.ComProposta).First().Data;
                intervaloDatas.Add(dataComProposta - dataContatoInicial);
            }

            var mediaintervalos = new TimeSpan(Convert.ToInt64(intervaloDatas.Average(t => t.Ticks)));
            double tempoMedioContato = mediaintervalos.TotalDays;

            int prospContatoInicial = prospeccoesDaCasa.Select(p => new { p.Status }).Where(p => p.Status.Any(p => p.Status == StatusProspeccao.ContatoInicial)).Count();

            int prospeccoesInfrutiferas = prospeccoesDaCasa
                .Select(p => new { p.Status })
                .Where(p => p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.NaoConvertida
                || p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.Suspensa).Count();

            double percentInfrutiferas = (double)prospeccoesInfrutiferas / prospeccoesDaCasa.Count() * 100;

            IndicadoresProspeccao indicadoresProspeccao = new IndicadoresProspeccao
            {
                EmpresasProspectadas = empresasProspectadas,
                TempoMedioContato = tempoMedioContato,
                PercentualInfrutiferas = percentInfrutiferas,
                ProspContatoInicial = prospContatoInicial
            };

            return Ok(JsonConvert.SerializeObject(indicadoresProspeccao));
        }

        [Route("FunilDeVendas/GerarStatusGeralProspPizza/{casa}")]
        public async Task<IActionResult> GerarStatusGeralProspPizza(string casa)
        {
            Instituto enumCasa;

            if (!Enum.TryParse<Instituto>(casa, out enumCasa))
            {
                throw new ArgumentException("A casa selecionada é inválida");
            }

            int prospSuspensas = _context.Prospeccao.Select(p => new { p.Status, p.Casa }).Where(p => p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.Suspensa && p.Casa == enumCasa).Count();
            double percentCanceladas = (double)prospSuspensas / ObterProspeccoesTotais(enumCasa) * 100;

            int prospConvertidas = _context.Prospeccao.Select(p => new { p.Status, p.Casa }).Where(p => p.Status.Any(p => p.Status == StatusProspeccao.Convertida) && p.Casa == enumCasa).Count();
            double percentConvertidas = (double)prospConvertidas / ObterProspeccoesTotais(enumCasa) * 100;

            int prospNaoConvertidas = _context.Prospeccao.Select(p => new { p.Status, p.Casa }).Where(p => p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.NaoConvertida && p.Casa == enumCasa).Count();
            double percentNaoConvertidas = (double)prospNaoConvertidas / ObterProspeccoesTotais(enumCasa) * 100;

            int prospEmAndamento = ObterProspeccoesTotais(enumCasa) - prospConvertidas - prospNaoConvertidas - prospSuspensas;
            double percentEmAndamento = (double)prospEmAndamento / ObterProspeccoesTotais(enumCasa) * 100;

            StatusGeralProspeccaoPizza statusGeralProspeccaoPizza = new StatusGeralProspeccaoPizza { PercentualCanceladas = percentCanceladas, PercentualConvertidas = percentConvertidas, PercentualEmAndamento = percentEmAndamento, PercentualNaoConvertidas = percentNaoConvertidas };
            return Ok(JsonConvert.SerializeObject(statusGeralProspeccaoPizza));
        }

        [Route("FunilDeVendas/GerarStatusProspPropostaPizza/{casa}")]
        public async Task<IActionResult> GerarStatusProspPropostaPizza(string casa)
        {
            Instituto enumCasa;

            if (!Enum.TryParse<Instituto>(casa, out enumCasa))
            {
                throw new ArgumentException("A casa selecionada é inválida");
            }

            List<Prospeccao> prospeccoes = _context.Prospeccao.Where(p => p.Casa == enumCasa).ToList();
            int prospConvertidas = GerarQuantidadeProsp(prospeccoes, p => p.Status.Any(p => p.Status == StatusProspeccao.Convertida));
            int prospEmAndamento = GerarQuantidadeProsp(prospeccoes, p => p.Status.Any(p => p.Status != StatusProspeccao.Convertida) && p.Status.Any(p => p.Status == StatusProspeccao.NaoConvertida) && p.Status.Any(p => p.Status == StatusProspeccao.Suspensa));
            int prospNaoConvertidas = GerarQuantidadeProsp(prospeccoes, p => p.Status.Any(p => p.Status == StatusProspeccao.NaoConvertida));

            StatusProspeccoesPropostaPizza statusProspeccoesPropostaPizza = new StatusProspeccoesPropostaPizza { QuantidadeEmAndamento = prospEmAndamento, QuantidadeConvertidas = prospConvertidas, QuantidadeNaoConvertidas = prospNaoConvertidas };
            return Ok(JsonConvert.SerializeObject(statusProspeccoesPropostaPizza));
        }

        /// <summary>
        /// Obtém todas as prospecções (exceto planejadas)
        /// </summary>
        /// <returns></returns>
        private async Task<List<Prospeccao>> ObterProspeccoesTotais(Instituto casa)
        {
            return await _cache.GetCachedAsync($"Prospeccoes:{casa}", () => _context.Prospeccao
                .Where(p => p.Status.OrderBy(f => f.Data).Last().Status != StatusProspeccao.Planejada && p.Casa == casa)
                .ToListAsync());
        }

        /// <summary>
        /// Obtém a quantidade de todas as prospecções (exceto planejadas)
        /// </summary>
        /// <returns></returns>
        private int ObterQuantidadeProspeccoesTotais()
        {
            return _context.Prospeccao.Select(p => new { p.Status })
                .Where(p => p.Status.OrderBy(f => f.Data)
                .Last().Status != StatusProspeccao.Planejada)
                .Count();
        }

        /// <summary>
        /// Obtém a quantidade de todas as prospecções dado um Instituto/Casa (exceto planejadas)
        /// </summary>
        /// <param name="casa">Instituo a ser usado como filtro</param>
        /// <returns></returns>
        private int ObterQuantidadeProspeccoesTotais(Instituto casa)
        {
            return _context.Prospeccao.Select(p => new { p.Status, p.Casa })
                .Where(p => p.Status.OrderBy(f => f.Data)
                .Last().Status != StatusProspeccao.Planejada && p.Casa == casa)
                .Count();
        }


        [Route("FunilDeVendas/GerarStatusProspComProposta/{casa}")]
        public async Task<IActionResult> GerarStatusProspComProposta(string casa)
        {
            if (!Enum.TryParse(casa, out Instituto enumCasa))
            {
                throw new ArgumentException("A casa selecionada é inválida");
            }

            var prospeccoesDaCasa = await ObterProspeccoesTotais(enumCasa);

            int prospConvertidas = prospeccoesDaCasa.Select(p => new { p.Status }).Where(p => p.Status.Any(p => p.Status == StatusProspeccao.Convertida)).Count();
            double taxaConversaoProsp = (double)prospConvertidas / prospeccoesDaCasa.Select(p => new { p.Status }).Where(p => p.Status.Any(p => p.Status == StatusProspeccao.ComProposta)).Count() * 100;

            decimal ticketMedioProsp = prospeccoesDaCasa.Select(p => new { p.ValorProposta, p.Status }).Where(p => p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.ComProposta).Average(p => p.ValorProposta);
            int propostasComerciais = prospeccoesDaCasa.Select(p => new { p.Status }).Where(p => p.Status.Any(p => p.Status == StatusProspeccao.ComProposta)).Count();

            int projetosContratados = prospeccoesDaCasa.Select(p => new { p.Status }).Where(p => p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.Convertida).Count();

            StatusProspPropostaIndicadores statusProspPropostaIndicadores = new StatusProspPropostaIndicadores
            {
                TaxaConversao = taxaConversaoProsp,
                TicketMedioProsp = ticketMedioProsp,
                PropostasEnviadas = propostasComerciais,
                ProjetosContratados = projetosContratados
            };

            return Ok(JsonConvert.SerializeObject(statusProspPropostaIndicadores));
        }


        // GET: FunilDeVendas
        [Route("FunilDeVendas/Index/{casa?}/{aba?}/{ano?}")]
        public async Task<IActionResult> Index(string casa, string aba, string sortOrder = "", string searchString = "", string ano = "", int numeroPagina = 1, int tamanhoPagina = 20)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                var prospeccoes = await _cache.GetCachedAsync("Prospeccoes:Funil", () => _context.Prospeccao.Include(p => p.Status).Include(p => p.Empresa).Include(p => p.Usuario).ToListAsync());

                ViewBag.searchString = searchString;
                ViewBag.TamanhoPagina = tamanhoPagina;

                if (string.IsNullOrEmpty(casa))
                {
                    casa = UsuarioAtivo.Casa.ToString();
                }

                prospeccoes = await ObterProspeccoesFunilFiltradas(casa, aba, sortOrder, searchString, ano, UsuarioAtivo);

                await InserirDadosEmpresasUsuariosViewData();

                int qtdProspeccoes = prospeccoes.Count();
                int qtdPaginasTodo = (int)Math.Ceiling((double)qtdProspeccoes / tamanhoPagina);

                var pager = new Pager(qtdProspeccoes, numeroPagina, tamanhoPagina, 50); // 50 paginas max

                if (numeroPagina > qtdPaginasTodo)
                {
                    var paginaVazia = new ProspeccoesViewModel
                    {
                        Prospeccoes = new List<Prospeccao>(),
                        Pager = pager
                    };
                    return View(paginaVazia);
                }

                List<Prospeccao> prospeccoesPagina = ObterProspeccoesPorPagina(prospeccoes, numeroPagina, tamanhoPagina);

                var model = new ProspeccoesViewModel
                {
                    Prospeccoes = prospeccoesPagina,
                    Pager = pager,
                };

                model.ProspeccoesAtivas = prospeccoes.Where(
                        p => p.Status.OrderBy(k => k.Data).All(
                            pa => pa.Status == StatusProspeccao.ContatoInicial || pa.Status == StatusProspeccao.Discussao_EsbocoProjeto || pa.Status == StatusProspeccao.ComProposta)).ToList();
                model.ProspeccoesComProposta = prospeccoes.Select(p => new { p.Status }).Where(p => p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.ComProposta).ToList().Count();
                model.ProspeccoesConcluidas = prospeccoes.Select(p => new { p.Status }).Where(p => p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.Convertida).ToList().Count();
                model.ProspeccoesPlanejadas = prospeccoes.Select(p => new { p.Status }).Where(p => p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.Planejada).ToList().Count();
                model.ProspeccoesNaoPlanejadas= prospeccoes.Where(p => p.Status.OrderBy(f => f.Data).Last().Status != StatusProspeccao.Planejada).ToList();

                if (!string.IsNullOrEmpty(aba))
                {
                    var prospeccoesParaFiltragemAgregadas = await _cache.GetCachedAsync("Prospeccoes:Funil", () => _context.Prospeccao.Include(p => p.Status).Include(p => p.Empresa).Include(p => p.Usuario).ToListAsync());
                    model.ProspeccoesAgregadas = prospeccoesParaFiltragemAgregadas.Where(p => p.Status.OrderBy(k => k.Data).Last().Status == StatusProspeccao.Agregada).ToList();
                    return View(model);
                }
                else
                {
                    model.ProspeccoesGrafico = prospeccoes;

                    model.ProspeccoesAvancadas = prospeccoes.Where(
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

        private async Task InserirDadosEmpresasUsuariosViewData()
        {
            var empresas = await _cache.GetCachedAsync("Empresas:Funil", () => _context.Empresa.Select(e => new EmpresasFunilDTO { Id = e.Id, Nome = e.Nome }).ToListAsync());
            var usuarios = await _cache.GetCachedAsync("Usuarios:Funil", () => _context.Users.Select(u => new UsuariosFunilDTO { Id = u.Id, UserName = u.UserName, Email = u.Email }).ToListAsync());

            ViewData["Usuarios"] = usuarios;
            ViewData["Empresas"] = new SelectList(empresas, "Id", "Nome");
            ViewData["Equipe"] = new SelectList(usuarios, "Id", "UserName");
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
            List<Prospeccao> prospeccoes = await FunilHelpers.DefinirCasaParaVisualizar(casa, usuario, _context, HttpContext, _cache, ViewData);

            if (!string.IsNullOrEmpty(ano))
            {
                FunilHelpers.PeriodizarProspecções(ano, prospeccoes);
            }

            prospeccoes = FunilHelpers.OrdenarProspecções(sortOrder, prospeccoes); // SORT ORDEM ALFABETICA
            prospeccoes = FunilHelpers.FiltrarProspecções(searchString, prospeccoes); // APENAS NA BUSCA

            FunilHelpers.SetarFiltrosNaView(HttpContext, ViewData, sortOrder, searchString);

            if (!string.IsNullOrEmpty(aba))
            {
                ParametrosFiltroFunil parametros = new ParametrosFiltroFunil(casa, usuario, _cache, aba, HttpContext);

                if (!string.IsNullOrEmpty(searchString))
                {
                    prospeccoes = FunilHelpers.RetornarProspeccoesPorStatus(prospeccoes, parametros, true);
                }
                else
                {
                    prospeccoes = FunilHelpers.RetornarProspeccoesPorStatus(prospeccoes, parametros, false);
                }
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
                ViewbagizarUsuario(_context, _cache);

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
            ViewbagizarUsuario(_context, _cache);

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
        public async Task<IActionResult> Create([Bind("Id, TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Status, MembrosEquipe, EmpresaId, Contato, Casa, CaminhoPasta, Tags, Origem, Ancora, Agregadas")] Prospeccao prospeccao)
        {
            ViewbagizarUsuario(_context, _cache);

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

                if (prospeccao.EmpresaId != -1 && !string.IsNullOrEmpty(prospeccao.Contato.Nome))
                {
                    var empresa = await _cache.GetCachedAsync($"Empresa:{prospeccao.EmpresaId}", () => _context.Empresa.FindAsync(prospeccao.EmpresaId).AsTask());
                    if (empresa != null)
                    {
                        prospeccao.Contato.empresa = empresa;
                    }
                }

                await VincularUsuario(prospeccao, HttpContext, _context);

                prospeccao.Status[0].Origem = prospeccao;

                // Por necessidade da implementação do cache tive de omitir essa função, que no momento não está sendo utilizada pois o serviço de email não está habilitado.
                // Ao ligar o serviço de email no futuro essa função estará quebrada (atrelamento de empresas)
                // TODO: Consertar a funcionalidade de Notificar Prospecções pelo Email Helper
                //bool enviou = MailHelper.NotificarProspecção(prospeccao.Status[0], _mailer);

                await _context.AddAsync(prospeccao);
                await _context.SaveChangesAsync();
                await CacheHelper.CleanupProspeccoesCache(_cache);
                return RedirectToAction(nameof(Index), new { casa = UsuarioAtivo.Casa });
            }
            else
            {
                return View("Error");
            }
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

        /// <summary>
        /// Valida e cadastra uma empresa a uma prospecção
        /// </summary>
        /// <param name="prospeccao">Prospecção a ter uma empresa cadastrada</param>
        /// <returns></returns>
        /// <exception cref="Exception">Erro a ser lançado caso não seja possível cadastrar a empresa/seja uma empresa inválida</exception>
        public async Task<Prospeccao> ValidarEmpresa(Prospeccao prospeccao)
        {
            var empresas = await _cache.GetCachedAsync("AllEmpresas", () => _context.Empresa.ToListAsync());
            var empresa = empresas.FirstOrDefault(e => e.Id == prospeccao.EmpresaId);

            if (empresa == null)
            {
                throw new ArgumentNullException("Ocorreu um erro no registro da empresa. \n A empresa selecionada não foi encontrada. \n Contacte um administrador do sistema");
            }

            if (string.IsNullOrEmpty(empresa.Nome) || string.IsNullOrEmpty(empresa.CNPJ) || empresa.Id == -1)
            {
                throw new ArgumentException("Ocorreu um erro no registro da empresa. \n As informações da empresa estão inválidas \n Contacte um administrador do sistema");
            }

            return prospeccao;
        }

        /// <summary>
        /// Cria uma selectlist para a View(?)
        /// </summary>
        private async Task CriarSelectListsDaView()
        {
            var empresas = await _cache.GetCachedAsync("Empresas:FunilUnique", () => _context.Empresa.Select(e => new EmpresasReadComUniqueDTO { EmpresaUnique = e.EmpresaUnique, Id = e.Id, Nome = e.Nome }).ToListAsync());
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
            ViewData["Equipe"] = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
        }

        // POST: FunilDeVendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, EmpresaId, Contato, Casa, Usuario, MembrosEquipe, ValorProposta, ValorEstimado, Status, CaminhoPasta, Tags, Origem, Ancora, Agregadas")] Prospeccao prospeccao)
        {
            ViewbagizarUsuario(_context, _cache);

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
            ViewbagizarUsuario(_context, _cache);

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
            ViewbagizarUsuario(_context, _cache);

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
                ViewbagizarUsuario(_context, _cache);

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
                ViewbagizarUsuario(_context, _cache);
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
            ViewbagizarUsuario(_context, _cache);

            var prospeccoes = await _cache.GetCachedAsync("Prospeccoes:WithStatus", () => _context.Prospeccao.Include(f => f.Status).ToListAsync());

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