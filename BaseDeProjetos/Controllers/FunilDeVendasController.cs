using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


[assembly: InternalsVisibleTo("TestesBaseDeProjetos1")]

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class FunilDeVendasController : SGIController
    {
        private readonly DbCache _cache; 
        private readonly ApplicationDbContext _context;  
        private readonly IEmailSender _mailer; 
        public FunilDeVendasController(ApplicationDbContext context, IEmailSender mailer, DbCache cache)
        {
            _context = context;
            _mailer = mailer;
            _cache = cache;
        }

        [HttpPost]
        public async Task<IActionResult> Atualizar([Bind("OrigemID, Data, Status, Anotacoes, MotivoNaoConversao")] FollowUp followup)
        {
            ViewbagizarUsuario(_context, _cache);

            if (ModelState.IsValid)
            {
                Prospeccao prospeccao_origem = await _context.Prospeccao.FirstOrDefaultAsync(p => p.Id == followup.OrigemID);
                if (prospeccao_origem == null)
                {
                    return NotFound();
                }

                if (!FunilHelpers.UsuarioPodeAcessarCasa(UsuarioAtivo, prospeccao_origem.Casa))
                {
                    return View("Forbidden");
                }

                followup.Origem = prospeccao_origem;

                await CriarFollowUp(followup);

                bool enviou = MailHelper.NotificarProspecção(followup, _mailer);
                await _context.SaveChangesAsync();
                await CacheHelper.CleanupProspeccoesCache(_cache);

                return RedirectToAction(nameof(Index), new { 
                    casa = prospeccao_origem.Casa,
                    searchString = HttpContext.Session.GetString("searchString"),
                    numeroPagina = HttpContext.Session.GetInt32("numeroPagina"),
                    tamanhoPagina = HttpContext.Session.GetInt32("tamanhoPagina"),
                    sortOrder = HttpContext.Session.GetString("sortOrder")
                });

            }

            return RedirectToAction(nameof(Index), new { 
                casa = UsuarioAtivo.Casa,
                searchString = HttpContext.Session.GetString("searchString"),
                numeroPagina = HttpContext.Session.GetInt32("numeroPagina"),
                tamanhoPagina = HttpContext.Session.GetInt32("tamanhoPagina"),
                sortOrder = HttpContext.Session.GetString("sortOrder")
            });
        }

        // GET: FunilDeVendas/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewbagizarUsuario(_context, _cache);
            var casa = HttpContext.Session.GetString("_Casa") ?? UsuarioAtivo?.Casa.ToString();
            return RedirectToAction(nameof(Index), new { casa });
        }

        // POST: FunilDeVendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Tipologia, TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Status, MembrosEquipe, LiderNome, EmpresaId, Contato, Casa, CaminhoPasta, LinkArquivo, Tags, Origem, Ancora, Agregadas, TipoDeInteracao, TipoDeProjeto, PrevisaoTempoProjetoMeses, TipoContratacao, ParceiroInterno, Usuario, ProspeccaoPrincipalId")] Prospeccao prospeccao, [Bind(Prefix = "NovaEmpresa")] Empresa novaEmpresa, bool cadastrarNovaEmpresa, string liderProjeto, string tipoAssociacaoProspecao)
        {
            ViewbagizarUsuario(_context, _cache);

            await ValidarAssociacaoProspeccao(prospeccao, tipoAssociacaoProspecao);

            if (ModelState.IsValid)
            {
                if (!FunilHelpers.UsuarioPodeAcessarCasa(UsuarioAtivo, prospeccao.Casa))
                {
                    return View("Forbidden");
                }

                try
                {
                    if (cadastrarNovaEmpresa)
                    {
                        prospeccao.EmpresaId = await CriarEmpresaDaProspeccao(novaEmpresa);
                    }

                    prospeccao = await ValidarEmpresa(prospeccao);
                }
                catch (Exception e)
                {
                    return CapturarErro(e);
                }

                if (prospeccao.EmpresaId != -1 && prospeccao.Contato != null && !string.IsNullOrEmpty(prospeccao.Contato.Nome))
                {
                    var empresa = await _cache.GetCachedAsync($"Empresa:{prospeccao.EmpresaId}", () => _context.Empresa.FindAsync(prospeccao.EmpresaId).AsTask());
                    if (empresa != null)
                    {
                        prospeccao.Contato.EmpresaId = empresa.Id;
                    }
                }

                prospeccao.Contato = await ReutilizarContatoExistenteDaEmpresa(prospeccao.Contato, prospeccao.EmpresaId);

                DefinirLiderNome(prospeccao, liderProjeto);
                await VincularUsuario(prospeccao, HttpContext, _context, liderProjeto);

                prospeccao.Status[0].Origem = prospeccao;

                // Por necessidade da implementação do cache tive de omitir essa função, que no momento não está sendo utilizada pois o serviço de email não está habilitado.
                // Ao ligar o serviço de email no futuro essa função estará quebrada (atrelamento de empresas)
                // TODO: Consertar a funcionalidade de Notificar Prospecções pelo Email Helper
                //bool enviou = MailHelper.NotificarProspecção(prospeccao.Status[0], _mailer);

                await _context.AddAsync(prospeccao);
                await _context.SaveChangesAsync();
                await CacheHelper.CleanupProspeccoesCache(_cache);

                return RedirectToAction(nameof(Index), 
                    new { 
                        casa = prospeccao.Casa, 
                        aba = HttpContext.Session.GetString("aba"), 
                        searchString = HttpContext.Session.GetString("searchString"),
                        numeroPagina = HttpContext.Session.GetInt32("numeroPagina"),
                        tamanhoPagina = HttpContext.Session.GetInt32("tamanhoPagina"),
                        sortOrder = HttpContext.Session.GetString("sortOrder"),
                    });
            }
            else
            {
                return View("Error");
            }
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

        // POST: FunilDeVendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            ViewbagizarUsuario(_context, _cache);

            var prospeccoes = await _cache.GetCachedAsync("Prospeccoes:WithStatus", () => _context.Prospeccao.Include(f => f.Status).ToListAsync());

            var prospeccao = prospeccoes.FirstOrDefault(prosp => prosp.Id == id); // o First converte de IQuerable para objeto Prospeccao

            if (prospeccao == null)
            {
                return NotFound();
            }

            if (prospeccao.Ancora)
            {
                FunilHelpers.RepassarStatusAoCancelarAncora(_context, prospeccao);
            }

            _context.Remove(prospeccao);
            await _context.SaveChangesAsync();
            await CacheHelper.CleanupProspeccoesCache(_cache);
            return RedirectToAction(nameof(Index), 
                new { 
                    casa = prospeccao.Casa,
                    aba = HttpContext.Session.GetString("aba"),
                    searchString = HttpContext.Session.GetString("searchString"),
                    tamanhoPagina = HttpContext.Session.GetInt32("tamanhoPagina"),
                    numeroPagina = HttpContext.Session.GetInt32("numeroPagina"),
                    sortOrder = HttpContext.Session.GetString("sortOrder")
                });
        }

        // POST: FunilDeVendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, Tipologia, TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Status, MembrosEquipe, LiderNome, EmpresaId, Contato, Casa, CaminhoPasta, LinkArquivo, Tags, Origem, Ancora, Agregadas, ParceiroInterno, Usuario, TipoDeInteracao, TipoDeProjeto, PrevisaoTempoProjetoMeses, ValorEstimado, ValorProposta, ValorFinal")] Prospeccao prospeccao, string liderProjeto)
        {
            ViewbagizarUsuario(_context, _cache);

            if (id != prospeccao.Id)
            {
                return NotFound();
            }

            AplicarValoresMonetariosDoFormulario(prospeccao);

            if (ModelState.IsValid)
            {
                try
                {
                    // Ler EmpresaId vindo do campo hidden (datalist) caso o model binder não tenha preenchido
                    var empresaIdStr = Request.Form["EmpresaId"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(empresaIdStr) && int.TryParse(empresaIdStr, out int empresaIdParsed))
                    {
                        prospeccao.EmpresaId = empresaIdParsed;
                        if (prospeccao.Contato != null)
                        {
                            prospeccao.Contato.EmpresaId = empresaIdParsed;
                        }
                    }

                    prospeccao.Contato = await ReutilizarContatoExistenteDaEmpresa(prospeccao.Contato, prospeccao.EmpresaId);

                    prospeccao = await EditarDadosDaProspecao(id, prospeccao, liderProjeto);
                    await AtualizarComposicaoValoresProspeccao(id);

                    await _context.SaveChangesAsync();
                    await CacheHelper.CleanupProspeccoesCache(_cache);
                }
                catch (UnauthorizedAccessException)
                {
                    return View("Forbidden");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunilHelpers.ProspeccaoExists(prospeccao.Id, _context))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "FunilDeVendas", new { 
                    casa = prospeccao.Casa, 
                    aba = HttpContext.Session.GetString("aba"),
                    searchString = HttpContext.Session.GetString("searchString"),
                    numeroPagina = HttpContext.Session.GetInt32("numeroPagina"),
                    tamanhoPagina = HttpContext.Session.GetInt32("tamanhoPagina"),
                    sortOrder = HttpContext.Session.GetString("sortOrder")
                });
            }

            TempData["FunilErroEdicao"] = "Nao foi possivel salvar a prospeccao. Revise os campos informados e tente novamente.";
            return RedirectToAction("Index", "FunilDeVendas", new {
                casa = prospeccao.Casa,
                aba = HttpContext.Session.GetString("aba"),
                searchString = HttpContext.Session.GetString("searchString"),
                numeroPagina = HttpContext.Session.GetInt32("numeroPagina"),
                tamanhoPagina = HttpContext.Session.GetInt32("tamanhoPagina"),
                sortOrder = HttpContext.Session.GetString("sortOrder")
            });
        }

        private void AplicarValoresMonetariosDoFormulario(Prospeccao prospeccao)
        {
            ModelState.Remove(nameof(Prospeccao.ValorEstimado));
            ModelState.Remove(nameof(Prospeccao.ValorProposta));
            ModelState.Remove(nameof(Prospeccao.ValorFinal));

            string valorEstimadoTexto = Request.Form[nameof(Prospeccao.ValorEstimado)].FirstOrDefault();
            if (TentarConverterDecimal(valorEstimadoTexto, out decimal valorEstimado))
            {
                prospeccao.ValorEstimado = valorEstimado;
            }
            else
            {
                prospeccao.ValorEstimado = 0;
            }

            string valorPropostaTexto = Request.Form[nameof(Prospeccao.ValorProposta)].FirstOrDefault();
            if (TentarConverterDecimal(valorPropostaTexto, out decimal valorProposta))
            {
                prospeccao.ValorProposta = valorProposta;
            }
            else
            {
                prospeccao.ValorProposta = 0;
            }

            string valorFinalTexto = Request.Form[nameof(Prospeccao.ValorFinal)].FirstOrDefault();
            if (TentarConverterDecimal(valorFinalTexto, out decimal valorFinal))
            {
                prospeccao.ValorFinal = valorFinal;
            }
            else
            {
                prospeccao.ValorFinal = null;
            }
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
        public async Task<IActionResult> EditarFollowUp(int id, [Bind("Id", "OrigemID", "Status", "Anotacoes", "Data", "Vencimento, ParceiroInterno, ValorProposta, ValorEstimado, MembrosEquipe")] FollowUp followup)
        {
            ViewbagizarUsuario(_context, _cache);

            if (id != followup.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                // Em vez de atualizar diretamente a entidade vinculada pelo model binder (que pode não conter
                // navegações carregadas), recuperar a entidade existente e aplicar apenas os campos editáveis.
                var existente = await _context.FollowUp.Include(f => f.Origem).FirstOrDefaultAsync(f => f.Id == id);
                if (existente == null)
                {
                    return NotFound();
                }

                if (existente.Origem == null || !FunilHelpers.UsuarioPodeAcessarCasa(UsuarioAtivo, existente.Origem.Casa))
                {
                    return View("Forbidden");
                }

                // Atualiza campos permitidos
                existente.Data = followup.Data;
                existente.Status = followup.Status;
                existente.Anotacoes = followup.Anotacoes;
                existente.Vencimento = followup.Vencimento;

                // Campos opcionais se existirem no modelo
                // nenhum outro campo específico além de Data, Status, Anotacoes e Vencimento existe em FollowUp

                await _context.SaveChangesAsync();
                await CacheHelper.CleanupProspeccoesCache(_cache);
            }
            // Redireciona utilizando a casa da prospecção relacionada (se disponível) ou a casa do usuário ativo
            Instituto casaRedirect = UsuarioAtivo?.Casa ?? Instituto.ISIQV; // default caso inesperado
            // tentar obter a casa a partir do followup original caso exista no contexto
            var followupOrig = await _context.FollowUp.Include(f => f.Origem).FirstOrDefaultAsync(f => f.Id == id);
            if (followupOrig?.Origem != null)
            {
                casaRedirect = followupOrig.Origem.Casa;
            }

            return RedirectToAction("Index", "FunilDeVendas",
                new
                {
                    casa = casaRedirect,
                    aba = HttpContext.Session.GetString("aba"),
                    searchString = HttpContext.Session.GetString("searchString"),
                    numeroPagina = HttpContext.Session.GetInt32("numeroPagina"),
                    tamanhoPagina = HttpContext.Session.GetInt32("tamanhoPagina"),
                    sortOrder = HttpContext.Session.GetString("sortOrder")
                });
        }

        [Route("FunilDeVendas/GerarGraficoBarraTipoContratacao/{casa}")]
        public async Task<IActionResult> GerarGraficoBarraTipoContratacao(string casa)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (!FunilHelpers.TentarParseInstituto(casa, out Instituto enumCasa))
                {
                    throw new ArgumentException("A casa selecionada é inválida");
                }

                Dictionary<string, int> statusprospeccao = new Dictionary<string, int>();

                List<Prospeccao> prospeccoes = await _cache.GetCachedAsync($"Prospeccoes:{enumCasa}", () => _context.Prospeccao.Where(p => p.Casa == enumCasa).ToListAsync());

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
            else
            {
                return View("Forbidden");
            }
        }

        [Route("FunilDeVendas/GerarIndicadoresProsp/{casa}")]
        public async Task<IActionResult> GerarIndicadoresProsp(string casa)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (!FunilHelpers.TentarParseInstituto(casa, out Instituto enumCasa))
                {
                    throw new ArgumentException("A casa selecionada é inválida");
                }

                // TODO: Pensar em otimizações
                List<Prospeccao> prospeccoesDaCasa = await ObterProspeccoesTotais(enumCasa);
                var prospeccoesComPropostaEContatoInicial = prospeccoesDaCasa.Select(p => new { p.Status }).Where(p => p.Status.Any(p => p.Status == StatusProspeccao.ComProposta) && p.Status.Any(p => p.Status == StatusProspeccao.ContatoInicial)).ToList();
                int prospContatoInicial = prospeccoesDaCasa.Select(p => new { p.Status }).Where(p => p.Status.Any(p => p.Status == StatusProspeccao.ContatoInicial)).Count();
                int empresasProspectadas = prospeccoesDaCasa.Select(p => new { p.Empresa }).Distinct().Count();

                //todas prospepccoes que possui status em proposta e status inicial, duracao de dias 
                List<TimeSpan> intervaloDatas = new List<TimeSpan>();

                foreach (var prospeccao in prospeccoesComPropostaEContatoInicial)
                {
                    var dataContatoInicial = prospeccao.Status.Where(p => p.Status == StatusProspeccao.ContatoInicial).First().Data;
                    var dataComProposta = prospeccao.Status.Where(p => p.Status == StatusProspeccao.ComProposta).First().Data;
                    intervaloDatas.Add(dataComProposta - dataContatoInicial);
                }

                double tempoMedioContato = intervaloDatas.Any()
                    ? new TimeSpan(Convert.ToInt64(intervaloDatas.Average(t => t.Ticks))).TotalDays
                    : 0;

                int prospeccoesInfrutiferas = prospeccoesDaCasa
                    .Select(p => new { p.Status })
                    .Where(p => p.Status.Any() &&
                        (p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.NaoConvertida
                        || p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.Suspensa)).Count();

                double percentInfrutiferas = prospeccoesDaCasa.Any()
                    ? (double)prospeccoesInfrutiferas / prospeccoesDaCasa.Count() * 100
                    : 0;

                IndicadoresProspeccao indicadoresProspeccao = new IndicadoresProspeccao
                {
                    EmpresasProspectadas = empresasProspectadas,
                    TempoMedioContato = tempoMedioContato,
                    PercentualInfrutiferas = percentInfrutiferas,
                    ProspContatoInicial = prospContatoInicial
                };

                return Ok(JsonConvert.SerializeObject(indicadoresProspeccao));
            }
            else
            {
                return View("Forbidden");
            }
        }

        public int GerarQuantidadeProsp(List<Prospeccao> prospeccoes, Func<Prospeccao, bool> filtro)
        {
            return prospeccoes.Count(filtro);
        }

        public int GerarQuantidadeTipoContratacao(List<Prospeccao> prospeccoes, StatusProspeccao status, TipoContratacao tipo)
        {
            return GerarQuantidadeProsp(prospeccoes, p => p.Status.Any(p => p.Status == status) && p.TipoContratacao == tipo);
        }

        [Route("FunilDeVendas/GerarStatusGeralProspPizza/{casa}")]
        public async Task<IActionResult> GerarStatusGeralProspPizza(string casa)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (!FunilHelpers.TentarParseInstituto(casa, out Instituto enumCasa))
                {
                    throw new ArgumentException("A casa selecionada é inválida");
                }

                var prospeccoesDaCasa = await ObterProspeccoesTotais(enumCasa);
                var prospeccoesDaCasaConvertida = _cache.GetCached($"Prospeccoes:{enumCasa}:Convertidas:Count", () => prospeccoesDaCasa.Select(p => new { p.Status })
                                                                                                                                       .Where(p => p.Status.Any(p => p.Status == StatusProspeccao.Convertida))
                                                                                                                                       .ToList());

                var prospeccoesDaCasaNaoConvertidas = _cache.GetCached($"Prospeccoes:{enumCasa}:NaoConvertidas:Count", () => prospeccoesDaCasa.Select(p => new { p.Status })
                                                                                                                                              .Where(p => p.Status.Any(p => p.Status == StatusProspeccao.NaoConvertida))
                                                                                                                                              .ToList());

                int prospSuspensas = _cache.GetCached($"Prospeccoes:{enumCasa}:Suspensas:Count", () => prospeccoesDaCasa.Select(p => new { p.Status }).Where(p => p.Status.OrderBy(f => f.Data).Last().Status == StatusProspeccao.Suspensa).Count());
                double percentCanceladas = (double)prospSuspensas / prospeccoesDaCasa.Count() * 100;

                int prospConvertidas = prospeccoesDaCasaConvertida.Count();
                double percentConvertidas = (double)prospConvertidas / prospeccoesDaCasa.Count() * 100;

                int prospNaoConvertidas = prospeccoesDaCasaNaoConvertidas.Count();
                double percentNaoConvertidas = (double)prospNaoConvertidas / prospeccoesDaCasa.Count() * 100;

                int prospEmAndamento = prospeccoesDaCasa.Count() - prospConvertidas - prospNaoConvertidas - prospSuspensas;
                double percentEmAndamento = (double)prospEmAndamento / prospeccoesDaCasa.Count() * 100;

                StatusGeralProspeccaoPizza statusGeralProspeccaoPizza = new StatusGeralProspeccaoPizza
                {
                    PercentualCanceladas = percentCanceladas,
                    PercentualConvertidas = percentConvertidas,
                    PercentualEmAndamento = percentEmAndamento,
                    PercentualNaoConvertidas = percentNaoConvertidas
                };

                return Ok(JsonConvert.SerializeObject(statusGeralProspeccaoPizza));
            }
            else
            {
                return View("Forbidden");
            }
        }

        [Route("FunilDeVendas/GerarStatusProspComProposta/{casa}")]
        public async Task<IActionResult> GerarStatusProspComProposta(string casa)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (!FunilHelpers.TentarParseInstituto(casa, out Instituto enumCasa))
                {
                    throw new ArgumentException("A casa selecionada é inválida");
                }

                var prospeccoesDaCasa = await ObterProspeccoesTotais(enumCasa);
                var prospeccoesDaCasaComProposta = _cache.GetCached($"Prospeccoes:{enumCasa}:ComProposta", () => prospeccoesDaCasa.Select(p => new { p.Status })
                                                                                                                                        .Where(p => p.Status.Any(p => p.Status == StatusProspeccao.ComProposta))
                                                                                                                                        .ToList());

                var prospeccoesDaCasaConvertida = _cache.GetCached($"Prospeccoes:{enumCasa}:Convertidas", () => prospeccoesDaCasa.Select(p => new { p.Status })
                                                                   .Where(p => p.Status.Any(f => f.Status == StatusProspeccao.Convertida))
                                                                   .ToList());

                int prospConvertidas = prospeccoesDaCasaConvertida.Count();
                int totalComProposta = prospeccoesDaCasaComProposta.Count();

                double taxaConversaoProsp = totalComProposta == 0
                    ? 0
                    : (double)prospConvertidas / totalComProposta * 100;


                decimal ticketMedioProsp = prospeccoesDaCasa.Any()
                    ? prospeccoesDaCasa.Average(p => p.ValorProposta)
                    : 0;

                int propostasComerciais = prospeccoesDaCasaComProposta.Count();

                int projetosContratados = prospeccoesDaCasaConvertida.Count();

                StatusProspPropostaIndicadores statusProspPropostaIndicadores = new StatusProspPropostaIndicadores
                {
                    TaxaConversao = taxaConversaoProsp,
                    TicketMedioProsp = ticketMedioProsp,
                    PropostasEnviadas = propostasComerciais,
                    ProjetosContratados = projetosContratados
                };

                return Ok(JsonConvert.SerializeObject(statusProspPropostaIndicadores));
            }
            else
            {
                return View("Forbidden");
            }
        }

        [Route("FunilDeVendas/GerarStatusProspPropostaPizza/{casa}")]
        public async Task<IActionResult> GerarStatusProspPropostaPizza(string casa)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (!FunilHelpers.TentarParseInstituto(casa, out Instituto enumCasa))
                {
                    throw new ArgumentException("A casa selecionada é inválida");
                }

                var prospeccoesDaCasa = await ObterProspeccoesTotais(enumCasa);
                var prospeccoesDaCasaConvertida = _cache.GetCached($"Prospeccoes:{enumCasa}:Convertidas", () => prospeccoesDaCasa.Select(p => new { p.Status })
                                                                   .Where(p => p.Status.Any(f => f.Status == StatusProspeccao.Convertida))
                                                                   .ToList());

                var prospeccoesEmAndamento = _cache.GetCached($"Prospeccoes:{enumCasa}:EmAndamento", () => prospeccoesDaCasa.Select(p => new { p.Status })
                                                                                                                          .Where(p => !p.Status.Any(s => s.Status == StatusProspeccao.Convertida)
                                                                                                                                      && !p.Status.Any(s => s.Status == StatusProspeccao.NaoConvertida)
                                                                                                                                      && !p.Status.Any(s => s.Status == StatusProspeccao.Suspensa)));

                var prospeccoesDaCasaNaoConvertidas = _cache.GetCached($"Prospeccoes:{enumCasa}:NaoConvertidas", () => prospeccoesDaCasa.Select(p => new { p.Status })
                                                                   .Where(p => p.Status.Any(f => f.Status == StatusProspeccao.NaoConvertida))
                                                                   .ToList());

                int prospConvertidas = prospeccoesDaCasaConvertida.Count();
                int prospEmAndamento = prospeccoesEmAndamento.Count();
                int prospNaoConvertidas = prospeccoesDaCasaNaoConvertidas.Count();

                StatusProspeccoesPropostaPizza statusProspeccoesPropostaPizza = new StatusProspeccoesPropostaPizza
                {
                    QuantidadeEmAndamento = prospEmAndamento,
                    QuantidadeConvertidas = prospConvertidas,
                    QuantidadeNaoConvertidas = prospNaoConvertidas
                };

                return Ok(JsonConvert.SerializeObject(statusProspeccoesPropostaPizza));
            }
            else
            {
                return View("Forbidden");
            }
        }

        [Route("FunilDeVendas/GerarIndicadoresMensais/{casa}/{ano}")]
        public async Task<IActionResult> GerarIndicadoresMensais(string casa, int ano)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                List<Instituto> casasSelecionadas;
                List<Instituto> casasPermitidas = FunilHelpers.ObterCasasPermitidas(UsuarioAtivo);

                if (string.IsNullOrWhiteSpace(casa))
                {
                    casasSelecionadas = new List<Instituto> { ObterCasaPadraoAbertura() };
                }
                else if (casa.Equals("Todas", StringComparison.OrdinalIgnoreCase))
                {
                    casasSelecionadas = casasPermitidas;
                }
                else
                {
                    casasSelecionadas = casa
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(casaSelecionada =>
                        {
                            if (!FunilHelpers.TentarParseInstituto(casaSelecionada, out Instituto instituto) || instituto == Instituto.Super || instituto == Instituto.ISIII || instituto == Instituto.ISISVP)
                            {
                                throw new ArgumentException("A casa selecionada e invalida");
                            }

                            return instituto;
                        })
                        .Distinct()
                        .ToList();

                    if (casasSelecionadas.Any(casaSelecionada => !casasPermitidas.Contains(casaSelecionada)))
                    {
                        return View("Forbidden");
                    }
                }

                if (!casasSelecionadas.Any())
                {
                    return View("Forbidden");
                }

                var followUps = await _context.FollowUp
                    .Where(f => f.Origem != null
                        && casasSelecionadas.Contains(f.Origem.Casa)
                        && (f.Data.Year == ano || f.Data.Year == ano - 1)
                        && (f.Status == StatusProspeccao.ContatoInicial
                            || f.Status == StatusProspeccao.ComProposta
                            || f.Status == StatusProspeccao.Convertida))
                    .Select(f => new
                    {
                        f.OrigemID,
                        f.Status,
                        f.Data,
                        LiderId = f.Origem.Usuario != null ? f.Origem.Usuario.Id : f.Origem.LiderNome,
                        LiderNome = !string.IsNullOrWhiteSpace(f.Origem.LiderNome) ? f.Origem.LiderNome : f.Origem.Usuario != null ? f.Origem.Usuario.UserName : null,
                        Criador = !string.IsNullOrWhiteSpace(f.Origem.LiderNome) ? f.Origem.LiderNome : f.Origem.Usuario != null ? f.Origem.Usuario.UserName : null,
                        f.Origem.ValorEstimado,
                        f.Origem.ValorProposta
                    })
                    .ToListAsync();

                int mesReferencia = ObterMesReferenciaIndicadores(ano);

                var indicadoresEquipe = followUps
                    .Where(f => f.Data.Year == ano)
                    .GroupBy(f => string.IsNullOrWhiteSpace(f.Criador) ? "Sem criador" : f.Criador)
                    .Select(g => new
                    {
                        Equipe = g.Key,
                        ContatosRealizados = g.Where(f => f.Status == StatusProspeccao.ContatoInicial).Select(f => f.OrigemID).Distinct().Count(),
                        PropostasEnviadas = g.Where(f => f.Status == StatusProspeccao.ComProposta).Select(f => f.OrigemID).Distinct().Count(),
                        PropostasConvertidas = g.Where(f => f.Status == StatusProspeccao.Convertida).Select(f => f.OrigemID).Distinct().Count(),
                        ValorPropostasEnviadas = g.Where(f => f.Status == StatusProspeccao.ComProposta)
                            .GroupBy(f => f.OrigemID)
                            .Select(grupo => grupo.First().ValorEstimado)
                            .Sum(),
                        ValorPropostasConvertidas = g.Where(f => f.Status == StatusProspeccao.Convertida)
                            .GroupBy(f => f.OrigemID)
                            .Select(grupo => grupo.First().ValorProposta)
                            .Sum()
                    })
                    .OrderBy(linha => linha.Equipe)
                    .ToList();

                var totaisEquipe = new
                {
                    ContatosRealizados = indicadoresEquipe.Sum(linha => linha.ContatosRealizados),
                    PropostasEnviadas = indicadoresEquipe.Sum(linha => linha.PropostasEnviadas),
                    PropostasConvertidas = indicadoresEquipe.Sum(linha => linha.PropostasConvertidas),
                    ValorPropostasEnviadas = indicadoresEquipe.Sum(linha => linha.ValorPropostasEnviadas),
                    ValorPropostasConvertidas = indicadoresEquipe.Sum(linha => linha.ValorPropostasConvertidas)
                };

                var taxasEquipe = indicadoresEquipe
                    .Select(linha => new
                    {
                        Equipe = linha.Equipe,
                        Proposicao = CalcularPercentual(linha.PropostasEnviadas, linha.ContatosRealizados),
                        Conversao = CalcularPercentual(linha.PropostasConvertidas, linha.PropostasEnviadas),
                        Sucesso = CalcularPercentual(linha.PropostasConvertidas, linha.ContatosRealizados),
                        ContribuicaoReceitaGerada = CalcularPercentual(linha.ValorPropostasConvertidas, totaisEquipe.ValorPropostasConvertidas),
                        AssertividadePropostas = CalcularPercentual(linha.ValorPropostasConvertidas, linha.ValorPropostasEnviadas)
                    })
                    .ToList();

                var totaisTaxas = new
                {
                    Proposicao = CalcularPercentual(totaisEquipe.PropostasEnviadas, totaisEquipe.ContatosRealizados),
                    Conversao = CalcularPercentual(totaisEquipe.PropostasConvertidas, totaisEquipe.PropostasEnviadas),
                    Sucesso = CalcularPercentual(totaisEquipe.PropostasConvertidas, totaisEquipe.ContatosRealizados),
                    ContribuicaoReceitaGerada = CalcularPercentual(totaisEquipe.ValorPropostasConvertidas, totaisEquipe.ValorPropostasConvertidas),
                    AssertividadePropostas = CalcularPercentual(totaisEquipe.ValorPropostasConvertidas, totaisEquipe.ValorPropostasEnviadas)
                };

                string chaveFiltroCasas = ObterChaveFiltroCasasIndicadores(casasSelecionadas, casasPermitidas);
                string prefixoArrasteContatos = ObterPrefixoArrasteContatosPesquisador(chaveFiltroCasas);
                var registrosArrasteContatos = await _context.IndicadoresPlanejamentoMensal
                    .AsNoTracking()
                    .Where(indicador => indicador.Casa == Instituto.Super
                        && indicador.Ano == ano
                        && indicador.Indicador.StartsWith(prefixoArrasteContatos)
                        && indicador.Coluna == 0)
                    .ToListAsync();
                Dictionary<string, decimal> arrastesPorPesquisador = registrosArrasteContatos
                    .ToDictionary(
                        registro => registro.Indicador.Substring(prefixoArrasteContatos.Length),
                        registro => registro.Valor);

                List<ContatosPesquisadorIndicadorLinha> contatosPesquisadorBase = followUps
                    .Where(f => f.Data.Year == ano
                        && f.Status == StatusProspeccao.ContatoInicial
                        && !string.IsNullOrWhiteSpace(f.LiderId))
                    .GroupBy(f => NormalizarChavePesquisador(f.LiderNome ?? f.LiderId))
                    .Select(g =>
                    {
                        List<string> chavesAgrupadas = g
                            .Select(f => f.LiderId)
                            .Where(liderId => !string.IsNullOrWhiteSpace(liderId))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList();

                        string pesquisador = g
                            .Select(f => f.LiderNome)
                            .FirstOrDefault(liderNome => !string.IsNullOrWhiteSpace(liderNome));

                        if (string.IsNullOrWhiteSpace(pesquisador))
                        {
                            pesquisador = chavesAgrupadas.FirstOrDefault();
                        }

                        string pesquisadorId = chavesAgrupadas
                            .FirstOrDefault(liderId => arrastesPorPesquisador.ContainsKey(liderId));

                        pesquisadorId ??= chavesAgrupadas
                            .FirstOrDefault(liderId => !string.Equals(liderId, pesquisador, StringComparison.OrdinalIgnoreCase));

                        pesquisadorId ??= chavesAgrupadas.FirstOrDefault() ?? pesquisador;

                        int[] mesesPesquisador = Enumerable.Range(1, 12)
                            .Select(mes => g.Where(f => f.Data.Month == mes).Select(f => f.OrigemID).Distinct().Count())
                            .ToArray();

                        decimal arraste = !string.IsNullOrWhiteSpace(pesquisadorId) && arrastesPorPesquisador.ContainsKey(pesquisadorId)
                            ? arrastesPorPesquisador[pesquisadorId]
                            : 0;

                        return new ContatosPesquisadorIndicadorLinha
                        {
                            PesquisadorId = pesquisadorId,
                            Pesquisador = string.IsNullOrWhiteSpace(pesquisador) ? "Sem lider" : pesquisador,
                            Arraste = arraste,
                            Meses = mesesPesquisador,
                            ChavesAgrupadas = chavesAgrupadas
                        };
                    })
                    .ToList();

                List<string> pesquisadoresComLinha = contatosPesquisadorBase
                    .SelectMany(linha => linha.ChavesAgrupadas.Concat(new[] { linha.PesquisadorId }))
                    .Where(pesquisadorId => !string.IsNullOrWhiteSpace(pesquisadorId))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                List<string> pesquisadoresSomenteArraste = arrastesPorPesquisador.Keys
                    .Where(pesquisadorId => !pesquisadoresComLinha.Contains(pesquisadorId))
                    .ToList();

                if (pesquisadoresSomenteArraste.Any())
                {
                    var usuariosSomenteArraste = await _context.Users
                        .AsNoTracking()
                        .Where(usuario => pesquisadoresSomenteArraste.Contains(usuario.Id))
                        .Select(usuario => new { usuario.Id, usuario.UserName })
                        .ToListAsync();

                    contatosPesquisadorBase.AddRange(usuariosSomenteArraste.Select(usuario => new ContatosPesquisadorIndicadorLinha
                    {
                        PesquisadorId = usuario.Id,
                        Pesquisador = usuario.UserName,
                        Arraste = arrastesPorPesquisador[usuario.Id],
                        Meses = new int[12]
                    }));
                }

                decimal totalGeralContatosPesquisador = contatosPesquisadorBase.Sum(linha => linha.Total);
                var contatosPesquisadorLinhas = contatosPesquisadorBase
                    .OrderBy(linha => linha.Pesquisador)
                    .Select(linha => new
                    {
                        linha.PesquisadorId,
                        linha.Pesquisador,
                        linha.Arraste,
                        linha.Meses,
                        linha.Total,
                        Percentual = CalcularPercentual(linha.Total, totalGeralContatosPesquisador)
                    })
                    .ToList();
                var contatosPesquisadorTotais = new
                {
                    Arraste = contatosPesquisadorBase.Sum(linha => linha.Arraste),
                    Meses = Enumerable.Range(0, 12)
                        .Select(indiceMes => contatosPesquisadorBase.Sum(linha => linha.Meses[indiceMes]))
                        .ToArray(),
                    Total = totalGeralContatosPesquisador,
                    Percentual = totalGeralContatosPesquisador == 0 ? 0 : 100
                };
                List<string> pesquisadoresNaTabela = contatosPesquisadorBase
                    .Select(linha => linha.PesquisadorId)
                    .Where(pesquisadorId => !string.IsNullOrWhiteSpace(pesquisadorId))
                    .Distinct()
                    .ToList();
                var pesquisadoresDisponiveisContatos = await _context.Users
                    .AsNoTracking()
                    .Where(usuario => casasSelecionadas.Contains(usuario.Casa)
                        && usuario.Nivel != Nivel.Externos
                        && !pesquisadoresNaTabela.Contains(usuario.Id))
                    .OrderBy(usuario => usuario.UserName)
                    .Select(usuario => new
                    {
                        PesquisadorId = usuario.Id,
                        Pesquisador = usuario.UserName
                    })
                    .ToListAsync();

                string[] indicadoresPlanejamentoGraficos = new[]
                {
                    "CONTATOS_REGISTRADOS_META",
                    "PROPOSTAS_ENVIADAS_META",
                    "PROJETOS_CONVERTIDOS_META"
                };

                var planejamentoGraficos = await _context.IndicadoresPlanejamentoMensal
                    .Where(indicador => indicador.Ano == ano
                        && casasSelecionadas.Contains(indicador.Casa)
                        && indicadoresPlanejamentoGraficos.Contains(indicador.Indicador)
                        && indicador.Coluna >= 0
                        && indicador.Coluna <= 12)
                    .ToListAsync();

                var dados = new
                {
                    Ano = ano,
                    AnoAnterior = ano - 1,
                    Casa = string.Join(",", casasSelecionadas.Select(instituto => instituto.ToString())),
                    TodasAsCasas = casasSelecionadas.Count == Enum.GetValues(typeof(Instituto)).Cast<Instituto>().Count(instituto => instituto != Instituto.Super && instituto != Instituto.ISIII && instituto != Instituto.ISISVP),
                    MesAtual = mesReferencia,
                    Meses = Enumerable.Range(0, 13).ToArray(),
                    ContatosRealizados = new
                    {
                        Planejado = GerarSeriePlanejadoMensal(planejamentoGraficos, "CONTATOS_REGISTRADOS_META"),
                        Executado = GerarSerieExecutadoMensal(followUps
                            .Where(f => f.Status == StatusProspeccao.ContatoInicial && f.Data.Year == ano)
                            .Select(f => f.Data)),
                        ExecutadoAnoAnterior = GerarSerieExecutadoMensal(followUps
                            .Where(f => f.Status == StatusProspeccao.ContatoInicial && f.Data.Year == ano - 1)
                            .Select(f => f.Data))
                    },
                    PropostasEnviadas = new
                    {
                        Planejado = GerarSeriePlanejadoMensal(planejamentoGraficos, "PROPOSTAS_ENVIADAS_META"),
                        Executado = GerarSerieExecutadoMensal(followUps
                            .Where(f => f.Status == StatusProspeccao.ComProposta && f.Data.Year == ano)
                            .Select(f => f.Data)),
                        ExecutadoAnoAnterior = GerarSerieExecutadoMensal(followUps
                            .Where(f => f.Status == StatusProspeccao.ComProposta && f.Data.Year == ano - 1)
                            .Select(f => f.Data))
                    },
                    PropostasConvertidas = new
                    {
                        Planejado = GerarSeriePlanejadoMensal(planejamentoGraficos, "PROJETOS_CONVERTIDOS_META"),
                        Executado = GerarSerieExecutadoMensal(followUps
                            .Where(f => f.Status == StatusProspeccao.Convertida && f.Data.Year == ano)
                            .Select(f => f.Data)),
                        ExecutadoAnoAnterior = GerarSerieExecutadoMensal(followUps
                            .Where(f => f.Status == StatusProspeccao.Convertida && f.Data.Year == ano - 1)
                            .Select(f => f.Data))
                    },
                    Equipe = new
                    {
                        Linhas = indicadoresEquipe,
                        Totais = totaisEquipe
                    },
                    Taxas = new
                    {
                        Linhas = taxasEquipe,
                        Totais = totaisTaxas
                    },
                    ContatosPesquisador = new
                    {
                        PodeEditarArraste = UsuarioPodeEditarArrasteContatosPesquisador(),
                        Linhas = contatosPesquisadorLinhas,
                        Totais = contatosPesquisadorTotais,
                        PesquisadoresDisponiveis = pesquisadoresDisponiveisContatos
                    }
                };

                return Ok(dados);
            }

            return View("Forbidden");
        }
        // GET: FunilDeVendas
        [Route("FunilDeVendas/Index/{casa?}/{aba?}/{ano?}")]
        public async Task<IActionResult> Index(string casa, string aba, string sortOrder = "", string searchString = "", string ano = "", string temperatura = "", string fomento = "", int numeroPagina = 1, int tamanhoPagina = 21)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                if (string.IsNullOrEmpty(aba))
                {
                    aba = "ativas";
                }

                if (tamanhoPagina <= 0 || tamanhoPagina == 20)
                {
                    tamanhoPagina = 21;
                }

                SetarAbaNaSession(aba);

                ParametrosFunil parametrosFunil = new ParametrosFunil
                {
                    Aba = aba,
                    SearchString = searchString,
                    NumeroPagina = numeroPagina,
                    TamanhoPagina = tamanhoPagina,
                    SortOrder = sortOrder
                };
                SetarParametrosFunilSession(parametrosFunil);

                if (string.IsNullOrEmpty(casa))
                {
                    casa = ObterCasaPadraoAbertura().ToString();
                }

                bool abaIndicadores = aba.Equals("indicadores", StringComparison.OrdinalIgnoreCase);
                bool abaPlanejamentoIndicadores = aba.Equals("planejamento", StringComparison.OrdinalIgnoreCase);
                bool abaReceitasIndicadores = aba.Equals("receitas", StringComparison.OrdinalIgnoreCase);
                int anoSelecionado = !string.IsNullOrEmpty(ano) && int.TryParse(ano, out int anoIndicadores) ? anoIndicadores : DateTime.Now.Year;

                if (abaPlanejamentoIndicadores)
                {
                    if (!UsuarioPodeEditarPlanejamentoIndicadores())
                    {
                        return View("Forbidden");
                    }

                    Instituto casaPlanejamento = ResolverCasaPlanejamentoIndicadores(casa);
                    casa = casaPlanejamento.ToString();
                    ViewBag.PlanejamentoIndicadores = await MontarPlanejamentoIndicadores(casaPlanejamento, anoSelecionado);
                }
                else if (abaReceitasIndicadores)
                {
                    Instituto casaReceitas = ResolverCasaReceitasGestor(casa);
                    if (!UsuarioPodeEditarReceitasGestor(casaReceitas))
                    {
                        return View("Forbidden");
                    }

                    casa = casaReceitas.ToString();
                    ViewBag.ReceitasGestorIndicadores = await MontarReceitasGestorIndicadores(casaReceitas, anoSelecionado);
                }

                ViewBag.searchString = searchString;
                ViewBag.TamanhoPagina = tamanhoPagina;
                ViewBag.Casa = casa;
                ViewBag.Ano = anoSelecionado;
                ViewBag.Temperatura = temperatura;
                ViewBag.Fomento = fomento;
                
                bool abaSegmentos = aba.Equals("segmentos", StringComparison.OrdinalIgnoreCase);
                string abaConsulta = abaSegmentos ? string.Empty : aba;
                List<Prospeccao> prospeccoes = await ObterProspeccoesFunilFiltradas(casa, ano, UsuarioAtivo, abaConsulta, sortOrder, searchString, temperatura, fomento);
                if (abaSegmentos)
                {
                    prospeccoes = prospeccoes
                        .Where(prospeccao => prospeccao.Status != null
                            && prospeccao.Status.Any()
                            && prospeccao.Status.OrderBy(followup => followup.Data).ThenBy(followup => followup.Id).Last().Status != StatusProspeccao.Planejada)
                        .ToList();
                }
                List<Prospeccao> prospeccoesFunilCompleto = prospeccoes;
                if (!abaIndicadores && !abaPlanejamentoIndicadores && !abaReceitasIndicadores && !abaSegmentos)
                {
                    prospeccoesFunilCompleto = await ObterProspeccoesFunilFiltradas(casa, ano, UsuarioAtivo, string.Empty, sortOrder, searchString, temperatura, fomento);
                    prospeccoesFunilCompleto = prospeccoesFunilCompleto
                        .Where(prospeccao => prospeccao.Status != null
                            && prospeccao.Status.Any())
                        .ToList();
                }
                var quantidadesAbas = abaIndicadores || abaPlanejamentoIndicadores || abaReceitasIndicadores
                    ? (Planejadas: 0, Ativas: 0, ComProposta: 0, Contratacao: 0, Encerradas: 0)
                    : await ObterQuantidadesAbasFunil(casa, ano, UsuarioAtivo, sortOrder, searchString, temperatura, fomento);
                ViewBag.ProspeccoesPlanejadasCount = quantidadesAbas.Planejadas;
                ViewBag.ProspeccoesAtivasCount = quantidadesAbas.Ativas;
                ViewBag.ProspeccoesComProposta = quantidadesAbas.ComProposta;
                ViewBag.ProspeccoesContratacao = quantidadesAbas.Contratacao;
                ViewBag.ProspeccoesEncerradas = quantidadesAbas.Encerradas;
                ViewBag.ProspeccoesConcluidas = quantidadesAbas.Contratacao + quantidadesAbas.Encerradas;

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

                await InserirDadosEmpresasUsuariosViewData();
                List<Prospeccao> prospeccoesPagina = abaSegmentos
                    ? prospeccoes
                    : ObterProspeccoesPorPagina(prospeccoes, numeroPagina, tamanhoPagina);


                prospeccoesPagina = await EnriquecerProspeccoesFunilAsync(prospeccoesPagina);

                var model = new ProspeccoesViewModel
                {
                    Prospeccoes = prospeccoesPagina,
                    ProspeccoesGrafico = prospeccoesFunilCompleto,
                    Pager = pager,
                };

                /*
                var prospeccoesParaFiltragemAgregadas = await _cache.GetCachedAsync("Prospeccoes:Funil", () => _context.Prospeccao.Include(p => p.Status).Include(p => p.Empresa).Include(p => p.Usuario).ToListAsync());
                model.ProspeccoesAgregadas = prospeccoesParaFiltragemAgregadas.Where(p => p.Status.OrderBy(k => k.Data).Last().Status == StatusProspeccao.Agregada).ToList();
                */
                return View(model);
            }
            else
            {
                return View("Forbidden");
            }
        }

        private async Task<(int Planejadas, int Ativas, int ComProposta, int Contratacao, int Encerradas)> ObterQuantidadesAbasFunil(string casa, string ano, Usuario usuario, string sortOrder, string searchString, string temperatura, string fomento)
        {
            IQueryable<Prospeccao> query =
                FunilHelpers.DefinirCasaParaVisualizarQuery(
                    casa,
                    usuario,
                    _context,
                    HttpContext,
                    ViewData
                );

            query = query
                .Include(p => p.ComposicaoValores)
                .Where(p => p.Empresa != null);

            if (!string.IsNullOrEmpty(ano) && !ano.Equals("Todos", StringComparison.OrdinalIgnoreCase))
            {
                query = FunilHelpers.PeriodizarProspecçõesQuery(query, ano);
            }

            query = FunilHelpers.FiltrarProspecçõesQuery(query, searchString);

            if (!string.IsNullOrWhiteSpace(fomento) && Enum.TryParse(fomento, out TipoContratacao tipoContratacao))
            {
                query = query.Where(p => p.TipoContratacao == tipoContratacao);
            }

            List<Prospeccao> prospeccoes = await query.ToListAsync();

            if (!string.IsNullOrWhiteSpace(temperatura) && !temperatura.Equals("Todos", StringComparison.OrdinalIgnoreCase))
            {
                prospeccoes = prospeccoes
                    .Where(prospeccao => ProspeccaoTemTemperatura(prospeccao, temperatura))
                    .ToList();
            }

            int planejadas = prospeccoes.Count(prospeccao =>
            {
                FollowUp ultimoStatus = ObterUltimoStatusProspeccao(prospeccao);
                return ultimoStatus != null && ultimoStatus.Status == StatusProspeccao.Planejada;
            });

            int ativas = prospeccoes.Count(prospeccao =>
            {
                FollowUp ultimoStatus = ObterUltimoStatusProspeccao(prospeccao);
                return ultimoStatus != null && ultimoStatus.Status < StatusProspeccao.ComProposta;
            });

            int comProposta = prospeccoes.Count(prospeccao =>
                prospeccao.Status.Any(s => s.Status == StatusProspeccao.ComProposta) &&
                !prospeccao.Status.Any(s => s.Status == StatusProspeccao.Convertida || s.Status == StatusProspeccao.NaoConvertida || s.Status == StatusProspeccao.Suspensa)
            );

            int contratacao = prospeccoes.Count(prospeccao =>
                prospeccao.Status.Any(s => s.Status == StatusProspeccao.Convertida)
            );

            int encerradas = prospeccoes.Count(prospeccao =>
                prospeccao.Status.Any(s =>
                    s.Status == StatusProspeccao.Suspensa ||
                    s.Status == StatusProspeccao.NaoConvertida
                )
            );

            return (planejadas, ativas, comProposta, contratacao, encerradas);
        }

        private static FollowUp ObterUltimoStatusProspeccao(Prospeccao prospeccao)
        {
            return prospeccao.Status?
                .OrderBy(followup => followup.Data)
                .ThenBy(followup => followup.Id)
                .LastOrDefault();
        }

        private static int[] GerarSerieExecutadoMensal(IEnumerable<DateTime> datas)
        {
            int[] contagemMensal = new int[12];

            foreach (DateTime data in datas)
            {
                if (data.Month >= 1 && data.Month <= 12)
                {
                    contagemMensal[data.Month - 1]++;
                }
            }

            int[] serieAcumulada = new int[13];
            int acumulado = 0;

            for (int indiceMes = 0; indiceMes < contagemMensal.Length; indiceMes++)
            {
                acumulado += contagemMensal[indiceMes];
                serieAcumulada[indiceMes + 1] = acumulado;
            }

            return serieAcumulada;
        }

        private static decimal?[] GerarSeriePlanejadoMensal(List<IndicadoresPlanejamentoMensal> registros, string indicador)
        {
            var valoresPorColuna = registros
                .Where(registro => registro.Indicador == indicador)
                .GroupBy(registro => registro.Coluna)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.Sum(registro => registro.Valor));

            decimal?[] serie = new decimal?[13];

            for (int coluna = 0; coluna <= 12; coluna++)
            {
                if (valoresPorColuna.TryGetValue(coluna, out decimal valor))
                {
                    serie[coluna] = Math.Round(valor, 2);
                }
            }

            return serie;
        }

        private static int ObterMesReferenciaIndicadores(int ano)
        {
            if (ano < DateTime.Now.Year) return 12;
            if (ano > DateTime.Now.Year) return 0;
            return DateTime.Now.Month;
        }

        private static decimal CalcularPercentual(decimal parte, decimal total)
        {
            if (total == 0) return 0;

            return Math.Round(parte / total * 100, 2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("FunilDeVendas/SalvarArrasteContatosPesquisador")]
        public async Task<IActionResult> SalvarArrasteContatosPesquisador(string casa, int ano, string pesquisadorId, string valor)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            ViewbagizarUsuario(_context, _cache);

            if (!UsuarioPodeEditarArrasteContatosPesquisador() || string.IsNullOrWhiteSpace(pesquisadorId))
            {
                return Forbid();
            }

            if (!TentarConverterDecimal(valor, out decimal valorArraste))
            {
                return BadRequest("Valor invalido");
            }

            List<Instituto> casasSelecionadas;
            try
            {
                casasSelecionadas = ResolverCasasIndicadoresSelecionadas(casa);
            }
            catch (ArgumentException)
            {
                return BadRequest("Casa invalida");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }

            List<Instituto> casasPermitidas = FunilHelpers.ObterCasasPermitidas(UsuarioAtivo);
            string chaveFiltroCasas = ObterChaveFiltroCasasIndicadores(casasSelecionadas, casasPermitidas);
            string indicador = ObterIndicadorArrasteContatosPesquisador(chaveFiltroCasas, pesquisadorId);

            IndicadoresPlanejamentoMensal registro = await _context.IndicadoresPlanejamentoMensal
                .FirstOrDefaultAsync(item => item.Casa == Instituto.Super
                    && item.Ano == ano
                    && item.Indicador == indicador
                    && item.Coluna == 0);

            if (registro == null)
            {
                registro = new IndicadoresPlanejamentoMensal
                {
                    Casa = Instituto.Super,
                    Ano = ano,
                    Indicador = indicador,
                    Coluna = 0,
                    Valor = valorArraste
                };
                _context.IndicadoresPlanejamentoMensal.Add(registro);
            }
            else
            {
                registro.Valor = valorArraste;
            }

            await _context.SaveChangesAsync();

            return Ok(new { sucesso = true });
        }

        private bool UsuarioPodeEditarArrasteContatosPesquisador()
        {
            return UsuarioAtivo != null
                && (UsuarioAtivo.Nivel == Nivel.Supervisor || UsuarioAtivo.Nivel == Nivel.Dev || UsuarioAtivo.Nivel == Nivel.PMO);
        }

        private Instituto ObterCasaPadraoAbertura()
        {
            List<Instituto> casasPermitidas = FunilHelpers.ObterCasasPermitidas(UsuarioAtivo);
            List<Instituto> casasVisiveis = FiltrarCasasIndicadoresVisiveis(casasPermitidas);

            if (casasVisiveis.Contains(Instituto.ISIQV))
            {
                return Instituto.ISIQV;
            }

            if (UsuarioAtivo != null && casasVisiveis.Contains(UsuarioAtivo.Casa))
            {
                return UsuarioAtivo.Casa;
            }

            return casasVisiveis.FirstOrDefault();
        }

        private List<Instituto> ResolverCasasIndicadoresSelecionadas(string casa)
        {
            List<Instituto> casasPermitidas = FunilHelpers.ObterCasasPermitidas(UsuarioAtivo);
            List<Instituto> casasVisiveis = FiltrarCasasIndicadoresVisiveis(casasPermitidas);

            if (string.IsNullOrWhiteSpace(casa))
            {
                return new List<Instituto> { ObterCasaPadraoAbertura() };
            }

            if (casa.Equals("Todas", StringComparison.OrdinalIgnoreCase))
            {
                return casasVisiveis;
            }

            List<Instituto> casasSelecionadas = casa
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(casaSelecionada =>
                {
                    if (!FunilHelpers.TentarParseInstituto(casaSelecionada, out Instituto instituto)
                        || instituto == Instituto.Super
                        || instituto == Instituto.ISIII
                        || instituto == Instituto.ISISVP)
                    {
                        throw new ArgumentException("A casa selecionada e invalida");
                    }

                    return instituto;
                })
                .Distinct()
                .ToList();

            if (casasSelecionadas.Any(casaSelecionada => !casasVisiveis.Contains(casaSelecionada)))
            {
                throw new UnauthorizedAccessException("Casa nao permitida");
            }

            return casasSelecionadas;
        }

        private static List<Instituto> FiltrarCasasIndicadoresVisiveis(List<Instituto> casas)
        {
            return casas
                .Where(instituto => instituto != Instituto.Super && instituto != Instituto.ISIII && instituto != Instituto.ISISVP)
                .Distinct()
                .OrderBy(instituto => instituto.ToString())
                .ToList();
        }

        private static string ObterChaveFiltroCasasIndicadores(List<Instituto> casasSelecionadas, List<Instituto> casasPermitidas)
        {
            List<Instituto> casasSelecionadasOrdenadas = FiltrarCasasIndicadoresVisiveis(casasSelecionadas)
                .OrderBy(instituto => instituto.ToString())
                .ToList();
            List<Instituto> casasPermitidasOrdenadas = FiltrarCasasIndicadoresVisiveis(casasPermitidas);

            if (casasSelecionadasOrdenadas.Count == casasPermitidasOrdenadas.Count
                && !casasPermitidasOrdenadas.Except(casasSelecionadasOrdenadas).Any())
            {
                return "Todas";
            }

            return string.Join(",", casasSelecionadasOrdenadas.Select(instituto => instituto.ToString()));
        }

        private static string ObterPrefixoArrasteContatosPesquisador(string chaveFiltroCasas)
        {
            return $"ARR_CONT_{ObterHashCurto(chaveFiltroCasas)}_";
        }

        private static string ObterIndicadorArrasteContatosPesquisador(string chaveFiltroCasas, string pesquisadorId)
        {
            return $"{ObterPrefixoArrasteContatosPesquisador(chaveFiltroCasas)}{pesquisadorId}";
        }

        private static string ObterHashCurto(string valor)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(valor ?? ""));
                return BitConverter.ToString(hash).Replace("-", "").Substring(0, 16);
            }
        }

        private static string NormalizarChavePesquisador(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return string.Empty;
            }

            return string.Join(" ", valor.Trim().Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                .ToUpperInvariant();
        }

        private class ContatosPesquisadorIndicadorLinha
        {
            public string PesquisadorId { get; set; }
            public string Pesquisador { get; set; }
            public decimal Arraste { get; set; }
            public int[] Meses { get; set; } = new int[12];
            public List<string> ChavesAgrupadas { get; set; } = new List<string>();
            public decimal Total => Arraste + Meses.Sum();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("FunilDeVendas/SalvarReceitasGestorIndicadores")]
        public async Task<IActionResult> SalvarReceitasGestorIndicadores(string casa, int ano)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Forbidden");
            }

            ViewbagizarUsuario(_context, _cache);

            Instituto casaReceitas = ResolverCasaReceitasGestor(casa);
            if (!UsuarioPodeEditarReceitasGestor(casaReceitas))
            {
                return View("Forbidden");
            }

            var existentes = await _context.IndicadoresReceitaGestor
                .Where(registro => registro.Casa == casaReceitas && registro.AnoBase == ano)
                .ToListAsync();

            int totalLinhas = int.TryParse(Request.Form["linhasCount"].FirstOrDefault(), out int linhasInformadas)
                ? linhasInformadas
                : 0;

            int ordem = 0;
            for (int indice = 0; indice < totalLinhas; indice++)
            {
                int id = int.TryParse(Request.Form[$"linhas[{indice}].Id"].FirstOrDefault(), out int idLinha)
                    ? idLinha
                    : 0;
                bool remover = Request.Form[$"linhas[{indice}].Remover"].FirstOrDefault() == "true";

                IndicadoresReceitaGestor existente = existentes.FirstOrDefault(registro => registro.Id == id);
                if (remover)
                {
                    if (existente != null)
                    {
                        _context.IndicadoresReceitaGestor.Remove(existente);
                    }

                    continue;
                }

                string empresa = ObterTextoFormularioReceitas(indice, "Empresa", 200);
                string iniciativa = ObterTextoFormularioReceitas(indice, "Iniciativa", 160);
                string parceiroInterno = ObterTextoFormularioReceitas(indice, "ParceiroInterno", 120);
                decimal? valorTotal = ObterDecimalFormularioReceitas(indice, "ValorTotal");
                decimal? receitaTotal = ObterDecimalFormularioReceitas(indice, "ReceitaTotal");
                decimal? receitaAnoBase = ObterDecimalFormularioReceitas(indice, "ReceitaAnoBase");
                decimal? projecaoAno1 = ObterDecimalFormularioReceitas(indice, "ProjecaoAno1");
                decimal? projecaoAno2 = ObterDecimalFormularioReceitas(indice, "ProjecaoAno2");
                decimal? projecaoAno3 = ObterDecimalFormularioReceitas(indice, "ProjecaoAno3");
                decimal? projecaoAno4 = ObterDecimalFormularioReceitas(indice, "ProjecaoAno4");
                decimal? projecaoAno5 = ObterDecimalFormularioReceitas(indice, "ProjecaoAno5");

                bool linhaVazia = string.IsNullOrWhiteSpace(empresa)
                    && string.IsNullOrWhiteSpace(iniciativa)
                    && string.IsNullOrWhiteSpace(parceiroInterno)
                    && !valorTotal.HasValue
                    && !receitaTotal.HasValue
                    && !receitaAnoBase.HasValue
                    && !projecaoAno1.HasValue
                    && !projecaoAno2.HasValue
                    && !projecaoAno3.HasValue
                    && !projecaoAno4.HasValue
                    && !projecaoAno5.HasValue;

                if (linhaVazia)
                {
                    if (existente != null)
                    {
                        _context.IndicadoresReceitaGestor.Remove(existente);
                    }

                    continue;
                }

                if (existente == null)
                {
                    existente = new IndicadoresReceitaGestor
                    {
                        Casa = casaReceitas,
                        AnoBase = ano
                    };
                    await _context.IndicadoresReceitaGestor.AddAsync(existente);
                }

                existente.Ordem = ordem++;
                existente.Empresa = empresa;
                existente.Iniciativa = iniciativa;
                existente.ValorTotal = valorTotal;
                existente.ReceitaTotal = receitaTotal;
                existente.ReceitaAnoBase = receitaAnoBase;
                existente.ProjecaoAno1 = projecaoAno1;
                existente.ProjecaoAno2 = projecaoAno2;
                existente.ProjecaoAno3 = projecaoAno3;
                existente.ProjecaoAno4 = projecaoAno4;
                existente.ProjecaoAno5 = projecaoAno5;
                existente.ParceiroInterno = parceiroInterno;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { casa = casaReceitas.ToString(), aba = "receitas", ano });
        }

        private async Task<IndicadoresReceitaGestorViewModel> MontarReceitasGestorIndicadores(Instituto casa, int ano)
        {
            List<IndicadoresReceitaGestorLinhaViewModel> linhas = await _context.IndicadoresReceitaGestor
                .AsNoTracking()
                .Where(registro => registro.Casa == casa && registro.AnoBase == ano)
                .OrderBy(registro => registro.Ordem)
                .ThenBy(registro => registro.Id)
                .Select(registro => new IndicadoresReceitaGestorLinhaViewModel
                {
                    Id = registro.Id,
                    Empresa = registro.Empresa,
                    Iniciativa = registro.Iniciativa,
                    ValorTotal = registro.ValorTotal,
                    ReceitaTotal = registro.ReceitaTotal,
                    ReceitaAnoBase = registro.ReceitaAnoBase,
                    ProjecaoAno1 = registro.ProjecaoAno1,
                    ProjecaoAno2 = registro.ProjecaoAno2,
                    ProjecaoAno3 = registro.ProjecaoAno3,
                    ProjecaoAno4 = registro.ProjecaoAno4,
                    ProjecaoAno5 = registro.ProjecaoAno5,
                    ParceiroInterno = registro.ParceiroInterno
                })
                .ToListAsync();

            return new IndicadoresReceitaGestorViewModel
            {
                Casa = casa,
                AnoBase = ano,
                PodeEditar = UsuarioPodeEditarReceitasGestor(casa),
                CasasDisponiveis = FunilHelpers.ObterCasasPermitidas(UsuarioAtivo)
                    .Where(instituto => instituto != Instituto.Super && instituto != Instituto.ISIII && instituto != Instituto.ISISVP)
                    .ToList(),
                Linhas = linhas
            };
        }

        private Instituto ResolverCasaReceitasGestor(string casa)
        {
            List<Instituto> casasPermitidas = FunilHelpers.ObterCasasPermitidas(UsuarioAtivo);
            if (FunilHelpers.TentarParseInstituto(casa, out Instituto casaSelecionada) && casasPermitidas.Contains(casaSelecionada))
            {
                return casaSelecionada;
            }

            if (casasPermitidas.Contains(UsuarioAtivo.Casa))
            {
                return UsuarioAtivo.Casa;
            }

            return casasPermitidas.FirstOrDefault();
        }

        private bool UsuarioPodeEditarReceitasGestor(Instituto casa)
        {
            return UsuarioAtivo != null
                && UsuarioAtivo.Nivel != Nivel.Externos
                && FunilHelpers.UsuarioPodeAcessarCasa(UsuarioAtivo, casa);
        }

        private string ObterTextoFormularioReceitas(int indice, string campo, int tamanhoMaximo)
        {
            string valor = Request.Form[$"linhas[{indice}].{campo}"].FirstOrDefault()?.Trim();
            if (string.IsNullOrWhiteSpace(valor))
            {
                return null;
            }

            return valor.Length <= tamanhoMaximo ? valor : valor.Substring(0, tamanhoMaximo);
        }

        private decimal? ObterDecimalFormularioReceitas(int indice, string campo)
        {
            string valorTexto = Request.Form[$"linhas[{indice}].{campo}"].FirstOrDefault();
            return TentarConverterDecimal(valorTexto, out decimal valor) ? valor : (decimal?)null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("FunilDeVendas/SalvarPlanejamentoIndicadores")]
        public async Task<IActionResult> SalvarPlanejamentoIndicadores(string casa, int ano)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Forbidden");
            }

            ViewbagizarUsuario(_context, _cache);

            if (!UsuarioPodeEditarPlanejamentoIndicadores())
            {
                return View("Forbidden");
            }

            Instituto casaPlanejamento = ResolverCasaPlanejamentoIndicadores(casa);
            List<string> chaves = ObterDefinicoesPlanejamentoIndicadores().Select(definicao => definicao.Chave).ToList();
            int[] colunas = ObterColunasPlanejamentoIndicadores();

            var existentes = await _context.IndicadoresPlanejamentoMensal
                .Where(indicador => indicador.Casa == casaPlanejamento
                    && indicador.Ano == ano
                    && chaves.Contains(indicador.Indicador))
                .ToListAsync();

            foreach (string chave in chaves)
            {
                foreach (int coluna in colunas)
                {
                    string campo = $"valor_{chave}_{coluna}";
                    string valorTexto = Request.Form[campo].FirstOrDefault();
                    IndicadoresPlanejamentoMensal existente = existentes.FirstOrDefault(indicador => indicador.Indicador == chave && indicador.Coluna == coluna);

                    if (IndicadorPlanejamentoCalculado(chave, coluna))
                    {
                        if (existente != null)
                        {
                            _context.IndicadoresPlanejamentoMensal.Remove(existente);
                        }

                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(valorTexto))
                    {
                        if (existente != null)
                        {
                            _context.IndicadoresPlanejamentoMensal.Remove(existente);
                        }

                        continue;
                    }

                    if (!TentarConverterDecimal(valorTexto, out decimal valor))
                    {
                        continue;
                    }

                    if (existente == null)
                    {
                        await _context.IndicadoresPlanejamentoMensal.AddAsync(new IndicadoresPlanejamentoMensal
                        {
                            Casa = casaPlanejamento,
                            Ano = ano,
                            Indicador = chave,
                            Coluna = coluna,
                            Valor = valor
                        });
                    }
                    else
                    {
                        existente.Valor = valor;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { casa = casaPlanejamento.ToString(), aba = "planejamento", ano });
        }

        private async Task<IndicadoresPlanejamentoViewModel> MontarPlanejamentoIndicadores(Instituto casa, int ano)
        {
            int[] colunas = ObterColunasPlanejamentoIndicadores();
            int totalEmpresas = await _context.Empresa.CountAsync();
            int totalEmpresasForaEstado = await _context.Empresa
                .CountAsync(empresa => empresa.Estado != Estado.RioDeJaneiro
                    && empresa.Estado != Estado.SemCadastro);
            var contatosIniciaisAno = await _context.FollowUp
                .AsNoTracking()
                .Where(followUp => followUp.Origem != null
                    && followUp.Origem.Casa == casa
                    && followUp.Data.Year == ano
                    && followUp.Status == StatusProspeccao.ContatoInicial)
                .Select(followUp => new
                {
                    followUp.OrigemID,
                    followUp.Data,
                    followUp.Origem.ValorProposta
                })
                .ToListAsync();
            int totalContatosRegistradosReal = contatosIniciaisAno
                .Select(contato => contato.OrigemID)
                .Distinct()
                .Count();
            Dictionary<int, int> contatosRealizadosAcumulados = Enumerable.Range(1, 12)
                .ToDictionary(
                    mes => mes,
                    mes => contatosIniciaisAno
                        .Where(contato => contato.Data.Month <= mes)
                        .Select(contato => contato.OrigemID)
                        .Distinct()
                        .Count());
            var ndasAssinadosAno = await _context.FollowUp
                .AsNoTracking()
                .Where(followUp => followUp.Origem != null
                    && followUp.Origem.Casa == casa
                    && followUp.Data.Year == ano
                    && followUp.Status == StatusProspeccao.NDAAssinado)
                .Select(followUp => new
                {
                    followUp.OrigemID,
                    followUp.Data,
                    followUp.Origem.ValorProposta
                })
                .ToListAsync();
            int totalNdasAssinados = ndasAssinadosAno
                .Select(nda => nda.OrigemID)
                .Distinct()
                .Count();
            Dictionary<int, int> ndasAssinadosAcumulados = Enumerable.Range(1, 12)
                .ToDictionary(
                    mes => mes,
                    mes => ndasAssinadosAno
                        .Where(nda => nda.Data.Month <= mes)
                        .Select(nda => nda.OrigemID)
                        .Distinct()
                        .Count());
            var propostasEnviadasAno = await _context.FollowUp
                .AsNoTracking()
                .Where(followUp => followUp.Origem != null
                    && followUp.Origem.Casa == casa
                    && followUp.Data.Year == ano
                    && followUp.Status == StatusProspeccao.ComProposta)
                .Select(followUp => new
                {
                    followUp.OrigemID,
                    followUp.Data,
                    followUp.Origem.ValorEstimado
                })
                .ToListAsync();
            int totalPropostasEnviadas = propostasEnviadasAno
                .Select(proposta => proposta.OrigemID)
                .Distinct()
                .Count();
            Dictionary<int, int> propostasEnviadasAcumuladas = Enumerable.Range(1, 12)
                .ToDictionary(
                    mes => mes,
                    mes => propostasEnviadasAno
                        .Where(proposta => proposta.Data.Month <= mes)
                        .Select(proposta => proposta.OrigemID)
                        .Distinct()
                        .Count());
            decimal totalValorPropostasEnviadas = propostasEnviadasAno
                .GroupBy(proposta => proposta.OrigemID)
                .Select(grupo => grupo.First().ValorEstimado)
                .Sum();
            Dictionary<int, decimal> valorPropostasEnviadasAcumulado = Enumerable.Range(1, 12)
                .ToDictionary(
                    mes => mes,
                    mes => propostasEnviadasAno
                        .Where(proposta => proposta.Data.Month <= mes)
                        .GroupBy(proposta => proposta.OrigemID)
                        .Select(grupo => grupo.First().ValorEstimado)
                        .Sum());
            var projetosConvertidosAno = await _context.FollowUp
                .AsNoTracking()
                .Where(followUp => followUp.Origem != null
                    && followUp.Origem.Casa == casa
                    && followUp.Data.Year == ano
                    && followUp.Status == StatusProspeccao.Convertida)
                .Select(followUp => new
                {
                    followUp.OrigemID,
                    followUp.Data,
                    followUp.Origem.ValorProposta
                })
                .ToListAsync();
            int totalProjetosConvertidos = projetosConvertidosAno
                .Select(projeto => projeto.OrigemID)
                .Distinct()
                .Count();
            Dictionary<int, int> projetosConvertidosAcumulados = Enumerable.Range(1, 12)
                .ToDictionary(
                    mes => mes,
                    mes => projetosConvertidosAno
                        .Where(projeto => projeto.Data.Month <= mes)
                        .Select(projeto => projeto.OrigemID)
                        .Distinct()
                        .Count());
            decimal totalValorProjetosConvertidos = projetosConvertidosAno
                .GroupBy(projeto => projeto.OrigemID)
                .Select(grupo => grupo.First().ValorProposta)
                .Sum();
            Dictionary<int, decimal> valorProjetosConvertidosAcumulado = Enumerable.Range(1, 12)
                .ToDictionary(
                    mes => mes,
                    mes => projetosConvertidosAno
                        .Where(projeto => projeto.Data.Month <= mes)
                        .GroupBy(projeto => projeto.OrigemID)
                        .Select(grupo => grupo.First().ValorProposta)
                        .Sum());
            var registros = await _context.IndicadoresPlanejamentoMensal
                .AsNoTracking()
                .Where(indicador => indicador.Casa == casa && indicador.Ano == ano)
                .ToListAsync();

            var model = new IndicadoresPlanejamentoViewModel
            {
                Casa = casa,
                Ano = ano,
                PodeEditar = UsuarioPodeEditarPlanejamentoIndicadores(),
                CasasDisponiveis = FunilHelpers.ObterCasasPermitidas(UsuarioAtivo)
            };

            foreach (IndicadorPlanejamentoDefinicao definicao in ObterDefinicoesPlanejamentoIndicadores())
            {
                var linha = new IndicadoresPlanejamentoLinhaViewModel
                {
                    Grupo = definicao.Grupo,
                    Chave = definicao.Chave,
                    Nome = definicao.Nome
                };

                foreach (int coluna in colunas)
                {
                    if (definicao.Chave == "TOTAL_EMPRESAS" && coluna == -2)
                    {
                        linha.Valores[coluna] = totalEmpresas;
                    }
                    else if (definicao.Chave == "CONTATOS_REGISTRADOS_REAL" && coluna == -2)
                    {
                        linha.Valores[coluna] = totalContatosRegistradosReal;
                    }
                    else if (definicao.Chave == "CONTATOS_REALIZADOS")
                    {
                        if (coluna == -2)
                        {
                            linha.Valores[coluna] = totalContatosRegistradosReal;
                        }
                        else if (coluna >= 1 && coluna <= 12)
                        {
                            linha.Valores[coluna] = contatosRealizadosAcumulados[coluna];
                        }
                        else
                        {
                            linha.Valores[coluna] = null;
                        }
                    }
                    else if (definicao.Chave == "NDA_ASSINADOS")
                    {
                        if (coluna == -2)
                        {
                            linha.Valores[coluna] = totalNdasAssinados;
                        }
                        else if (coluna == -1)
                        {
                            linha.Valores[coluna] = totalContatosRegistradosReal != 0
                                ? Math.Round((decimal)totalNdasAssinados / totalContatosRegistradosReal * 100, 2)
                                : (decimal?)null;
                        }
                        else if (coluna >= 1 && coluna <= 12)
                        {
                            linha.Valores[coluna] = ndasAssinadosAcumulados[coluna];
                        }
                        else
                        {
                            linha.Valores[coluna] = null;
                        }
                    }
                    else if (definicao.Chave == "PROPOSTAS_ENVIADAS")
                    {
                        if (coluna == -2)
                        {
                            linha.Valores[coluna] = totalPropostasEnviadas;
                        }
                        else if (coluna == -1)
                        {
                            linha.Valores[coluna] = totalContatosRegistradosReal != 0
                                ? Math.Round((decimal)totalPropostasEnviadas / totalContatosRegistradosReal * 100, 2)
                                : (decimal?)null;
                        }
                        else if (coluna >= 1 && coluna <= 12)
                        {
                            linha.Valores[coluna] = propostasEnviadasAcumuladas[coluna];
                        }
                        else
                        {
                            linha.Valores[coluna] = null;
                        }
                    }
                    else if (definicao.Chave == "VALOR_PROPOSTA")
                    {
                        if (coluna == -2)
                        {
                            linha.Valores[coluna] = totalValorPropostasEnviadas;
                        }
                        else if (coluna >= 1 && coluna <= 12)
                        {
                            linha.Valores[coluna] = valorPropostasEnviadasAcumulado[coluna];
                        }
                        else
                        {
                            linha.Valores[coluna] = null;
                        }
                    }
                    else if (definicao.Chave == "VALOR_MEDIO_PROPOSTA" && coluna == -2)
                    {
                        linha.Valores[coluna] = totalPropostasEnviadas != 0
                            ? Math.Round(totalValorPropostasEnviadas / totalPropostasEnviadas, 2)
                            : 0;
                    }
                    else if (definicao.Chave == "PROJETOS_CONVERTIDOS")
                    {
                        if (coluna == -2)
                        {
                            linha.Valores[coluna] = totalProjetosConvertidos;
                        }
                        else if (coluna == -1)
                        {
                            linha.Valores[coluna] = totalPropostasEnviadas != 0
                                ? Math.Round((decimal)totalProjetosConvertidos / totalPropostasEnviadas * 100, 2)
                                : (decimal?)null;
                        }
                        else if (coluna >= 1 && coluna <= 12)
                        {
                            linha.Valores[coluna] = projetosConvertidosAcumulados[coluna];
                        }
                        else
                        {
                            linha.Valores[coluna] = null;
                        }
                    }
                    else if (definicao.Chave == "VALOR_TOTAL_PROJETOS_CONVERTIDOS")
                    {
                        if (coluna == -2)
                        {
                            linha.Valores[coluna] = totalValorProjetosConvertidos;
                        }
                        else if (coluna >= 1 && coluna <= 12)
                        {
                            linha.Valores[coluna] = valorProjetosConvertidosAcumulado[coluna];
                        }
                        else
                        {
                            linha.Valores[coluna] = null;
                        }
                    }
                    else if (definicao.Chave == "VALOR_MEDIO_PROJETOS_CONVERTIDOS" && coluna == -2)
                    {
                        linha.Valores[coluna] = totalProjetosConvertidos != 0
                            ? Math.Round(totalValorProjetosConvertidos / totalProjetosConvertidos, 2)
                            : 0;
                    }
                    else if (definicao.Chave == "EMPRESAS_FORA_ESTADO_PLANEJAMENTO" && coluna == -2)
                    {
                        linha.Valores[coluna] = totalEmpresasForaEstado;
                    }
                    else
                    {
                        linha.Valores[coluna] = registros
                            .FirstOrDefault(indicador => indicador.Indicador == definicao.Chave && indicador.Coluna == coluna)
                            ?.Valor;
                    }
                }

                model.Linhas.Add(linha);
            }

            AplicarFormulasPlanejamentoIndicadores(model);

            return model;
        }

        private Instituto ResolverCasaPlanejamentoIndicadores(string casa)
        {
            List<Instituto> casasPermitidas = FunilHelpers.ObterCasasPermitidas(UsuarioAtivo);

            if (FunilHelpers.TentarParseInstituto(casa, out Instituto casaSelecionada) && casasPermitidas.Contains(casaSelecionada))
            {
                return casaSelecionada;
            }

            return ObterCasaPadraoAbertura();
        }

        private bool UsuarioPodeEditarPlanejamentoIndicadores()
        {
            return UsuarioAtivo != null
                && (UsuarioAtivo.Nivel == Nivel.Supervisor || UsuarioAtivo.Nivel == Nivel.Dev);
        }

        private static bool IndicadorPlanejamentoCalculado(string chave, int coluna)
        {
            return (chave == "TOTAL_EMPRESAS" && coluna == -2)
                || (chave == "CONTATOS_REGISTRADOS_REAL" && coluna == -2)
                || (chave == "CONTATOS_REGISTRADOS_REAL" && coluna == -1)
                || (chave == "EMPRESAS_FORA_ESTADO_PLANEJAMENTO" && coluna == -1)
                || (chave == "EMPRESAS_FORA_ESTADO_PLANEJAMENTO" && coluna == -2)
                || (chave == "CONTATOS_REALIZADOS_META" && coluna >= -2 && coluna <= 12)
                || (chave == "CONTATOS_REALIZADOS" && coluna >= -2 && coluna <= 12)
                || (chave == "NDA_ASSINADOS" && coluna >= -2 && coluna <= 12)
                || (chave == "TAXA_ASSINATURA" && coluna >= -2 && coluna <= 12)
                || (chave == "PROPOSTAS_ENVIADAS" && coluna >= -2 && coluna <= 12)
                || (chave == "TAXA_PROPOSTA_ENVIADA" && coluna >= -2 && coluna <= 12)
                || (chave == "VALOR_PROPOSTA" && coluna >= -2 && coluna <= 12)
                || (chave == "VALOR_MEDIO_PROPOSTA" && coluna == -2)
                || (chave == "VALOR_MEDIO_PROPOSTA_ATIVA" && coluna == -2)
                || (chave == "PROJETOS_CONVERTIDOS" && coluna >= -2 && coluna <= 12)
                || (chave == "VALOR_TOTAL_PROJETOS_CONVERTIDOS" && coluna >= -2 && coluna <= 12)
                || (chave == "VALOR_MEDIO_PROJETOS_CONVERTIDOS" && coluna == -2)
                || (chave == "PERCENTUAL_PROJETOS_CONVERTIDOS_META" && (coluna == -1 || (coluna >= 0 && coluna <= 12)))
                || (chave == "DIVERGENCIA_MENSAL" && coluna >= 1 && coluna <= 12)
                || (chave == "CURVA_CONTATOS_PLANEJADA" && coluna >= 1 && coluna <= 12)
                || ((chave == "CURVA_PLANEJADA" || chave == "CURVA_REAL" || chave == "CURVA_PROPOSTAS" || chave == "CURVA_PLANEJADA_PROJETOS") && coluna >= -2 && coluna <= 12);
        }

        private static void AplicarFormulasPlanejamentoIndicadores(IndicadoresPlanejamentoViewModel model)
        {
            IndicadoresPlanejamentoLinhaViewModel contatosRegistradosMeta = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "CONTATOS_REGISTRADOS_META");
            IndicadoresPlanejamentoLinhaViewModel totalEmpresas = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "TOTAL_EMPRESAS");
            IndicadoresPlanejamentoLinhaViewModel contatosRegistradosReal = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "CONTATOS_REGISTRADOS_REAL");
            IndicadoresPlanejamentoLinhaViewModel empresasForaEstadoPlanejamento = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "EMPRESAS_FORA_ESTADO_PLANEJAMENTO");
            IndicadoresPlanejamentoLinhaViewModel curvaContatosPlanejada = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "CURVA_CONTATOS_PLANEJADA");
            IndicadoresPlanejamentoLinhaViewModel contatosRealizadosMeta = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "CONTATOS_REALIZADOS_META");
            IndicadoresPlanejamentoLinhaViewModel totalEmpresasPlanejadas = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "TOTAL_EMPRESAS_PLANEJADAS");
            IndicadoresPlanejamentoLinhaViewModel contatosRealizados = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "CONTATOS_REALIZADOS");
            IndicadoresPlanejamentoLinhaViewModel ndasAssinados = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "NDA_ASSINADOS");
            IndicadoresPlanejamentoLinhaViewModel taxaAssinatura = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "TAXA_ASSINATURA");
            IndicadoresPlanejamentoLinhaViewModel propostasEnviadas = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "PROPOSTAS_ENVIADAS");
            IndicadoresPlanejamentoLinhaViewModel propostasAindaValidas = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "PROPOSTAS_AINDA_VALIDAS");
            IndicadoresPlanejamentoLinhaViewModel valorPropostaAtiva = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "VALOR_PROPOSTA_ATIVA");
            IndicadoresPlanejamentoLinhaViewModel valorMedioPropostaAtiva = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "VALOR_MEDIO_PROPOSTA_ATIVA");
            IndicadoresPlanejamentoLinhaViewModel taxaPropostaEnviada = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "TAXA_PROPOSTA_ENVIADA");
            IndicadoresPlanejamentoLinhaViewModel projetosConvertidosMeta = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "PROJETOS_CONVERTIDOS_META");
            IndicadoresPlanejamentoLinhaViewModel projetosConvertidos = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "PROJETOS_CONVERTIDOS");
            IndicadoresPlanejamentoLinhaViewModel percentualProjetosConvertidosMeta = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "PERCENTUAL_PROJETOS_CONVERTIDOS_META");
            IndicadoresPlanejamentoLinhaViewModel divergenciaMensal = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "DIVERGENCIA_MENSAL");
            IndicadoresPlanejamentoLinhaViewModel curvaPlanejada = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "CURVA_PLANEJADA");
            IndicadoresPlanejamentoLinhaViewModel curvaReal = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "CURVA_REAL");
            IndicadoresPlanejamentoLinhaViewModel propostasEnviadasMeta = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "PROPOSTAS_ENVIADAS_META");
            IndicadoresPlanejamentoLinhaViewModel curvaPropostas = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "CURVA_PROPOSTAS");
            IndicadoresPlanejamentoLinhaViewModel curvaPlanejadaProjetos = model.Linhas
                .FirstOrDefault(linha => linha.Chave == "CURVA_PLANEJADA_PROJETOS");

            if (contatosRegistradosMeta == null || curvaContatosPlanejada == null)
            {
                return;
            }

            if (contatosRegistradosReal != null)
            {
                decimal? totalReal = ObterValorPlanejamento(contatosRegistradosReal, -2);
                decimal? totalMetaContatos = ObterValorPlanejamento(contatosRegistradosMeta, -2);
                contatosRegistradosReal.Valores[-1] = totalMetaContatos.HasValue && totalMetaContatos.Value != 0 && totalReal.HasValue
                    ? Math.Round(totalReal.Value / totalMetaContatos.Value * 100, 2)
                    : (decimal?)null;
            }

            if (empresasForaEstadoPlanejamento != null && totalEmpresas != null)
            {
                decimal? empresasForaEstado = ObterValorPlanejamento(empresasForaEstadoPlanejamento, -2);
                decimal? totalEmpresasCadastradas = ObterValorPlanejamento(totalEmpresas, -2);
                empresasForaEstadoPlanejamento.Valores[-1] = totalEmpresasCadastradas.HasValue && totalEmpresasCadastradas.Value != 0 && empresasForaEstado.HasValue
                    ? Math.Round(empresasForaEstado.Value / totalEmpresasCadastradas.Value * 100, 2)
                    : (decimal?)null;
            }

            if (contatosRealizadosMeta != null)
            {
                foreach (int coluna in ObterColunasPlanejamentoIndicadores())
                {
                    contatosRealizadosMeta.Valores[coluna] = ObterValorPlanejamento(contatosRegistradosMeta, coluna);
                }
            }

            if (divergenciaMensal != null && contatosRealizadosMeta != null)
            {
                for (int mes = 1; mes <= 12; mes++)
                {
                    decimal? metaMes = ObterValorPlanejamento(contatosRealizadosMeta, mes);
                    decimal? metaMesAnterior = ObterValorPlanejamento(contatosRealizadosMeta, mes - 1);
                    divergenciaMensal.Valores[mes] = metaMes.HasValue && metaMesAnterior.HasValue
                        ? metaMes.Value - metaMesAnterior.Value
                        : (decimal?)null;
                }
            }

            AplicarCurvaPercentualMensal(curvaPlanejada, contatosRealizadosMeta);
            AplicarCurvaPercentualMensal(curvaReal, contatosRealizados, contatosRealizadosMeta);
            AplicarCurvaPercentualMensal(curvaPropostas, propostasEnviadasMeta);
            AplicarCurvaPercentualMensal(curvaPlanejadaProjetos, projetosConvertidosMeta);

            if (contatosRealizados != null && totalEmpresasPlanejadas != null)
            {
                decimal? totalContatosRealizados = ObterValorPlanejamento(contatosRealizados, -2);
                decimal? totalPlanejado = ObterValorPlanejamento(totalEmpresasPlanejadas, -2);
                contatosRealizados.Valores[-1] = totalPlanejado.HasValue && totalPlanejado.Value != 0 && totalContatosRealizados.HasValue
                    ? Math.Round(totalContatosRealizados.Value / totalPlanejado.Value * 100, 2)
                    : (decimal?)null;
            }

            if (taxaAssinatura != null)
            {
                foreach (int coluna in ObterColunasPlanejamentoIndicadores())
                {
                    taxaAssinatura.Valores[coluna] = null;
                }

                if (ndasAssinados != null && contatosRealizados != null)
                {
                    for (int mes = 1; mes <= 12; mes++)
                    {
                        decimal? ndaMes = ObterValorPlanejamento(ndasAssinados, mes);
                        decimal? contatosMes = ObterValorPlanejamento(contatosRealizados, mes);
                        taxaAssinatura.Valores[mes] = contatosMes.HasValue && contatosMes.Value != 0 && ndaMes.HasValue
                            ? Math.Round(ndaMes.Value / contatosMes.Value * 100, 2)
                            : (decimal?)null;
                    }
                }
            }

            if (taxaPropostaEnviada != null)
            {
                foreach (int coluna in ObterColunasPlanejamentoIndicadores())
                {
                    taxaPropostaEnviada.Valores[coluna] = null;
                }

                if (propostasEnviadas != null && contatosRealizados != null)
                {
                    for (int mes = 1; mes <= 12; mes++)
                    {
                        decimal? propostasMes = ObterValorPlanejamento(propostasEnviadas, mes);
                        decimal? contatosMes = ObterValorPlanejamento(contatosRealizados, mes);
                        taxaPropostaEnviada.Valores[mes] = contatosMes.HasValue && contatosMes.Value != 0 && propostasMes.HasValue
                            ? Math.Round(propostasMes.Value / contatosMes.Value * 100, 2)
                            : (decimal?)null;
                    }
                }
            }

            if (valorMedioPropostaAtiva != null)
            {
                decimal? totalValorPropostaAtiva = ObterValorPlanejamento(valorPropostaAtiva, -2);
                decimal? totalPropostasAindaValidas = ObterValorPlanejamento(propostasAindaValidas, -2);
                valorMedioPropostaAtiva.Valores[-2] = totalPropostasAindaValidas.HasValue && totalPropostasAindaValidas.Value != 0 && totalValorPropostaAtiva.HasValue
                    ? Math.Round(totalValorPropostaAtiva.Value / totalPropostasAindaValidas.Value, 2)
                    : 0;
            }

            if (percentualProjetosConvertidosMeta != null)
            {
                foreach (int coluna in ObterColunasPlanejamentoIndicadores().Where(coluna => coluna != -2))
                {
                    percentualProjetosConvertidosMeta.Valores[coluna] = null;
                }

                if (projetosConvertidos != null && projetosConvertidosMeta != null)
                {
                    decimal? totalConvertidos = ObterValorPlanejamento(projetosConvertidos, -2);
                    decimal? totalMetaConvertidos = ObterValorPlanejamento(projetosConvertidosMeta, -2);
                    percentualProjetosConvertidosMeta.Valores[-1] = totalMetaConvertidos.HasValue && totalMetaConvertidos.Value != 0 && totalConvertidos.HasValue
                        ? Math.Round(totalConvertidos.Value / totalMetaConvertidos.Value * 100, 2)
                        : 0;

                    for (int mes = 1; mes <= 12; mes++)
                    {
                        decimal? convertidosMes = ObterValorPlanejamento(projetosConvertidos, mes);
                        percentualProjetosConvertidosMeta.Valores[mes] = totalMetaConvertidos.HasValue && totalMetaConvertidos.Value != 0 && convertidosMes.HasValue
                            ? Math.Round(convertidosMes.Value / totalMetaConvertidos.Value * 100, 2)
                            : 0;
                    }
                }
            }

            decimal? totalMeta = ObterValorPlanejamento(contatosRegistradosMeta, -2)
                ?? ObterValorPlanejamento(contatosRegistradosMeta, 12);

            for (int mes = 1; mes <= 12; mes++)
            {
                decimal? contatosMes = ObterValorPlanejamento(contatosRegistradosMeta, mes);
                curvaContatosPlanejada.Valores[mes] = totalMeta.HasValue && totalMeta.Value != 0 && contatosMes.HasValue
                    ? Math.Round(contatosMes.Value / totalMeta.Value * 100, 2)
                    : (decimal?)null;
            }
        }

        private static void AplicarCurvaPercentualMensal(
            IndicadoresPlanejamentoLinhaViewModel curva,
            IndicadoresPlanejamentoLinhaViewModel meta)
        {
            if (curva == null || meta == null)
            {
                return;
            }

            foreach (int coluna in ObterColunasPlanejamentoIndicadores())
            {
                curva.Valores[coluna] = null;
            }

            decimal? totalMeta = ObterValorPlanejamento(meta, -2);

            for (int mes = 1; mes <= 12; mes++)
            {
                decimal? metaMes = ObterValorPlanejamento(meta, mes);
                curva.Valores[mes] = totalMeta.HasValue && totalMeta.Value != 0 && metaMes.HasValue
                    ? Math.Round(metaMes.Value / totalMeta.Value * 100, 2)
                    : 0;
            }
        }

        private static void AplicarCurvaPercentualMensal(
            IndicadoresPlanejamentoLinhaViewModel curva,
            IndicadoresPlanejamentoLinhaViewModel numerador,
            IndicadoresPlanejamentoLinhaViewModel denominador)
        {
            if (curva == null || numerador == null || denominador == null)
            {
                return;
            }

            foreach (int coluna in ObterColunasPlanejamentoIndicadores())
            {
                curva.Valores[coluna] = null;
            }

            for (int mes = 1; mes <= 12; mes++)
            {
                decimal? valorNumerador = ObterValorPlanejamento(numerador, mes);
                decimal? valorDenominador = ObterValorPlanejamento(denominador, mes);
                curva.Valores[mes] = valorDenominador.HasValue && valorDenominador.Value != 0 && valorNumerador.HasValue
                    ? Math.Round(valorNumerador.Value / valorDenominador.Value * 100, 2)
                    : 0;
            }
        }

        private static decimal? ObterValorPlanejamento(IndicadoresPlanejamentoLinhaViewModel linha, int coluna)
        {
            return linha.Valores.ContainsKey(coluna) ? linha.Valores[coluna] : null;
        }

        private static bool TentarConverterDecimal(string valorTexto, out decimal valor)
        {
            valorTexto = valorTexto?
                .Trim()
                .Replace("R$", "")
                .Replace("%", "")
                .Replace(" ", "");

            if (string.IsNullOrWhiteSpace(valorTexto))
            {
                valor = 0;
                return false;
            }

            CultureInfo culturaBrasileira = new CultureInfo("pt-BR");

            if (valorTexto.Contains(","))
            {
                return decimal.TryParse(valorTexto, NumberStyles.Number, culturaBrasileira, out valor)
                    || decimal.TryParse(valorTexto, NumberStyles.Float, CultureInfo.InvariantCulture, out valor);
            }

            return decimal.TryParse(valorTexto, NumberStyles.Float, CultureInfo.InvariantCulture, out valor)
                || decimal.TryParse(valorTexto, NumberStyles.Number, culturaBrasileira, out valor);
        }

        private async Task AtualizarComposicaoValoresProspeccao(string prospeccaoId)
        {
            var valores = ObterComposicaoValoresFormulario(prospeccaoId);
            var valoresExistentes = await _context.ProspeccaoValorComposicao
                .Where(valor => valor.ProspeccaoId == prospeccaoId)
                .ToListAsync();

            _context.ProspeccaoValorComposicao.RemoveRange(valoresExistentes);

            if (valores.Any())
            {
                await _context.ProspeccaoValorComposicao.AddRangeAsync(valores);
            }
        }

        private List<ProspeccaoValorComposicao> ObterComposicaoValoresFormulario(string prospeccaoId)
        {
            const string prefixo = "ComposicaoValores[";
            var indices = Request.Form.Keys
                .Where(chave => chave.StartsWith(prefixo) && chave.Contains("]."))
                .Select(chave =>
                {
                    int inicio = prefixo.Length;
                    int fim = chave.IndexOf("].", StringComparison.Ordinal);
                    return fim > inicio && int.TryParse(chave.Substring(inicio, fim - inicio), out int indice)
                        ? indice
                        : -1;
                })
                .Where(indice => indice >= 0)
                .Distinct()
                .OrderBy(indice => indice)
                .ToList();

            var valores = new List<ProspeccaoValorComposicao>();

            foreach (int indice in indices)
            {
                string prefixoCampo = $"ComposicaoValores[{indice}].";
                string origem = Request.Form[prefixoCampo + "Origem"].FirstOrDefault()?.Trim();
                string tipo = Request.Form[prefixoCampo + "Tipo"].FirstOrDefault()?.Trim();
                string natureza = Request.Form[prefixoCampo + "Natureza"].FirstOrDefault()?.Trim();
                string observacao = Request.Form[prefixoCampo + "Observacao"].FirstOrDefault()?.Trim();
                string valorTexto = Request.Form[prefixoCampo + "Valor"].FirstOrDefault();

                decimal? valor = null;
                if (TentarConverterDecimal(valorTexto, out decimal valorConvertido))
                {
                    valor = valorConvertido;
                }

                bool linhaVazia = string.IsNullOrWhiteSpace(origem)
                    && string.IsNullOrWhiteSpace(tipo)
                    && string.IsNullOrWhiteSpace(natureza)
                    && string.IsNullOrWhiteSpace(observacao)
                    && !valor.HasValue;

                if (linhaVazia)
                {
                    continue;
                }

                valores.Add(new ProspeccaoValorComposicao
                {
                    ProspeccaoId = prospeccaoId,
                    Origem = origem,
                    Tipo = tipo,
                    Natureza = natureza,
                    Valor = valor,
                    Observacao = observacao
                });
            }

            return valores;
        }

        private static int[] ObterColunasPlanejamentoIndicadores()
        {
            return new[] { -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        }

        private static List<IndicadorPlanejamentoDefinicao> ObterDefinicoesPlanejamentoIndicadores()
        {
            return new List<IndicadorPlanejamentoDefinicao>
            {
                new IndicadorPlanejamentoDefinicao("Planejamento", "TOTAL_EMPRESAS_PLANEJADAS", "Total de Empresas Planejadas"),
                new IndicadorPlanejamentoDefinicao("Planejamento", "TOTAL_EMPRESAS", "Total de Empresas"),
                new IndicadorPlanejamentoDefinicao("Planejamento", "CONTATOS_REGISTRADOS_META", "Contatos Registrados (Meta)"),
                new IndicadorPlanejamentoDefinicao("Planejamento", "CURVA_CONTATOS_PLANEJADA", "Curva de contatos planejada"),
                new IndicadorPlanejamentoDefinicao("Planejamento", "CONTATOS_REGISTRADOS_REAL", "Contatos Registrados (Real)"),
                new IndicadorPlanejamentoDefinicao("Planejamento", "EMPRESAS_FORA_ESTADO_PLANEJAMENTO", "Empresas Fora do Estado"),
                new IndicadorPlanejamentoDefinicao("Execucao", "CONTATOS_REALIZADOS_META", "Contatos Realizados (Meta)"),
                new IndicadorPlanejamentoDefinicao("Execucao", "DIVERGENCIA_MENSAL", "Divergencia Mensal"),
                new IndicadorPlanejamentoDefinicao("Execucao", "CURVA_PLANEJADA", "Curva planejada"),
                new IndicadorPlanejamentoDefinicao("Execucao", "CURVA_REAL", "Curva real"),
                new IndicadorPlanejamentoDefinicao("Execucao", "CONTATOS_REALIZADOS", "Contatos Realizados"),
                new IndicadorPlanejamentoDefinicao("Execucao", "NDA_ASSINADOS", "NDA assinados"),
                new IndicadorPlanejamentoDefinicao("Execucao", "TAXA_ASSINATURA", "Taxa de Assinatura"),
                new IndicadorPlanejamentoDefinicao("Execucao", "PROPOSTAS_ENVIADAS_META", "Propostas Enviadas (Meta)"),
                new IndicadorPlanejamentoDefinicao("Execucao", "CURVA_PROPOSTAS", "Curva de Propostas"),
                new IndicadorPlanejamentoDefinicao("Execucao", "PROPOSTAS_ENVIADAS", "Propostas Enviadas"),
                new IndicadorPlanejamentoDefinicao("Execucao", "PROPOSTAS_AINDA_VALIDAS", "Propostas ainda validas"),
                new IndicadorPlanejamentoDefinicao("Execucao", "TAXA_PROPOSTA_ENVIADA", "Taxa de Proposta Enviada"),
                new IndicadorPlanejamentoDefinicao("Execucao", "VALOR_PROPOSTA", "Valor de Proposta"),
                new IndicadorPlanejamentoDefinicao("Execucao", "VALOR_MEDIO_PROPOSTA", "Valor Medio de Proposta"),
                new IndicadorPlanejamentoDefinicao("Execucao", "VALOR_PROPOSTA_ATIVA", "Valor de Proposta Ativa"),
                new IndicadorPlanejamentoDefinicao("Execucao", "VALOR_MEDIO_PROPOSTA_ATIVA", "Valor Medio de Proposta Ativa"),
                new IndicadorPlanejamentoDefinicao("Execucao", "PROJETOS_CONVERTIDOS_META", "Projetos Convertidos (Meta)"),
                new IndicadorPlanejamentoDefinicao("Execucao", "CURVA_PLANEJADA_PROJETOS", "Curva planejada"),
                new IndicadorPlanejamentoDefinicao("Execucao", "PROJETOS_CONVERTIDOS", "Projetos Convertidos"),
                new IndicadorPlanejamentoDefinicao("Execucao", "PERCENTUAL_PROJETOS_CONVERTIDOS_META", "Percentual de Projetos Convertidos em Relacao a Meta"),
                new IndicadorPlanejamentoDefinicao("Execucao", "VALOR_TOTAL_PROJETOS_CONVERTIDOS", "Valor total de Projetos Convertidos"),
                new IndicadorPlanejamentoDefinicao("Execucao", "VALOR_MEDIO_PROJETOS_CONVERTIDOS", "Valor Medio de Projetos Convertidos"),
                new IndicadorPlanejamentoDefinicao("Execucao", "EMPRESAS_FORA_ESTADO_EXECUCAO", "Empresas Fora do Estado"),
                new IndicadorPlanejamentoDefinicao("Execucao", "PARCERIAS_SENAI_EXTERNAS", "Parcerias SENAI Externas")
            };
        }

        private class IndicadorPlanejamentoDefinicao
        {
            public IndicadorPlanejamentoDefinicao(string grupo, string chave, string nome)
            {
                Grupo = grupo;
                Chave = chave;
                Nome = nome;
            }

            public string Grupo { get; }
            public string Chave { get; }
            public string Nome { get; }
        }
        private void SetarParametrosFunilSession(ParametrosFunil parametrosFunil)
        {
            SetarAbaNaSession(parametrosFunil.Aba);
            SetarSearchStringNaSession(parametrosFunil.SearchString);
            SetarNumeroPaginaSession(parametrosFunil.NumeroPagina);
            SetarTamanhoPaginaSession(parametrosFunil.TamanhoPagina);
            SetarSortOrder(parametrosFunil.SortOrder);
        }

        private void SetarSortOrder(string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortOrder))
            {
                HttpContext.Session.Remove("sortOrder");
                return;
            }

            HttpContext.Session.SetString("sortOrder", sortOrder);
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
            if (string.IsNullOrWhiteSpace(searchString))
            {
                HttpContext.Session.Remove("searchString");
                HttpContext.Session.Remove("_CurrentFilter");
            }
            else
            {
                HttpContext.Session.SetString("searchString", searchString);
                HttpContext.Session.SetString("_CurrentFilter", searchString);
            }
        }

        private void SetarAbaNaSession(string aba)
        {
            if (aba != null)
            {
                HttpContext.Session.SetString("aba", aba);
            }
        }

        /// <summary>
        /// Planeja uma prospecção quando criada à partir do Módulo de Empresas
        /// </summary>
        /// <param name="id">ID da Empresa</param>
        /// <param name="userId">ID do Usuário</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Planejar(int id, string nomeProspeccao, Instituto casa)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                if (!FunilHelpers.UsuarioPodeAcessarCasa(UsuarioAtivo, casa))
                {
                    return View("Forbidden");
                }

                Empresa empresa = await _context.Empresa.FindAsync(id);
                if (empresa == null)
                {
                    return NotFound();
                }

                bool jaExistePlanejamento = await _context.Prospeccao
                    .AnyAsync(p => p.EmpresaId == id
                        && p.Casa == casa
                        && p.Status.Any(f => f.Status == StatusProspeccao.Planejada)
                        && !p.Status.Any(f => f.Status != StatusProspeccao.Planejada));

                if (jaExistePlanejamento)
                {
                    return RedirectToAction("Index", "Empresas");
                }

                string nomeEmpresa = !string.IsNullOrWhiteSpace(empresa.Nome)
                    ? empresa.Nome
                    : empresa.RazaoSocial;

                string nomePlanejamento = !string.IsNullOrWhiteSpace(nomeProspeccao)
                    ? nomeProspeccao.Trim()
                    : $"Planejamento - {nomeEmpresa}";

                Prospeccao prosp = new Prospeccao
                {
                    Id = $"prosp_{DateTime.Now.Ticks}",
                    EmpresaId = id,
                    Empresa = empresa,
                    NomeProspeccao = nomePlanejamento,
                    Usuario = await _context.Users.FirstOrDefaultAsync(u => u.UserName == UsuarioAtivo.UserName),
                    Casa = casa,
                    LinhaPequisa = LinhaPesquisa.Indefinida,
                    TipoContratacao = TipoContratacao.Indefinida,
                    TipoDeInteracao = TipoDeInteracao.Adefinir,
                    TipoDeProjeto = TipoDeProjeto.Adefinir,
                    ParceiroInterno = ParceiroInterno.Adefinir,
                    Origem = Origem.Adefinir,
                    Tipologia = "A definir",
                    MembrosEquipe = "",
                    PotenciaisParceiros = "",
                    CaminhoPasta = "",
                };

                prosp.Status = new List<FollowUp>
                {
                    new FollowUp
                    {
                        OrigemID = prosp.Id,
                        Origem = prosp,
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
                    ["Líder"] = !string.IsNullOrWhiteSpace(p.LiderNome) ? p.LiderNome : p.Usuario?.UserName,
                    // Converter e depois pegar o UserName
                    ["Membros"] = string.Join(" ", p.TratarMembrosEquipeString(_context).Select(u => u.UserName).ToList()),
                    ["Status"] = p.Status.OrderBy(k => k.Data).LastOrDefault().Status.GetDisplayName(),
                    ["Data_inicio"] = p.Status.OrderBy(k => k.Data).FirstOrDefault().Data.ToString("dd/MM/yyyy"),
                    ["Data_fim"] = p.Status.OrderBy(k => k.Data).LastOrDefault().Data.ToString("dd/MM/yyyy"),
                    ["qtde_followups"] = p.Status.Count(),
                    ["Empresa"] = p.Empresa?.Nome ?? "",
                    ["CNPJ"] = p.Empresa?.CNPJ ?? "",
                    ["Segmento"] = p.Empresa != null ? p.Empresa.Segmento.GetDisplayName() : "",
                    ["Estado"] = p.Empresa.Estado.GetDisplayName(),
                    ["Casa"] = p.Casa.GetDisplayName(),
                    ["Origem"] = p.Origem.GetDisplayName(),
                    ["TipoContratacao"] = p.TipoContratacao.GetDisplayName(),
                    ["LinhaPesquisa"] = p.LinhaPequisa.GetDisplayName(),
                    ["PrevisaoTempoProjetoMeses"] = p.PrevisaoTempoProjetoMeses,
                    ["LinkArquivo"] = p.LinkArquivo,
                    ["ValorEstimado"] = p.ValorEstimado,
                    ["ValorProposta"] = p.ValorProposta,
                    ["ValorFinal"] = p.ValorFinal ?? (p.ValorProposta != 0 ? p.ValorProposta : p.ValorEstimado),
                };

                if (string.IsNullOrEmpty(p.NomeProspeccao))
                {
                    dict["Titulo"] = "Sem título";
                }
                else
                {
                    dict["Titulo"] = p.NomeProspeccao;
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
                if (!string.IsNullOrEmpty(tipo))
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
                if (!string.IsNullOrEmpty(tipo))
                {
                    return ViewComponent($"Modal{tipo}Prosp", new { id = idProsp });
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
        /// Valida e cadastra uma empresa a uma prospecção
        /// </summary>
        /// <param name="prospeccao">Prospecção a ter uma empresa cadastrada</param>
        /// <returns></returns>
        /// <exception cref="Exception">Erro a ser lançado caso não seja possível cadastrar a empresa/seja uma empresa inválida</exception>
        private async Task<Pessoa> ReutilizarContatoExistenteDaEmpresa(Pessoa contato, int empresaId)
        {
            if (contato == null || empresaId <= 0)
            {
                return contato;
            }

            contato.Email = contato.Email?.Trim();
            contato.Telefone = contato.Telefone?.Trim();
            contato.Nome = contato.Nome?.Trim();
            contato.Cargo = contato.Cargo?.Trim();
            contato.EmpresaId = empresaId;

            string email = NormalizarEmail(contato.Email);
            string telefone = NormalizarTelefone(contato.Telefone);

            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(telefone))
            {
                return contato;
            }

            var contatosDaEmpresa = await _context.Pessoa
                .Where(p => p.EmpresaId == empresaId)
                .ToListAsync();

            var contatoExistente = contatosDaEmpresa.FirstOrDefault(p =>
                (!string.IsNullOrEmpty(email) && NormalizarEmail(p.Email) == email) ||
                (!string.IsNullOrEmpty(telefone) && NormalizarTelefone(p.Telefone) == telefone));

            return contatoExistente ?? contato;
        }

        private static string NormalizarEmail(string email)
        {
            return string.IsNullOrWhiteSpace(email)
                ? string.Empty
                : email.Trim().ToLowerInvariant();
        }

        private static string NormalizarTelefone(string telefone)
        {
            return string.IsNullOrWhiteSpace(telefone)
                ? string.Empty
                : new string(telefone.Where(char.IsDigit).ToArray());
        }

        private async Task<int> CriarEmpresaDaProspeccao(Empresa novaEmpresa)
        {
            if (novaEmpresa == null)
            {
                throw new ArgumentException("Informe os dados da nova empresa.");
            }

            novaEmpresa.Nome = novaEmpresa.Nome?.Trim();
            novaEmpresa.RazaoSocial = novaEmpresa.RazaoSocial?.Trim();
            novaEmpresa.CNPJ = novaEmpresa.CNPJ?.Trim();
            novaEmpresa.Porte = novaEmpresa.Porte?.Trim();

            if (string.IsNullOrWhiteSpace(novaEmpresa.Nome) || string.IsNullOrWhiteSpace(novaEmpresa.CNPJ))
            {
                throw new ArgumentException("Informe o nome fantasia e o CNPJ para cadastrar a nova empresa.");
            }

            if (string.IsNullOrWhiteSpace(novaEmpresa.RazaoSocial))
            {
                novaEmpresa.RazaoSocial = novaEmpresa.Nome;
            }

            novaEmpresa.Logo = "";

            _context.Empresa.Add(novaEmpresa);
            await _context.SaveChangesAsync();
            CacheHelper.CleanupEmpresasCache(_cache);

            return novaEmpresa.Id;
        }

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
        /// Valida as condições de remoção de uma prospecção
        /// </summary>
        /// <param name="prospeccao">Prospecção a se validada</param>
        /// <param name="dono">Usuario líder da prospecção</param>
        /// <param name="ativo">Usuário ativo (HttpContext)</param>
        /// <returns></returns>
        internal static bool VerificarCondicoesRemocao(Prospeccao prospeccao, Usuario ativoNaSessao, Usuario donoProsp)
        {
            // Valida nulidades
            if (prospeccao == null || ativoNaSessao == null || donoProsp == null)
                return false;

            // Deve haver mais de um status para permitir remoção
            if (prospeccao.Status == null || prospeccao.Status.Count() <= 1)
                return false;

            // Comparar por Id do usuário (evita comparação de instâncias diferentes)
            if (string.IsNullOrEmpty(ativoNaSessao.Id) || string.IsNullOrEmpty(donoProsp.Id))
                return false;

            return ativoNaSessao.Id == donoProsp.Id;
        }

        /// <summary>
        /// Vincula o usuário logado a uma prospecção
        /// </summary>
        /// <param name="prospeccao">Prospecção que se deseja vincular ao usuário logado</param>
        /// <returns></returns>
        internal static async Task VincularUsuario(Prospeccao prospeccao, HttpContext httpContext, ApplicationDbContext context, string liderProjeto = null)
        {
            DefinirLiderNome(prospeccao, liderProjeto);
            if (!string.IsNullOrWhiteSpace(prospeccao.LiderNome))
            {
                prospeccao.Usuario = null;
                return;
            }

            Usuario user = null;
            if (prospeccao.Usuario != null && !string.IsNullOrWhiteSpace(prospeccao.Usuario.Id))
            {
                user = await context.Users.FirstOrDefaultAsync(u => u.Id == prospeccao.Usuario.Id);
            }

            user ??= ResolverUsuarioLider(context, liderProjeto);
            if (user == null && string.IsNullOrWhiteSpace(liderProjeto))
            {
                string userId = httpContext.User.Identity.Name;
                user = await context.Users.FirstAsync(u => u.UserName == userId);
            }

            prospeccao.Usuario = user;
        }

        private static void DefinirLiderNome(Prospeccao prospeccao, string liderProjeto = null)
        {
            string liderNome = !string.IsNullOrWhiteSpace(prospeccao.LiderNome)
                ? prospeccao.LiderNome
                : liderProjeto;

            prospeccao.LiderNome = LimparNomeLider(liderNome);
        }

        private static string LimparNomeLider(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return null;
            }

            return valor.Split(new char[] { ',', ';' })[0].Trim();
        }

        private static Usuario ResolverUsuarioLider(ApplicationDbContext context, string valorLider)
        {
            if (string.IsNullOrWhiteSpace(valorLider))
            {
                return null;
            }

            string valor = NormalizarTextoUsuario(valorLider);
            return context.Users
                .AsEnumerable()
                .FirstOrDefault(u => NormalizarTextoUsuario(u.UserName) == valor);
        }

        private static string NormalizarTextoUsuario(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return string.Empty;
            }

            string texto = valor.Split(new char[] { ',', ';' })[0].Trim().ToUpperInvariant();
            string normalizado = texto.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (char c in normalizado)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
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
                RequestId = HttpContext.TraceIdentifier,
                Mensagem = e.Message
            };
            return View("Error", erro);
        }

        /// <summary>
        /// Cria um followup no Banco de Dados
        /// </summary>
        /// <param name="followup">Followup específico a ser criado no Banco de Dados</param>
        private async Task CriarFollowUp(FollowUp followup)
        {
            await _context.AddAsync(followup);
        }

        /// <summary>
        /// Cria uma selectlist para a View(?)
        /// </summary>
        private async Task CriarSelectListsDaView()
        {
            var empresas = await _cache.GetCachedAsync("Empresas:FunilUnique", () => _context.Empresa.Select(e => new EmpresasReadComUniqueDTO
            {
                EmpresaUnique = e.EmpresaUnique,
                Id = e.Id,
                Nome = e.Nome
            }).ToListAsync());

            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
            ViewData["Equipe"] = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
        }

        /// <summary>
        /// Método auxiliar para editar dados de uma prospecção
        /// </summary>
        /// <param name="id">Inutilizado</param>
        /// <param name="prospeccao">Prospecção a ter seus dados editados</param>
        /// <returns></returns>
        private async Task<Prospeccao> EditarDadosDaProspecao(string id, Prospeccao prospeccao, string liderProjeto = null)
        {
            DefinirLiderNome(prospeccao, liderProjeto);
            if (!string.IsNullOrWhiteSpace(prospeccao.LiderNome))
            {
                prospeccao.Usuario = null;
            }
            else
            {
            // Obter usuário líder a partir do DbContext (instância corretamente rastreada)
            Usuario lider = ResolverUsuarioLider(_context, liderProjeto);
            if (prospeccao.Usuario != null && !string.IsNullOrEmpty(prospeccao.Usuario.Id))
            {
                lider = await _context.Users.FirstOrDefaultAsync(p => p.Id == prospeccao.Usuario.Id);
            }
    
            // Se não encontrou no contexto, tentar fallback para cache (sem causar tracking conflict)
            if (lider == null)
            {
                var usuariosCache = await _cache.GetCachedAsync("AllUsuarios", () => _context.Users.ToListAsync());
                lider = usuariosCache.FirstOrDefault(p => p.Id == prospeccao.Usuario?.Id);
                if (lider != null)
                {
                    // garantir que usamos a instância rastreada pelo context (se existir)
                    var tracked = await _context.Users.FindAsync(lider.Id);
                    if (tracked != null)
                    {
                        lider = tracked;
                    }
                    else
                    {
                        // se não houver instância rastreada, anexamos sem alterar seu estado (opcional)
                        _context.Attach(lider);
                    }
                }
            }

            if (lider != null)
            {
                prospeccao.Usuario = lider;
            }
            else
            {
                // Em caso de usuário não encontrado, evitar atribuir um objeto estranho
                prospeccao.Usuario = null;
            }

            // Se não encontrou no contexto, tentar fallback para cache (sem causar tracking conflict)
            if (lider == null)
            {
                var usuariosCache = await _cache.GetCachedAsync("AllUsuarios", () => _context.Users.ToListAsync());
                lider = usuariosCache.FirstOrDefault(p => p.Id == prospeccao.Usuario?.Id);
                if (lider != null)
                {
                    // garantir que usamos a instância rastreada pelo context (se existir)
                    var tracked = await _context.Users.FindAsync(lider.Id);
                    if (tracked != null)
                    {
                        lider = tracked;
                    }
                    else
                    {
                        // se não houver instância rastreada, anexamos sem alterar seu estado (opcional)
                        _context.Attach(lider);
                    }
                }
            }

            if (lider != null)
            {
                prospeccao.Usuario = lider;
            }
            else
            {
                // Em caso de usuário não encontrado, evitar atribuir um objeto estranho
                prospeccao.Usuario = null;
            }

            // Se não encontrou no contexto, tentar fallback para cache (sem causar tracking conflict)
            if (lider == null)
            {
                var usuariosCache = await _cache.GetCachedAsync("AllUsuarios", () => _context.Users.ToListAsync());
                lider = usuariosCache.FirstOrDefault(p => p.Id == prospeccao.Usuario?.Id);
                if (lider != null)
                {
                    // garantir que usamos a instância rastreada pelo context (se existir)
                    var tracked = await _context.Users.FindAsync(lider.Id);
                    if (tracked != null)
                    {
                        lider = tracked;
                    }
                    else
                    {
                        // se não houver instância rastreada, anexamos sem alterar seu estado (opcional)
                        _context.Attach(lider);
                    }
                }
            }

            if (lider != null)
            {
                prospeccao.Usuario = lider;
            }
            else
            {
                // Em caso de usuário não encontrado, evitar atribuir um objeto estranho
                prospeccao.Usuario = null;
            }

            // Se não encontrou no contexto, tentar fallback para cache (sem causar tracking conflict)
            if (lider == null)
            {
                var usuariosCache = await _cache.GetCachedAsync("AllUsuarios", () => _context.Users.ToListAsync());
                lider = usuariosCache.FirstOrDefault(p => p.Id == prospeccao.Usuario?.Id);
                if (lider != null)
                {
                    // garantir que usamos a instância rastreada pelo context (se existir)
                    var tracked = await _context.Users.FindAsync(lider.Id);
                    if (tracked != null)
                    {
                        lider = tracked;
                    }
                    else
                    {
                        // se não houver instância rastreada, anexamos sem alterar seu estado (opcional)
                        _context.Attach(lider);
                    }
                }
            }

            if (lider != null)
            {
                prospeccao.Usuario = lider;
            }
            else
            {
                // Em caso de usuário não encontrado, evitar atribuir um objeto estranho
                prospeccao.Usuario = null;
            }

            // Se não encontrou no contexto, tentar fallback para cache (sem causar tracking conflict)
            if (lider == null)
            {
                var usuariosCache = await _cache.GetCachedAsync("AllUsuarios", () => _context.Users.ToListAsync());
                lider = usuariosCache.FirstOrDefault(p => p.Id == prospeccao.Usuario?.Id);
                if (lider != null)
                {
                    // garantir que usamos a instância rastreada pelo context (se existir)
                    var tracked = await _context.Users.FindAsync(lider.Id);
                    if (tracked != null)
                    {
                        lider = tracked;
                    }
                    else
                    {
                        // se não houver instância rastreada, anexamos sem alterar seu estado (opcional)
                        _context.Attach(lider);
                    }
                }
            }

            if (lider != null)
            {
                prospeccao.Usuario = lider;
            }
            else
            {
                // Em caso de usuário não encontrado, evitar atribuir um objeto estranho
                prospeccao.Usuario = null;
            }

            // Se não encontrou no contexto, tentar fallback para cache (sem causar tracking conflict)
            if (lider == null)
            {
                var usuariosCache = await _cache.GetCachedAsync("AllUsuarios", () => _context.Users.ToListAsync());
                lider = usuariosCache.FirstOrDefault(p => p.Id == prospeccao.Usuario?.Id);
                if (lider != null)
                {
                    // garantir que usamos a instância rastreada pelo context (se existir)
                    var tracked = await _context.Users.FindAsync(lider.Id);
                    if (tracked != null)
                    {
                        lider = tracked;
                    }
                    else
                    {
                        // se não houver instância rastreada, anexamos sem alterar seu estado (opcional)
                        _context.Attach(lider);
                    }
                }
            }

            if (lider != null)
            {
                prospeccao.Usuario = lider;
            }
            else
            {
                // Em caso de usuário não encontrado, evitar atribuir um objeto estranho
                prospeccao.Usuario = null;
            }
            }

            Prospeccao prospAntiga = await _context.Prospeccao.AsNoTracking().FirstAsync(p => p.Id == prospeccao.Id);
            if (!FunilHelpers.UsuarioPodeAcessarCasa(UsuarioAtivo, prospAntiga.Casa))
            {
                throw new UnauthorizedAccessException();
            }

            prospeccao.Casa = prospAntiga.Casa;
            prospeccao.ProspeccaoPrincipalId = prospAntiga.ProspeccaoPrincipalId;

            // tudo abaixo compara a versão antiga com a nova que irá para o Update()

            if (prospAntiga.Ancora == true && prospeccao.Ancora == false)
            { // verifica se a âncora foi cancelada
                FunilHelpers.RepassarStatusAoCancelarAncora(_context, prospeccao);
            }
            else if (prospeccao.Ancora == true && string.IsNullOrEmpty(prospeccao.Agregadas))
            { // verifica se o campo agg está vazio
                throw new InvalidOperationException("Nao e possivel adicionar uma Ancora sem nenhuma agregada.");
            }
            else if (prospAntiga.Agregadas != prospeccao.Agregadas)
            { // verifica se alguma agregada foi alterada
                FunilHelpers.AddAgregadas(_context, prospAntiga, prospeccao);
                FunilHelpers.DelAgregadas(_context, prospAntiga, prospeccao);
            }

            _context.Update(prospeccao);
            return prospeccao;
        }

        private async Task ValidarAssociacaoProspeccao(Prospeccao prospeccao, string tipoAssociacaoProspecao)
        {
            string tipoAssociacaoNormalizado = string.IsNullOrWhiteSpace(tipoAssociacaoProspecao)
                ? "nova"
                : tipoAssociacaoProspecao.Trim().ToLowerInvariant();

            if (tipoAssociacaoNormalizado == "nova")
            {
                prospeccao.ProspeccaoPrincipalId = null;
                return;
            }

            if (string.IsNullOrWhiteSpace(prospeccao.ProspeccaoPrincipalId))
            {
                ModelState.AddModelError(nameof(Prospeccao.ProspeccaoPrincipalId), "Selecione a prospecção principal.");
                return;
            }

            var prospeccaoPrincipal = await _context.Prospeccao
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == prospeccao.ProspeccaoPrincipalId);

            if (prospeccaoPrincipal == null)
            {
                ModelState.AddModelError(nameof(Prospeccao.ProspeccaoPrincipalId), "A prospecção principal selecionada não foi encontrada.");
                return;
            }

            bool mesmaCasa = prospeccaoPrincipal.Casa == prospeccao.Casa;
            if (tipoAssociacaoNormalizado == "mesma-casa" && !mesmaCasa)
            {
                ModelState.AddModelError(nameof(Prospeccao.ProspeccaoPrincipalId), "Selecione uma prospecção principal da mesma casa.");
                return;
            }

            if (tipoAssociacaoNormalizado == "outra-casa" && mesmaCasa)
            {
                ModelState.AddModelError(nameof(Prospeccao.ProspeccaoPrincipalId), "Selecione uma prospecção principal de outra casa.");
            }
        }

        private async Task InserirDadosEmpresasUsuariosViewData()
        {
            var empresas = await _cache.GetCachedAsync("Empresas:Funil", () => _context.Empresa.Select(e => new EmpresasFunilDTO
            {
                Id = e.Id,
                Nome = e.Nome
            }).ToListAsync());

            var usuarios = await _cache.GetCachedAsync("Usuarios:Funil", () => _context.Users.Select(u => new UsuariosFunilDTO
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToListAsync());

            ViewData["Usuarios"] = usuarios;
            ViewData["Empresas"] = new SelectList(empresas, "Id", "Nome");
            ViewData["Equipe"] = new SelectList(usuarios, "Id", "UserName");
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
        private async Task<List<Prospeccao>> ObterProspeccoesFunilFiltradas(string casa, string ano, Usuario usuario, string aba, string sortOrder, string searchString, string temperatura, string fomento)
        {
            int? tamanhoPagina = HttpContext.Session.GetInt32("tamanhoPagina");

            IQueryable<Prospeccao> query =
                FunilHelpers.DefinirCasaParaVisualizarQuery(
                    casa,
                    usuario,
                    _context,
                    HttpContext,
                    ViewData
                );

            query = query.Where(p => p.Empresa != null);

            if (!string.IsNullOrEmpty(ano) && !ano.Equals("Todos", StringComparison.OrdinalIgnoreCase))
            {
                query = FunilHelpers.PeriodizarProspecçõesQuery(query, ano);
            }

            query = FunilHelpers.FiltrarProspecçõesQuery(query, searchString);
            query = FunilHelpers.OrdenarProspecçõesQuery(query, sortOrder);

            if (!string.IsNullOrWhiteSpace(fomento) && Enum.TryParse(fomento, out TipoContratacao tipoContratacao))
            {
                query = query.Where(p => p.TipoContratacao == tipoContratacao);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                FunilHelpers.SetarFiltrosNaView(HttpContext, ViewData, sortOrder, searchString);
            }

            if (!string.IsNullOrEmpty(aba))
            {
                var parametros = new ParametrosFiltroFunil(casa, usuario, _cache, aba, HttpContext);
                query = FunilHelpers.FiltrarPorStatusQuery(
                    query,
                    parametros,
                    !string.IsNullOrEmpty(searchString)
                );
            }

            List<Prospeccao> prospeccoes = await query.ToListAsync();

            if (!string.IsNullOrWhiteSpace(temperatura) && !temperatura.Equals("Todos", StringComparison.OrdinalIgnoreCase))
            {
                prospeccoes = prospeccoes
                    .Where(prospeccao => ProspeccaoTemTemperatura(prospeccao, temperatura))
                    .ToList();
            }

            return prospeccoes;

        }
        private async Task<List<Prospeccao>> EnriquecerProspeccoesFunilAsync(List<Prospeccao> prospeccoes)
        {
            if (prospeccoes == null || !prospeccoes.Any())
            {
                return prospeccoes ?? new List<Prospeccao>();
            }

            List<string> ids = prospeccoes
                .Where(prospeccao => prospeccao != null && !string.IsNullOrWhiteSpace(prospeccao.Id))
                .Select(prospeccao => prospeccao.Id)
                .Distinct()
                .ToList();

            if (!ids.Any())
            {
                return prospeccoes;
            }

            List<ProspeccaoValorComposicao> composicoes = await _context.Set<ProspeccaoValorComposicao>()
                .AsNoTracking()
                .Where(valor => ids.Contains(valor.ProspeccaoId))
                .ToListAsync();

            List<Prospeccao> dependentes = await _context.Prospeccao
                .AsNoTracking()
                .Include(prospeccao => prospeccao.Empresa)
                .Where(prospeccao => prospeccao.ProspeccaoPrincipalId != null && ids.Contains(prospeccao.ProspeccaoPrincipalId))
                .ToListAsync();

            var composicoesPorProspeccao = composicoes
                .GroupBy(valor => valor.ProspeccaoId)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.ToList());

            var dependentesPorProspeccao = dependentes
                .GroupBy(prospeccao => prospeccao.ProspeccaoPrincipalId)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.Select(MaterializarProspeccaoFunil).ToList());

            return prospeccoes
                .Where(prospeccao => prospeccao != null)
                .Select(prospeccao =>
                {
                    var prospeccaoMaterializada = MaterializarProspeccaoFunil(prospeccao);
                    prospeccaoMaterializada.ComposicaoValores = composicoesPorProspeccao.ContainsKey(prospeccao.Id)
                        ? composicoesPorProspeccao[prospeccao.Id]
                        : new List<ProspeccaoValorComposicao>();
                    prospeccaoMaterializada.ProspeccoesRelacionadas = dependentesPorProspeccao.ContainsKey(prospeccao.Id)
                        ? dependentesPorProspeccao[prospeccao.Id]
                        : new List<Prospeccao>();
                    return prospeccaoMaterializada;
                })
                .ToList();
        }

        private static Prospeccao MaterializarProspeccaoFunil(Prospeccao prospeccao)
        {
            if (prospeccao == null)
            {
                return null;
            }

            return new Prospeccao
            {
                Id = prospeccao.Id,
                NomeProspeccao = prospeccao.NomeProspeccao,
                Tipologia = prospeccao.Tipologia,
                TipoDeInteracao = prospeccao.TipoDeInteracao,
                ParceiroInterno = prospeccao.ParceiroInterno,
                PotenciaisParceiros = prospeccao.PotenciaisParceiros,
                TipoDeProjeto = prospeccao.TipoDeProjeto,
                PrevisaoTempoProjetoMeses = prospeccao.PrevisaoTempoProjetoMeses,
                LinkArquivo = prospeccao.LinkArquivo,
                Empresa = prospeccao.Empresa,
                EmpresaId = prospeccao.EmpresaId,
                Contato = prospeccao.Contato,
                Usuario = prospeccao.Usuario,
                LiderNome = prospeccao.LiderNome,
                MembrosEquipe = prospeccao.MembrosEquipe,
                TipoContratacao = prospeccao.TipoContratacao,
                LinhaPequisa = prospeccao.LinhaPequisa,
                Status = prospeccao.Status != null ? prospeccao.Status.ToList() : new List<FollowUp>(),
                Casa = prospeccao.Casa,
                ValorProposta = prospeccao.ValorProposta,
                ValorEstimado = prospeccao.ValorEstimado,
                ValorFinal = prospeccao.ValorFinal,
                ProspeccaoPrincipal = prospeccao.ProspeccaoPrincipal != null ? new Prospeccao
                {
                    Id = prospeccao.ProspeccaoPrincipal.Id,
                    NomeProspeccao = prospeccao.ProspeccaoPrincipal.NomeProspeccao,
                    Casa = prospeccao.ProspeccaoPrincipal.Casa,
                    Empresa = prospeccao.ProspeccaoPrincipal.Empresa,
                    EmpresaId = prospeccao.ProspeccaoPrincipal.EmpresaId
                } : null,
                ProspeccaoPrincipalId = prospeccao.ProspeccaoPrincipalId,
                CaminhoPasta = prospeccao.CaminhoPasta,
                Tags = prospeccao.Tags,
                Origem = prospeccao.Origem,
                Ancora = prospeccao.Ancora,
                Agregadas = prospeccao.Agregadas,
                ComposicaoValores = new List<ProspeccaoValorComposicao>(),
                ProspeccoesRelacionadas = new List<Prospeccao>()
            };
        }

        private static bool ProspeccaoTemTemperatura(Prospeccao prospeccao, string temperatura)
        {
            FollowUp ultimoStatus = prospeccao.Status?
                .OrderBy(followup => followup.Data)
                .ThenBy(followup => followup.Id)
                .LastOrDefault();

            if (ultimoStatus == null || ultimoStatus.Status == StatusProspeccao.Planejada)
            {
                return false;
            }

            int qtdDias = DateTime.Now.Subtract(ultimoStatus.Data).Days;
            switch (temperatura.ToLowerInvariant())
            {
                case "quente":
                    return qtdDias < 7;
                case "morno":
                    return qtdDias >= 7 && qtdDias <= 15;
                case "esfriando":
                    return qtdDias >= 16 && qtdDias <= 30;
                case "frio":
                    return qtdDias > 30 && qtdDias <= 365;
                case "congelado":
                    return qtdDias > 365;
                default:
                    return true;
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
        /// Obtém todas as prospecções (exceto planejadas)
        /// </summary>
        /// <returns></returns>
        private async Task<List<Prospeccao>> ObterProspeccoesTotais(Instituto casa)
        {
            return await _cache.GetCachedAsync($"Prospeccoes:{casa}", () => _context.Prospeccao
                .Include(p => p.Status)
                .Include(p => p.Empresa)
                .Where(p => p.Status.Any() &&
                    p.Status.OrderBy(f => f.Data).Last().Status != StatusProspeccao.Planejada
                    && p.Casa == casa)
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

        [HttpGet]
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> ExportarExcel(string casa)
        {
            if (!FunilHelpers.TentarParseInstituto(casa, out Instituto enumCasa))
                return BadRequest("Casa inválida");

            var prospeccoes = await _context.Prospeccao
                .Include(p => p.Empresa)
                .Include(p => p.Usuario)
                .Include(p => p.Status)
                .Where(p => p.Casa == enumCasa)
                .ToListAsync();

            var dados = prospeccoes.Select(p => new
            {
                Empresa = p.Empresa?.Nome ?? "",

                Razao = p.Empresa.RazaoSocial ?? "",

                CNPJ = p.Empresa.CNPJ ?? "",
                //AJUSTAR QUANDO TIVER MAIS TEMPO
                TipoContratacao = p.TipoContratacao switch
                {
                    TipoContratacao.ContratacaoDireta => "Contratação Direta",
                    TipoContratacao.Embrapii => "Embrapii",
                    TipoContratacao.EditalInovacao => "Edital SESI-SENAI",
                    TipoContratacao.AgenciaFomento => "ANEEL",
                    TipoContratacao.ANP => "ANP",
                    TipoContratacao.Parceiro => "Finep",
                    TipoContratacao.Push => "Embrapii + ANP",
                    TipoContratacao.EmbrapiiANEEL => "Embrapii + ANEEL",
                    TipoContratacao.EditalOutros => "Edital - outros",
                    TipoContratacao.Indefinida => "A definir",
                    _ => p.TipoContratacao.ToString() 
                },

                TipoProjeto = p.TipoDeProjeto switch
                {
                    TipoDeProjeto.PDeI => "PD&I",
                    TipoDeProjeto.Servico_tecnologico => "Serviço tecnológico",
                    TipoDeProjeto.Adefinir => "A definir",
                    0 => "Não informado",
                    _ => p.TipoDeProjeto.ToString() ?? "Não informado"
                },

                PrevisaoTempoProjetoMeses = p.PrevisaoTempoProjetoMeses,

                LinkArquivo = p.LinkArquivo,

                Iniciativa = p.Origem switch
                {   Origem.Unidade => "Unidade",
                    Origem.ICTParceira => "ICT parceira",
                    Origem.EmpresaCliente => "Empresa / Cliente",
                    Origem.CatalisaSebrae => "Catalisa SEBRAE",
                    Origem.ProgramaProspectores => "Programa Prospectores",
                    Origem.Adefinir => "A definir",
                    _ => p.Origem.ToString() 
                 },

                TipoDeInteracao = p.TipoDeInteracao switch
                {
                    TipoDeInteracao.VisitaEmpresa => "Visita à empresa",
                    TipoDeInteracao.AtendimentoUnidade => "Atendimento na unidade",
                    TipoDeInteracao.TelefoneOuTeleconferencia => "Telefone ou teleconferência",
                    TipoDeInteracao.ReuniaoEventoProspeccao => "Reunião em evento de prospecção",
                    TipoDeInteracao.Outro => "Outros",
                    TipoDeInteracao.Adefinir => "A definir",
                    0 => "Não informado",
                    _ => p.TipoDeInteracao.ToString() ?? "Não informado"
                },

                Segmento = p.Empresa != null
                    ? p.Empresa.Segmento.GetDisplayName()
                    : "",

                Estado = p.Empresa != null
                    ? p.Empresa.Estado.GetDisplayName()
                    : "",

                Responsavel = p.Contato != null
                    ? $"{p.Contato.Nome} | {p.Contato.Cargo} | {p.Contato.Telefone} | {p.Contato.Email}"
                    : "Não informado",

                AlocadoPara = !string.IsNullOrWhiteSpace(p.LiderNome) ? p.LiderNome : p.Usuario?.UserName ?? "",

                Apoio = string.Join(", ",
                    (p.MembrosEquipe ?? "")
                        .Split(";")
                        .Where(m => !string.IsNullOrWhiteSpace(m))
                        .Select(m => m.Trim())
                ),

                Porte = p.Empresa.Porte,

                Tipologia = p.Tipologia ?? "",

                IdProspecaoSGI = p.Id,

                Tema = p.NomeProspeccao ?? "",

                LinhaPesquisa = p.LinhaPequisa.GetDisplayName(),

                ParceiroInterno = p.ParceiroInterno switch 
                {
                    ParceiroInterno.ISI_II => "ISI-II",
                    ParceiroInterno.ISI_BF => "ISI-BF",
                    ParceiroInterno.ISI_SVP => "ISI-SVP",
                    ParceiroInterno.CIS_SO => "CIS-SO",
                    ParceiroInterno.IST_EDI => "IST-EDI",
                    ParceiroInterno.IST_QMA => "IST-QMA",
                    ParceiroInterno.ISI_QV => "ISI-QV",
                    ParceiroInterno.Adefinir => "A definir",
                    ParceiroInterno.NaoHa => "Não há",
                    0 => "Não informado",
                    _ => p.ParceiroInterno.ToString() ?? "Não informado"
                },       

                ParceiroExterno = p.PotenciaisParceiros,

                ContatoRealizado = p.Status?
                 .Where(s => s.Status == StatusProspeccao.ContatoInicial)
                 .OrderBy(s => s.Data)
                 .Select(s => (DateTime?)s.Data)
                 .FirstOrDefault(),

                NDAAssinado = p.Status?
                 .Where(s => s.Status == StatusProspeccao.NDAAssinado)
                 .OrderBy(s => s.Data)
                 .Select(s => (DateTime?)s.Data)
                 .FirstOrDefault(),

                PropostaEnviada = p.Status?
                 .Where(s => s.Status == StatusProspeccao.ComProposta)
                 .OrderBy(s => s.Data)
                 .Select(s => (DateTime?)s.Data)
                 .FirstOrDefault(),

                ProjetoConvertido = p.Status?
                 .Where(s => s.Status == StatusProspeccao.Convertida)
                 .OrderBy(s => s.Data)
                 .Select(s => (DateTime?)s.Data)
                 .FirstOrDefault(),

                ValorEstimado = p.ValorEstimado,

                ValorProposta = p.ValorProposta,

                HistoricoContatos = p.Status != null
                    ? string.Join(" | ",
                        p.Status
                            .OrderBy(s => s.Data)
                            .Select(s =>
                                $"{s.Data:dd/MM/yyyy} - {s.Status}"))
                    : "",

                Situacao = p.Status != null && p.Status.Any()
                    ? p.Status
                        .OrderBy(s => s.Data)
                        .Last()
                        .Status
                        .ToString()
                    : ""
            })
            .ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Funil de Vendas");

            var table = worksheet.Cell(1, 1).InsertTable(dados);

            table.Field("Empresa").Name = "Empresa";
            table.Field("Razao").Name = "Razão Social";
            table.Field("CNPJ").Name = "CNPJ";
            table.Field("Segmento").Name = "Segmento";
            table.Field("Estado").Name = "Estado";
            table.Field("Iniciativa").Name = "Iniciativa";
            table.Field("Responsavel").Name = "Nome, cargo e contato na empresa";
            table.Field("TipoDeInteracao").Name = "Tipo de interação";
            table.Field("AlocadoPara").Name = "Responsável pela prospecção";
            table.Field("Apoio").Name = "Apoio";
            table.Field("Porte").Name = "Porte";
            table.Field("Tipologia").Name = "Tipologia (ABCD)";
            table.Field("IdProspecaoSGI").Name = "Id da Prospecção no SGI";
            table.Field("Tema").Name = "Tema";
            table.Field("LinhaPesquisa").Name = "Linha de Pesquisa";
            table.Field("TipoContratacao").Name = "Tipo de contratação";
            table.Field("TipoProjeto").Name = "Tipo de Projeto";
            table.Field("PrevisaoTempoProjetoMeses").Name = "Previsão de tempo do projeto (meses)";
            table.Field("LinkArquivo").Name = "Link para pasta ou documento do projeto";
            table.Field("ParceiroInterno").Name = "Parceiro Interno";
            table.Field("ParceiroExterno").Name = "Parceiro Externo";
            table.Field("ContatoRealizado").Name = "Contato Realizado";
            table.Field("NDAAssinado").Name = "NDA Assinado";
            table.Field("PropostaEnviada").Name = "Proposta Enviada";
            table.Field("ProjetoConvertido").Name = "Projeto Convertido";
            table.Field("ValorEstimado").Name = "Valor da Estimado";
            table.Field("ValorProposta").Name = "Valor da Proposta";
            table.Field("HistoricoContatos").Name = "Histórico de contatos";
            table.Field("Situacao").Name = "Situação";

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"funil_vendas_{casa}_{DateTime.Now:yyyyMMdd}.xlsx"
            );
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
        private async Task<IActionResult> RemoverFollowupAutenticado(FollowUp followup)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);
                
                ParametrosFunil parametrosFunil = new ParametrosFunil();
                parametrosFunil.ObterParametrosSession(HttpContext.Session);

                var prospeccoes = await _cache.GetCachedAsync("AllProspeccoes", () => _context.Prospeccao.ToListAsync());
                Prospeccao prospeccao = prospeccoes.Find(p => p.Id == followup.OrigemID);

                // Obter o usuário dono a partir da prospecção (evita depender de followup.Origem que pode não estar carregado)
                Usuario donoProsp = prospeccao?.Usuario;

                //Verifica se o usuário está apto para remover o followup
                if (VerificarCondicoesRemocao(prospeccao, UsuarioAtivo, donoProsp) || UsuarioAtivo?.Nivel == Nivel.Dev)
                {
                    _context.FollowUp.Remove(followup);
                    await _context.SaveChangesAsync();
                    await CacheHelper.CleanupProspeccoesCache(_cache);

                    return RedirectToAction("Index",
                                            "FunilDeVendas",
                                            new
                                            {
                                                casa = prospeccao.Casa,
                                                aba = parametrosFunil.Aba,
                                                tamanhoPagina = parametrosFunil.TamanhoPagina,
                                                sortOrder = parametrosFunil.SortOrder,
                                                searchString = parametrosFunil.SearchString
                                            });
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
    }
}






