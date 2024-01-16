using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("TestesBaseDeProjetos1")]

namespace BaseDeProjetos.Controllers
{
    public class ParticipacaoController : SGIController
    {
        private const string nomeCargoBolsista = "Pesquisador Bolsista";
        private const string nomeCargoEstagiário = "Estagiário";

        // TODO: Precisamos não utilizar esses valores mágicos de string no futuro!!
        private const string nomeCargoPesquisador = "Pesquisador QMS";

        private static readonly Dictionary<int, int> pesquisadores = new Dictionary<int, int>();
        private static Dictionary<int, decimal> despesas = new Dictionary<int, decimal>();
        private readonly DbCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ParticipacaoController> _logger;
        private List<Prospeccao> _prospeccoes = new List<Prospeccao>();

        public ParticipacaoController(ApplicationDbContext context, DbCache cache, ILogger<ParticipacaoController> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? dataInicio, DateTime? dataFim)
        {
            ViewbagizarUsuario(_context, _cache);

            dataInicio ??= new DateTime(2021, 01, 01);
            dataFim ??= new DateTime(DateTime.Now.Year, 12, 31);
            ViewData["dataInicio"] = dataInicio.Value.ToString("yyyy-MM-dd");
            ViewData["dataFim"] = dataFim.Value.ToString("yyyy-MM-dd");

            List<IndicadoresFinanceirosDTO> indicadores = await ObterIndicadoresFinanceirosParaParticipacao();

            foreach (var indicador in indicadores)
            {
                despesas[indicador.Data.Year] = indicador.Despesa;
                pesquisadores[indicador.Data.Year] = indicador.QtdPesquisadores;
            }

            return View();
        }

        /// <summary>
        /// Retorna os dados de indicadores de acordo com o seu nome e datas de filtragem
        /// </summary>
        /// <param name="nomeIndicador"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IActionResult> RetornarDadosIndicador(string nomeIndicador, DateTime? dataInicio, DateTime? dataFim)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                dataInicio ??= new DateTime(2021, 01, 01);
                dataFim ??= new DateTime(DateTime.Now.Year, 12, 31);

                List<IndicadoresFinanceirosDTO> indicadores = await ObterIndicadoresFinanceirosParaParticipacao();

                foreach (var indicador in indicadores)
                {
                    despesas[indicador.Data.Year] = indicador.Despesa;
                    pesquisadores[indicador.Data.Year] = indicador.QtdPesquisadores;
                }

                if (_prospeccoes.Count == 0)
                {
                    _prospeccoes = await ObterProspeccoesParaParticipacao();
                }

                if (string.IsNullOrEmpty(nomeIndicador))
                {
                    throw new Exception("O nome do indicador jamais pode estar vazio");
                }

                string chaveCache = $"Participacoes:{dataInicio.Value.Month}:{dataInicio.Value.Year}:{dataFim.Value.Month}:{dataFim.Value.Year}";
                List<ParticipacaoTotalViewModel> participacoes = await _cache.GetCachedAsync(chaveCache, () => GetParticipacoesTotaisUsuarios((DateTime)dataInicio, (DateTime)dataFim));
                List<IndicadorResultadoViewModel> resultados = new List<IndicadorResultadoViewModel>();

                RankearParticipacoes(participacoes, false);

                foreach (var participacao in participacoes)
                {
                    try
                    {
                        var valor = typeof(ParticipacaoTotalViewModel).GetProperty(nomeIndicador).GetValue(participacao, null);
                        decimal rank = ObterRankingParticipacao(participacao, nomeIndicador);
                        if (valor != null)
                        {
                            if (UsuarioAtivo.Nivel == Nivel.Usuario || UsuarioAtivo.Nivel == Nivel.Externos)
                            {
                                if (participacao.Lider.Id == UsuarioAtivo.Id)
                                {
                                    resultados.Add(new IndicadorResultadoViewModel
                                    {
                                        Pesquisador = participacao.Lider.ToPesquisadorViewModel(),
                                        Valor = valor,
                                        Rank = rank
                                    });
                                }
                            }
                            else
                            {
                                resultados.Add(new IndicadorResultadoViewModel
                                {
                                    Pesquisador = participacao.Lider.ToPesquisadorViewModel(),
                                    Valor = valor,
                                    Rank = rank
                                });
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return Ok(JsonConvert.SerializeObject(resultados));
                    }
                }

                return Ok(JsonConvert.SerializeObject(resultados));
            }
            else
            {
                return View("Forbidden");
            }
        }

        private async Task<List<IndicadoresFinanceirosDTO>> ObterIndicadoresFinanceirosParaParticipacao()
        {
            return await _cache.GetCachedAsync("IndicadoresFinanceiros:Participacao", () => _context.IndicadoresFinanceiros.Select(i => new IndicadoresFinanceirosDTO { Despesa = i.Despesa, QtdPesquisadores = i.QtdPesquisadores, Data = i.Data }).ToListAsync());
        }

        /// <summary>
        /// Obtém o rank de uma participacao de acordo com o nome do indicador
        /// </summary>
        /// <param name="participacao"></param>
        /// <param name="nomeIndicador"></param>
        /// <returns></returns>
        private decimal ObterRankingParticipacao(ParticipacaoTotalViewModel participacao, string nomeIndicador)
        {
            if (participacao.RankPorIndicador != null)
            {
                PropertyInfo[] propriedadesParticipacao = participacao.RankPorIndicador.GetType().GetProperties();
                foreach (var property in propriedadesParticipacao)
                {
                    if (property.Name.Contains(nomeIndicador))
                    {
                        if (property.PropertyType == typeof(decimal))
                        {
                            return (decimal)property.GetValue(participacao.RankPorIndicador);
                        }
                    }
                }
                throw new Exception("Nenhuma propriedade foi encontrada.");
            }
            else
            {
                throw new ArgumentNullException("O rank por indicador está nulo.");
            }
        }

        /// <summary>
        /// Retorna os dados do gráfico temporal considerando o id do usuário passado por parâmetro
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("Participacao/RetornarDadosGraficoTemporal/{idUsuario}")]
        [HttpGet("Participacao/RetornarDadosPesquisador/{idUsuario}")]
        public async Task<IActionResult> RetornarDadosGraficoTemporal(string idUsuario, DateTime? dataInicio, DateTime? dataFim)
        {
            ViewbagizarUsuario(_context, _cache);

            List<IndicadoresFinanceirosDTO> indicadores = await ObterIndicadoresFinanceirosParaParticipacao();

            foreach (var indicador in indicadores)
            {
                despesas[indicador.Data.Year] = indicador.Despesa;
                pesquisadores[indicador.Data.Year] = indicador.QtdPesquisadores;
            }

            if (!dataInicio.HasValue)
            {
                dataInicio = new DateTime(2021, 01, 01);
            }

            if (!dataFim.HasValue)
            {
                dataFim = new DateTime(DateTime.Now.Year, 12, 31);
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _prospeccoes = await ObterProspeccoesParaParticipacao();

                var participacoes = await GetParticipacoesTotaisUsuarios((DateTime)dataInicio, (DateTime)dataFim);

                var participacao = participacoes.Where(p => p.Lider.Id == idUsuario).First();

                ObterRankingMedioPesquisador(participacao, participacoes);

                if (participacao != null)
                {
                    return Ok(JsonConvert.SerializeObject(participacao));
                }
                else
                {
                    return Ok(null);
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        private async Task<List<Prospeccao>> ObterProspeccoesParaParticipacao()
        {
            return await _cache.GetCachedAsync("Prospeccoes:Participacao", () => _context.Prospeccao.Include(p => p.Status).Include(p => p.Usuario).Include(p => p.Empresa).ToListAsync());
        }

        /// <summary>
        /// Atribui as propriedades de RankMedio de acordo com as participacoes (incluindo a do próprio usuario)
        /// </summary>
        /// <param name="participacao"></param>
        /// <param name="participacoes"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void ObterRankingMedioPesquisador(ParticipacaoTotalViewModel participacao, List<ParticipacaoTotalViewModel> participacoes)
        {
            if (participacao == null || participacoes == null)
            {
                throw new ArgumentNullException(nameof(participacao) + " não pode estar nulo");
            }

            var tipoValor = typeof(ParticipacaoTotalViewModel);
            var tipoRankSobreMedia = typeof(RankParticipacao);

            foreach (var propriedadeRank in tipoRankSobreMedia.GetProperties())
            {
                string nomePropriedadeBase = propriedadeRank.Name.Substring("Rank".Length);
                var propriedadeBase = tipoValor.GetProperty(nomePropriedadeBase);

                if (propriedadeBase == null)
                {
                    throw new InvalidOperationException($"A propriedade base {nomePropriedadeBase} não foi encontrada para {propriedadeRank.Name}");
                }

                decimal valor;
                decimal valorMedio;

                if (propriedadeBase.PropertyType == typeof(int))
                {
                    int valorInt = (int)propriedadeBase.GetValue(participacao);
                    double valorMedioDouble = participacoes.Average(p => (int)propriedadeBase.GetValue(p));
                    valor = valorInt;
                    valorMedio = (decimal)valorMedioDouble;
                }
                else
                {
                    valor = (decimal)propriedadeBase.GetValue(participacao);
                    valorMedio = (decimal)participacoes.Average(p => Convert.ToDouble(propriedadeBase.GetValue(p)));
                }

                participacao.RankSobreMedia ??= new RankParticipacao();

                var instancia = participacao.RankSobreMedia;

                propriedadeRank.SetValue(instancia, IndicadorHelper.DivisaoSegura(valor, valorMedio));
            }
        }

        /// <summary>
        /// Retorna uma lista de projetos com o seu valor ajustado para o custo de acordo com o filtro
        /// </summary>
        /// <param name="mesInicio">Mês de inicio do filtro</param>
        /// <param name="anoInicio">Ano de inicio do filtro</param>
        /// <param name="mesFim">Mês de fim do filtro</param>
        /// <param name="anoFim">Ano de fim do filtro</param>
        /// <param name="projetos">Lista de projetos a serem ajustados</param>
        /// <returns></returns>
        internal List<Projeto> AcertarPrecificacaoProjetos(DateTime dataInicio, DateTime dataFim, List<Projeto> projetos)
        {
            foreach (var projeto in projetos)
            {
                if (dataInicio > projeto.DataEncerramento || dataFim < projeto.DataInicio)
                {
                    projeto.ValorTotalProjeto = 0;
                }
                else
                {
                    if (dataInicio < projeto.DataEncerramento)
                    {
                        // TODO: E se o inicio do filtro for maior que o começo do projeto?
                        if (projeto.DataInicio < dataInicio)
                        {
                            ReatribuirValorProjeto(projeto, dataFim, dataInicio);
                        }
                        else
                        {
                            ReatribuirValorProjeto(projeto, dataFim);
                        }
                    }
                    else
                    {
                        ReatribuirValorProjeto(dataInicio, projeto);
                    }
                }
            }

            return projetos;
        }



        /// <summary>
        /// Reatribui os valores de um projeto de acordo com uma data final de filtro
        /// </summary>
        /// <param name="projeto"></param>
        /// <param name="dataFinalFiltro"></param>
        internal void ReatribuirValorProjeto(Projeto projeto, DateTime dataFinalFiltro, DateTime? dataInicialFiltro = null)
        {
            int qtdMeses;
            if (dataFinalFiltro < projeto.DataInicio)
            {
                throw new ArgumentException($"{nameof(dataFinalFiltro)} não pode ser inferior a {nameof(projeto.DataInicio)}");
            }

            if (dataInicialFiltro != null)
            {
                qtdMeses = Helpers.Helpers.DiferencaMeses(dataFinalFiltro, (DateTime)dataInicialFiltro, true);
            }
            else
            {
                qtdMeses = Helpers.Helpers.DiferencaMeses(dataFinalFiltro, projeto.DataInicio, true);
            }

            int diferencaMeses = Helpers.Helpers.DiferencaMeses(projeto.DataEncerramento, projeto.DataInicio, true);
            double valorProjetoPorMes = projeto.ValorTotalProjeto / diferencaMeses;
            projeto.ValorTotalProjeto = valorProjetoPorMes * qtdMeses;
        }

        /// <summary>
        /// Reatribui os valores de um projeto de acordo com uma data inicial de filtro
        /// </summary>
        /// <param name="dataInicialFiltro"></param>
        /// <param name="projeto"></param>
        internal void ReatribuirValorProjeto(DateTime dataInicialFiltro, Projeto projeto)
        {
            int qtdMeses = Helpers.Helpers.DiferencaMeses(projeto.DataEncerramento, dataInicialFiltro, true);
            int diferencaMeses = Helpers.Helpers.DiferencaMeses(projeto.DataEncerramento, projeto.DataInicio, true);
            double valorProjetoPorMes = projeto.ValorTotalProjeto / diferencaMeses;
            projeto.ValorTotalProjeto = valorProjetoPorMes * qtdMeses;
        }

        /// <summary>
        /// Atribui os rankings as participações passadas por parâmetro, para que sejam exibidas na View. Valores de 0 a 1
        /// </summary>
        /// <param name="participacoes">Lista de participações (normalmente de um usuário específico mas pode ser genérica)</param>
        private static void RankearParticipacoes(List<ParticipacaoTotalViewModel> participacoes, bool rankPorMaximo)
        {
            decimal medValorTotalProsp = participacoes.Average(p => p.ValorTotalProspeccoes);
            decimal medValorMedioProsp = participacoes.Average(p => p.ValorMedioProspeccoes);
            decimal medValorMedioProspComProposta = participacoes.Average(p => p.ValorMedioProspeccoesComProposta);
            decimal medValorMedioProspConvertidas = participacoes.Average(p => p.ValorMedioProspeccoesConvertidas);
            decimal medTotalProspComProposta = participacoes.Average(p => p.ValorTotalProspeccoesComProposta);
            decimal medTotalProspConvertidas = participacoes.Average(p => p.ValorTotalProspeccoesConvertidas);
            decimal medQtdProspeccoes = (decimal)participacoes.Average(p => p.QuantidadeProspeccoes);
            decimal medQtdProspeccoesLider = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesLider);
            decimal medQtdProspeccoesMembro = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesMembro);
            decimal medQtdProspeccoesComProposta = participacoes.Average(p => p.QuantidadeProspeccoesComProposta);
            decimal medQtdProspProjetizadas = participacoes.Average(p => p.QuantidadeProspeccoesConvertidas);
            decimal medConversaoProjeto = participacoes.Average(p => p.TaxaConversaoProjeto);
            decimal medConversaoProposta = participacoes.Average(p => p.TaxaConversaoProposta);
            decimal medFatorContribuicaoFinanceira = participacoes.Average(p => p.FatorContribuicaoFinanceira);

            foreach (var participacao in participacoes)
            {
                CalculosParticipacao.CalcularMediaFatores(participacoes, participacao);

                RankParticipacao rp = new RankParticipacao
                {
                    RankValorTotalProspeccoes = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoes, participacoes.Max(p => p.ValorTotalProspeccoes)) : IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoes, medValorTotalProsp),
                    RankValorMedioProspeccoes = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoes, participacoes.Max(p => p.ValorMedioProspeccoes)) : IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoes, medValorMedioProsp),
                    RankValorTotalProspeccoesComProposta = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesComProposta, participacoes.Max(p => p.ValorTotalProspeccoesComProposta)) : IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesComProposta, medTotalProspComProposta),
                    RankValorTotalProspeccoesConvertidas = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesConvertidas, participacoes.Max(p => p.ValorTotalProspeccoesConvertidas)) : IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesConvertidas, medTotalProspConvertidas),
                    RankValorMedioProspeccoesComProposta = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoesComProposta, participacoes.Max(p => p.ValorMedioProspeccoesComProposta)) : IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoesComProposta, medValorMedioProspComProposta),
                    RankValorMedioProspeccoesConvertidas = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoesConvertidas, participacoes.Max(p => p.ValorMedioProspeccoesConvertidas)) : IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoesConvertidas, medValorMedioProspConvertidas),
                    RankQuantidadeProspeccoes = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoes, participacoes.Max(p => p.QuantidadeProspeccoes)) : IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoes, medQtdProspeccoes),
                    RankQuantidadeProspeccoesLider = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesLider, participacoes.Max(p => p.QuantidadeProspeccoes)) : IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesLider, medQtdProspeccoes),
                    RankQuantidadeProspeccoesConvertidas = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesConvertidas, participacoes.Max(p => p.QuantidadeProspeccoesConvertidas)) : IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesConvertidas, medQtdProspProjetizadas),
                    RankQuantidadeProspeccoesComProposta = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesComProposta, participacoes.Max(p => p.QuantidadeProspeccoesComProposta)) : IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesComProposta, medQtdProspeccoesComProposta),
                    RankQuantidadeProspeccoesMembro = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesMembro, participacoes.Max(p => p.QuantidadeProspeccoesMembro)) : IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesMembro, medQtdProspeccoesMembro),
                    RankAssertividadePrecificacao = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.AssertividadePrecificacao, participacoes.Max(p => p.AssertividadePrecificacao)) : IndicadorHelper.DivisaoSegura(participacao.AssertividadePrecificacao, participacoes.Average(p => p.AssertividadePrecificacao)),
                    RankFatorContribuicaoFinanceira = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.FatorContribuicaoFinanceira, participacoes.Max(p => p.FatorContribuicaoFinanceira)) : IndicadorHelper.DivisaoSegura(participacao.FatorContribuicaoFinanceira, participacoes.Average(p => p.FatorContribuicaoFinanceira))
                };

                participacao.RankPorIndicador = rp;
            }
        }

        /// <summary>
        /// Atribui os valores relativos a assertividade na precificação dado um usuario, sua participacao e prospeccoes
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        /// <param name="participacao"></param>
        /// <param name="prospeccoesUsuario"></param>
        /// <returns></returns>
        private async Task AtribuirAssertividadePrecificacao(Usuario usuario, DateTime dataInicio, DateTime dataFim, ParticipacaoTotalViewModel participacao, ProspeccoesUsuarioParticipacao prospeccoesUsuario)
        {
            var prospeccoesUsuarioLiderComProposta = prospeccoesUsuario.ProspeccoesLider.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ComProposta || p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida);
            decimal quantidadeProspeccoesComPropostaLider = prospeccoesUsuarioLiderComProposta.Count();
            decimal valorTotalProspeccoesComPropostaLider = prospeccoesUsuarioLiderComProposta.Sum(p => p.ValorProposta == 0 ? p.ValorEstimado : p.ValorProposta);
            var prospeccoesUsuarioLiderConvertida = prospeccoesUsuario.ProspeccoesLider.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida);
            decimal quantidadeProspeccoesConvertidasLider = prospeccoesUsuarioLiderConvertida.Count();
            decimal valorTotalProspeccoesConvertidasLider = prospeccoesUsuarioLiderConvertida.Sum(p => p.ValorProposta == 0 ? p.ValorEstimado : p.ValorProposta);
            decimal valorMedioProspeccoesComPropostaLider = IndicadorHelper.DivisaoSegura(valorTotalProspeccoesComPropostaLider, quantidadeProspeccoesComPropostaLider);
            decimal valorMedioProspeccoesConvertidasLider = IndicadorHelper.DivisaoSegura(valorTotalProspeccoesConvertidasLider, quantidadeProspeccoesConvertidasLider);

            // Erro relativo
            participacao.AssertividadePrecificacao = IndicadorHelper.DivisaoSegura(Math.Abs(valorMedioProspeccoesConvertidasLider - valorMedioProspeccoesComPropostaLider), Math.Abs(valorMedioProspeccoesConvertidasLider));
            participacao.TaxaConversaoProposta = IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesComProposta, participacao.QuantidadeProspeccoes);
            participacao.TaxaConversaoProjeto = IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesConvertidas, participacao.QuantidadeProspeccoesComProposta);

            decimal despesaIsiMeses = CalculosParticipacao.CalculoDespesa(dataInicio, dataFim, despesas);
            decimal valorTotalProjetosParaFCF = await ExtrairValorProjetos(usuario, dataInicio, dataFim);

            // projetos convertidos / despesa
            participacao.FatorContribuicaoFinanceira = IndicadorHelper.DivisaoSegura(valorTotalProspeccoesConvertidasLider, despesaIsiMeses);
        }

        /// <summary>
        /// Realiza a atribuição de uma participação de acordo com as prospecções de um usuário.
        /// </summary>
        /// <param name="participacao">Objeto para as participação total de um usuário (ou genérico)</param>
        /// <param name="prospeccoesUsuario">Prospecções de um usuário (membro e lider)</param>
        private async Task AtribuirParticipacoesIndividuais(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
        {
            List<UsuarioParticipacaoDTO> usuarios = await _context.Users
                .Select(u => new UsuarioParticipacaoDTO { Id = u.Id, Cargo = new CargoDTO { Nome = u.Cargo.Nome, Id = u.Cargo.Id }, Casa = u.Casa, EmailConfirmed = u.EmailConfirmed, Nivel = u.Nivel, Email = u.Email, UserName = u.UserName })
                .ToListAsync();

            foreach (var prospeccao in prospeccoesUsuario)
            {
                bool prospConvertida = false;
                bool prospPlanejada = false;
                bool prospSuspensa = false;
                bool prospNaoConvertida = false;
                bool prospEmDiscussao = false;
                bool prospComProposta = false;

                int qtdBolsistas = 0;
                int qtdPesquisadores = 0;
                int qtdEstagiarios = 0;
                int qtdMembros = 0; // Não inclui o líder

                string nomeProjeto = !string.IsNullOrEmpty(prospeccao.NomeProspeccao) ? prospeccao.NomeProspeccao : $"{prospeccao.Empresa.Nome} (Empresa)";

                // Tratar prospecções que tem "projeto" no nome (...)
                // i.e: Remover na hora de apresentar o nome casos em que temos "Projeto projeto XYZ"
                // TODO: Transformar em REGEX
                if (!string.IsNullOrEmpty(nomeProjeto) && nomeProjeto.ToLowerInvariant().Contains("projeto") && !nomeProjeto.ToLowerInvariant().Contains("projetos"))
                {
                    nomeProjeto = nomeProjeto.Replace("projeto", "");
                    nomeProjeto = nomeProjeto.Replace("Projeto", "");
                }

                if (prospeccao.EquipeProspeccao != null)
                {
                    qtdBolsistas = prospeccao.EquipeProspeccao.Count(e => e.Usuario.Cargo?.Nome == nomeCargoBolsista); // TODO: Temporário, precisa estar definido de forma mais clara?
                    qtdEstagiarios = prospeccao.EquipeProspeccao.Count(e => e.Usuario.Cargo?.Nome == nomeCargoEstagiário); // TODO: Temporário, precisa estar definido de forma mais clara?
                    qtdPesquisadores = prospeccao.EquipeProspeccao.Count(e => e.Usuario.Cargo?.Nome == nomeCargoPesquisador); // TODO: Temporário, precisa estar definido de forma mais clara?

                    qtdMembros = qtdBolsistas + qtdEstagiarios + qtdPesquisadores;
                }

                Dictionary<string, decimal> calculoParticipantes = CalculosParticipacao.CalculoParticipacao(qtdPesquisadores, qtdBolsistas, qtdEstagiarios);

                var valorLider = calculoParticipantes["percentualLider"] * prospeccao.ValorProposta;
                var valorPesquisadores = calculoParticipantes["percentualPesquisadores"] * prospeccao.ValorProposta;
                var valorBolsistas = calculoParticipantes["percentualBolsistas"] * prospeccao.ValorProposta;
                var valorEstagiarios = calculoParticipantes["percentualEstagiarios"] * prospeccao.ValorProposta;
                var valorPorBolsista = calculoParticipantes["percentualPorBolsista"] * prospeccao.ValorProposta;
                var valorPorPesquisador = calculoParticipantes["percentualPorPesquisador"] * prospeccao.ValorProposta;
                var valorPorEstagiario = calculoParticipantes["percentualPorEstagiario"] * prospeccao.ValorProposta;

                var ultimoStatusProspeccao = prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status;

                switch (ultimoStatusProspeccao)
                {
                    case StatusProspeccao.Convertida:
                        prospConvertida = true;
                        break;

                    case StatusProspeccao.Planejada:
                        prospPlanejada = true;
                        break;

                    case StatusProspeccao.Suspensa:
                        prospSuspensa = true;
                        break;

                    case StatusProspeccao.NaoConvertida:
                        prospNaoConvertida = true;
                        break;

                    case StatusProspeccao.ContatoInicial:
                    case StatusProspeccao.Discussao_EsbocoProjeto:
                        prospEmDiscussao = true;
                        break;

                    case StatusProspeccao.ComProposta:
                        prospComProposta = true;
                        break;

                    default:
                        prospSuspensa = false;
                        prospConvertida = false;
                        prospPlanejada = false;
                        prospNaoConvertida = false;
                        prospEmDiscussao = false;
                        prospComProposta = false;
                        break;
                }

                participacao.Participacoes.Add(new ParticipacaoViewModel()
                {
                    Id = Guid.NewGuid(),
                    NomeProjeto = nomeProjeto,
                    EmpresaProjeto = prospeccao.Empresa.Nome,
                    ComProposta = prospComProposta,
                    EmDiscussao = prospEmDiscussao,
                    NaoConvertida = prospNaoConvertida,
                    Convertida = prospConvertida,
                    Planejada = prospPlanejada,
                    Suspensa = prospSuspensa,
                    ValorNominal = prospeccao.ValorProposta,
                    MembrosEquipe = prospeccao.EquipeProspeccao.Select(p => p.Usuario).ToList().ToString(),
                    ValorLider = valorLider,
                    ValorPesquisadores = valorPesquisadores,
                    ValorBolsistas = valorBolsistas,
                    ValorEstagiarios = valorEstagiarios,
                    ValorPorBolsista = valorPorBolsista,
                    ValorPorEstagiario = valorPorEstagiario,
                    ValorPorPesquisador = valorPorPesquisador,
                    QuantidadeBolsistas = qtdBolsistas,
                    QuantidadeEstagiarios = qtdEstagiarios,
                    QuantidadePesquisadores = qtdPesquisadores,
                    QuantidadeMembros = qtdMembros + 1 // 1 == Líder
                });
            }
        }

        /// <summary>
        /// Atribui as quantidades de prospecção de acordo com a participacao de um usuario, um usuario e suas prospeccoes
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="participacao"></param>
        /// <param name="prospeccoesUsuario"></param>
        private void AtribuirQuantidadesDeProspeccao(Usuario usuario, ParticipacaoTotalViewModel participacao, ProspeccoesUsuarioParticipacao prospeccoesUsuario)
        {
            decimal quantidadeProspeccoesComPropostaPeso = CalculosParticipacao.CalcularQuantidadeDeProspeccoesComProposta(usuario, prospeccoesUsuario);
            decimal quantidadeProspeccoesConvertidasPeso = CalculosParticipacao.CalcularQuantidadeDeProspeccoesConvertidas(usuario, prospeccoesUsuario);

            participacao.QuantidadeProspeccoes = prospeccoesUsuario.ProspeccoesTotais.Count();
            participacao.QuantidadeProspeccoesComProposta = quantidadeProspeccoesComPropostaPeso;
            participacao.QuantidadeProspeccoesConvertidas = quantidadeProspeccoesConvertidasPeso;
            participacao.QuantidadeProspeccoesMembro = prospeccoesUsuario.ProspeccoesMembro.Count();
            participacao.QuantidadeProspeccoesLider = prospeccoesUsuario.ProspeccoesLider.Count();
        }

        /// <summary>
        /// Atribui os valores financeiros relativos a participação de um usuário
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        /// <param name="participacao"></param>
        private void AtribuirValoresFinanceirosDeProspeccao(Usuario usuario, ParticipacaoTotalViewModel participacao, ProspeccoesUsuarioParticipacao prospeccoesUsuario)
        {
            participacao.ValorTotalProspeccoes = ExtrairValorProspeccoes(prospeccoesUsuario.ProspeccoesTotais, usuario);
            participacao.ValorTotalProspeccoesComProposta = ExtrairValorProspeccoes(prospeccoesUsuario.ProspeccoesTotaisComProposta, usuario);
            participacao.ValorTotalProspeccoesConvertidas = ExtrairValorProspeccoes(prospeccoesUsuario.ProspeccoesTotaisConvertidas, usuario);
            participacao.ValorMedioProspeccoes = IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoes, participacao.QuantidadeProspeccoes);
            participacao.ValorMedioProspeccoesComProposta = IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesComProposta, participacao.QuantidadeProspeccoesComProposta);
            participacao.ValorMedioProspeccoesConvertidas = IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesConvertidas, Math.Ceiling(participacao.QuantidadeProspeccoesConvertidas));
        }


        /// <summary>
        /// Obtém os rankings médios para serem utilizados no gráfico de aranha
        /// </summary>
        /// <param name="participacoes"></param>
        /// <returns></returns>
        private List<decimal> ObterRankingsMediosGrafico(List<ParticipacaoTotalViewModel> participacoes)
        {
            return new List<decimal> {
                participacoes.Average(p => p.MediaFatores),
                participacoes.Average(p => p.RankPorIndicador.RankValorTotalProspeccoes),
                participacoes.Average(p => p.RankPorIndicador.RankValorTotalProspeccoesComProposta),
                participacoes.Average(p => p.RankPorIndicador.RankValorMedioProspeccoes),
                participacoes.Average(p => p.RankPorIndicador.RankValorMedioProspeccoesComProposta),
                participacoes.Average(p => p.RankPorIndicador.RankValorTotalProspeccoesConvertidas),
                participacoes.Average(p => p.RankPorIndicador.RankQuantidadeProspeccoes),
                participacoes.Average(p => p.RankPorIndicador.RankQuantidadeProspeccoesLider),
                participacoes.Average(p => p.RankPorIndicador.RankQuantidadeProspeccoesComProposta),
                participacoes.Average(p => p.RankPorIndicador.RankQuantidadeProspeccoesConvertidas),
                participacoes.Average(p => p.RankPorIndicador.RankQuantidadeProspeccoesMembro),
                participacoes.Average(p => p.RankPorIndicador.RankAssertividadePrecificacao),
                participacoes.Average(p => p.RankPorIndicador.RankFatorContribuicaoFinanceira),
            };
        }

        /// <summary>
        /// Retorna os dados para o gráfico de participação do usuário
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("Participacao/RetornarDadosGrafico")]
        public async Task<IActionResult> RetornarDadosGrafico()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                List<IndicadoresFinanceirosDTO> indicadores = await ObterIndicadoresFinanceirosParaParticipacao();

                foreach (var indicador in indicadores)
                {
                    despesas[indicador.Data.Year] = indicador.Despesa;
                    pesquisadores[indicador.Data.Year] = indicador.QtdPesquisadores;
                }

                _prospeccoes = await ObterProspeccoesParaParticipacao();

                var participacoes = await GetParticipacoesTotaisUsuarios(new DateTime(2021, 01, 01), new DateTime(DateTime.Now.Year, 12, 31));
                Dictionary<string, object> dadosGrafico = new Dictionary<string, object>();
                List<decimal> rankingsMedios = new List<decimal>();

                if (participacoes.Count > 0)
                {
                    RankearParticipacoes(participacoes, true);
                    CalculosParticipacao.CalcularValorSobreMediaDoFCF(participacoes);
                    rankingsMedios = ObterRankingsMediosGrafico(participacoes);
                }

                var participacaoUsuario = participacoes.FirstOrDefault(p => p.Lider.Id == UsuarioAtivo.Id);

                dadosGrafico["Participacao"] = participacaoUsuario;
                dadosGrafico["Rankings"] = rankingsMedios;

                if (UsuarioAtivo != null)
                {
                    return Ok(JsonConvert.SerializeObject(dadosGrafico));
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
        /// Percorre o range de data inicio até data fim e cria valores e labels para o gráfico de prospeccoes
        /// </summary>
        /// <param name="participacao"></param>
        /// <param name="prospeccoesUsuario"></param>
        private void ComputarRangeGraficoParticipacao(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
        {
            if (prospeccoesUsuario.Count > 0)
            {
                DateTime dataInicial = prospeccoesUsuario.Where(p => p.Status.Count > 0).Min(p => p.Status.Min(f => f.Data));
                DateTime dataIterada = dataInicial;
                DateTime dataAtual = DateTime.Now;
                List<string> mesAno = new List<string>();
                List<decimal> valoresProposta = new List<decimal>();

                while (dataIterada <= dataAtual)
                {
                    string dataFormatada = $"{dataIterada:MMM} {dataIterada.Year}";
                    mesAno.Add(dataFormatada);
                    decimal valorSomado = prospeccoesUsuario
                        .FindAll(p => p.Status
                        .Any(f => new DateTime(f.Data.Year, f.Data.Month, 1) <= new DateTime(dataIterada.Year, dataIterada.Month, 1)))
                        .Sum(p => p.ValorProposta);
                    valoresProposta.Add(valorSomado);
                    dataIterada = dataIterada.AddMonths(1);
                }

                participacao.Valores = valoresProposta;
                participacao.Labels = mesAno;
            }
        }

        /// <summary>
        /// Envia para o ViewData os valores mínimos e máximos de cada propriedade do tipo decimal das participações
        /// </summary>
        /// <param name="participacoes"></param>
        private void DefinirValoresMinMax(List<ParticipacaoTotalViewModel> participacoes)
        {
            PropertyInfo[] atributos = typeof(ParticipacaoTotalViewModel).GetProperties();

            Dictionary<string, decimal> valoresMaximos = new Dictionary<string, decimal>();
            Dictionary<string, decimal> valoresMinimos = new Dictionary<string, decimal>();

            foreach (var property in atributos)
            {
                if (property.PropertyType == typeof(decimal))
                {
                    decimal valorMaximo = participacoes.Max(x => (decimal)property.GetValue(x));
                    decimal valorMinimo = participacoes.Min(x => (decimal)property.GetValue(x));

                    valoresMaximos[$"{property.Name}"] = valorMaximo;
                    valoresMinimos[$"{property.Name}"] = valorMinimo;
                }

                if (property.PropertyType == typeof(int))
                {
                    int valorMaximo = participacoes.Max(x => (int)property.GetValue(x));
                    int valorMinimo = participacoes.Min(x => (int)property.GetValue(x));

                    valoresMaximos[$"{property.Name}"] = valorMaximo;
                    valoresMinimos[$"{property.Name}"] = valorMinimo;
                }
            }

            ViewData["ValoresMaximos"] = valoresMaximos;
            ViewData["ValoresMinimos"] = valoresMinimos;
        }

        /// <summary>
        /// TODO: SRP
        /// TODO: Apenas como lider??
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        /// <returns></returns>
        private async Task<decimal> ExtrairValorProjetos(Usuario usuario, DateTime dataInicio, DateTime dataFim)
        {
            var projetosUsuario = await _context.Projeto.Where(p => p.UsuarioId == usuario.Id).ToListAsync();

            projetosUsuario = AcertarPrecificacaoProjetos(dataInicio, dataFim, projetosUsuario);
            return (decimal)projetosUsuario.Sum(p => p.ValorTotalProjeto);
        }

        /// <summary>
        /// Extrai o valor total das prospecções do usuário considerando o valor da proposta se presente, se não, o valor estimado
        /// </summary>
        /// <param name="prospeccoes"></param>
        /// <param name="valorTotalProspeccoes"></param>
        /// <returns></returns>
        private decimal ExtrairValorProspeccoes(List<Prospeccao> prospeccoes, Usuario usuario)
        {
            decimal valorProspeccoes = 0;

            foreach (var prospeccao in prospeccoes)
            {
                // Verificar se a prospecção tem como líder o usuário passado ou se a prospecção tem como membro o usuário passado
                if (prospeccao.Usuario.Id == usuario.Id || (prospeccao.EquipeProspeccao?.Any(u => u.IdUsuario == usuario.Id) ?? false))
                {
                    List<Usuario> membrosProspeccao = prospeccao.EquipeProspeccao.Select(e => e.Usuario).ToList();
                    // TODO: Remover hardcoding no futuro!!
                    int qtdBolsistas = membrosProspeccao.Where(m => m.Cargo?.Nome == nomeCargoBolsista).Count();
                    int qtdPesquisadores = membrosProspeccao.Where(m => m.Cargo?.Nome == nomeCargoPesquisador).Count();
                    int qtdEstagiarios = membrosProspeccao.Where(m => m.Cargo?.Nome == nomeCargoEstagiário).Count();

                    decimal percentualPesquisador = CalculosParticipacao.CalculoPercentualPesquisador(1 + membrosProspeccao.Count, qtdPesquisadores);
                    decimal percentualBolsista = CalculosParticipacao.CalculoPercentualBolsista(1 + membrosProspeccao.Count, qtdBolsistas);
                    decimal percentualEstagiario = CalculosParticipacao.CalculoPercentualEstagiario(1 + membrosProspeccao.Count, qtdEstagiarios);
                    decimal percentualLider = 1 - (percentualBolsista + percentualPesquisador + percentualEstagiario);

                    decimal percentualTotalPesquisador = percentualPesquisador * qtdPesquisadores;
                    decimal percentualTotalBolsistas = percentualBolsista * qtdBolsistas;
                    decimal percentualTotalEstagiarios = percentualEstagiario * qtdEstagiarios;

                    // Se for lider...
                    if (prospeccao.Usuario.Id == usuario.Id)
                    {
                        // Usaremos o valor da proposta se não tivermos o valor estimado
                        decimal valorMultiplicado = prospeccao.ValorProposta == 0 ? prospeccao.ValorEstimado : prospeccao.ValorProposta;
                        valorProspeccoes += percentualLider * valorMultiplicado;
                    } // Se não for líder...
                    else
                    {
                        // Em tese não haveria necessidade de verificar o cargo...
                        switch (usuario.Cargo?.Nome)
                        {
                            case nomeCargoEstagiário:
                                if (prospeccao.ValorProposta != 0)
                                {
                                    valorProspeccoes += percentualEstagiario * prospeccao.ValorProposta;
                                }
                                else
                                {
                                    valorProspeccoes += percentualEstagiario * prospeccao.ValorEstimado;
                                }
                                break;

                            case nomeCargoPesquisador:
                                if (prospeccao.ValorProposta != 0)
                                {
                                    valorProspeccoes += percentualPesquisador * prospeccao.ValorProposta;
                                }
                                else
                                {
                                    valorProspeccoes += percentualPesquisador * prospeccao.ValorEstimado;
                                }
                                break;

                            case nomeCargoBolsista:
                                if (prospeccao.ValorProposta != 0)
                                {
                                    valorProspeccoes += percentualBolsista * prospeccao.ValorProposta;
                                }
                                else
                                {
                                    valorProspeccoes += percentualBolsista * prospeccao.ValorEstimado;
                                }
                                break;
                        }
                    }
                }
            }

            return valorProspeccoes;
        }



        /// <summary>
        /// Filtra as prospecções do usuario de acordo com a data de inicio e data de fim do filtro
        /// </summary>
        /// <param name="prospeccoes"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        private void PeriodizarProspeccoesUsuario(ProspeccoesUsuarioParticipacao prospeccoes, DateTime dataInicio, DateTime dataFim)
        {
            prospeccoes.ProspeccoesMembro = ParticipacaoHelper.FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesMembro);
            prospeccoes.ProspeccoesLider = ParticipacaoHelper.FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesLider);
            prospeccoes.ProspeccoesTotais = ParticipacaoHelper.FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesTotais);
            prospeccoes.ProspeccoesTotaisComProposta = ParticipacaoHelper.FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesTotaisComProposta);
            prospeccoes.ProspeccoesTotaisConvertidas = ParticipacaoHelper.FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesTotaisConvertidas);
        }

        /// <summary>
        /// Obtém uma participação de acordo com um usuário específico.
        /// </summary>
        /// <param name="usuario">Usuário do sistema a ter participações retornadas</param>
        /// <returns></returns>
        private async Task<ParticipacaoTotalViewModel> GetParticipacaoTotalUsuario(Usuario usuario, DateTime dataInicio, DateTime dataFim)
        {
            ParticipacaoTotalViewModel participacao = new ParticipacaoTotalViewModel
            {
                Participacoes = new List<ParticipacaoViewModel>(),
                Lider = usuario.ToUsuarioParticipacao(),
            };

            ProspeccoesUsuarioParticipacao prospeccoesUsuario = ObterProspeccoesUsuario(usuario);

            PeriodizarProspeccoesUsuario(prospeccoesUsuario, dataInicio, dataFim);

            // Evitar exibir usuários sem prospecção
            if (prospeccoesUsuario.ProspeccoesTotais.Count == 0)
            {
                return null;
            }

            // TODO: Computar range do gráfico? (Obs: eu acho que faz mais sentido a lógica referente ao grafico estar abstraída em outro lugar...)
            await AtribuirParticipacoesIndividuais(participacao, prospeccoesUsuario.ProspeccoesTotais);

            AtribuirQuantidadesDeProspeccao(usuario, participacao, prospeccoesUsuario);
            AtribuirValoresFinanceirosDeProspeccao(usuario, participacao, prospeccoesUsuario);

            await AtribuirAssertividadePrecificacao(usuario, dataInicio, dataFim, participacao, prospeccoesUsuario);

            // TODO: Código inutilizado??
            //double quantidadePesquisadores = CalculoNumeroPesquisadores(dataInicio.Year, dataFim.Year);

            return participacao;
        }

        /// <summary>
        /// Obtém as prospecções do usuário para serem utilizadas em participação
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private ProspeccoesUsuarioParticipacao ObterProspeccoesUsuario(Usuario usuario)
        {
            var prospeccoesTotais = GetProspeccoesUsuarioMembroEquipe(usuario);
            var prospeccoesTotaisConvertidas = ParticipacaoHelper.FiltrarProspeccoesConvertidas(prospeccoesTotais);
            var prospeccoesTotaisComProposta = ParticipacaoHelper.FiltrarProspeccoesComProposta(prospeccoesTotais);
            var prospeccoesLider = GetProspeccoesUsuarioLider(usuario);
            var prospeccoesLiderConvertidas = ParticipacaoHelper.FiltrarProspeccoesConvertidas(prospeccoesLider);
            var prospeccoesMembro = GetProspeccoesUsuarioMembro(usuario);


            var prospeccoesUsuario = new ProspeccoesUsuarioParticipacao
            {
                ProspeccoesLider = prospeccoesLider,
                ProspeccoesLiderConvertidas = prospeccoesLiderConvertidas,
                ProspeccoesMembro = prospeccoesMembro,
                ProspeccoesTotais = prospeccoesTotais,
                ProspeccoesTotaisConvertidas = prospeccoesTotaisConvertidas,
                ProspeccoesTotaisComProposta = prospeccoesTotaisComProposta,
            };
            return prospeccoesUsuario;
        }


        /// <summary>
        /// Obtém uma lista de participações de todos os usuários, com base na casa do usuário que está acessando.
        /// </summary>
        /// <returns></returns>
        private async Task<List<ParticipacaoTotalViewModel>> GetParticipacoesTotaisUsuarios(DateTime dataInicio, DateTime dataFim)
        {
            List<UsuarioParticipacaoDTO> usuariosDTO;

            // Obs: Incluir pesquisadores da Q4.0 que façam prospecções abaixo. Eles possuem nível 3/2 logo podem acabar não apareçendo na listagem de participação
            if (UsuarioAtivo.Casa == Instituto.ISIQV || UsuarioAtivo.Casa == Instituto.CISHO)
            {
                usuariosDTO = await _cache.GetCachedAsync($"Usuarios:Participacao:{dataInicio}:{dataFim}:{UsuarioAtivo.Casa}", () =>
                    _context.Users.Select(u => new UsuarioParticipacaoDTO { Cargo = new CargoDTO { Nome = u.Cargo.Nome, Id = u.Cargo.Id }, Casa = u.Casa, Email = u.Email, EmailConfirmed = u.EmailConfirmed, Nivel = u.Nivel, Id = u.Id, UserName = u.UserName })
                        .Where(u => ((u.Casa == Instituto.ISIQV || u.Casa == Instituto.CISHO) && u.Cargo.Nome == nomeCargoPesquisador && u.EmailConfirmed == true && u.Nivel == Nivel.Usuario) || u.Email.Contains("lednascimento"))
                        .ToListAsync());
            }
            else
            {
                usuariosDTO = await _cache.GetCachedAsync($"Usuarios:Participacao:{dataInicio}:{dataFim}:{UsuarioAtivo.Casa}", () =>
                    _context.Users.Select(u => new UsuarioParticipacaoDTO { Cargo = new CargoDTO { Nome = u.Cargo.Nome, Id = u.Cargo.Id }, Casa = u.Casa, Email = u.Email, EmailConfirmed = u.EmailConfirmed, Nivel = u.Nivel, Id = u.Id, UserName = u.UserName })
                        .Where(u => (u.Casa == UsuarioAtivo.Casa && u.EmailConfirmed == true && u.Cargo.Nome == nomeCargoPesquisador && u.Nivel == Nivel.Usuario) || u.Email.Contains("lednascimento"))
                        .ToListAsync());
            }

            List<ParticipacaoTotalViewModel> participacoes = new List<ParticipacaoTotalViewModel>();

            foreach (var usuarioDTO in usuariosDTO)
            {
                var participacao = await GetParticipacaoTotalUsuario(usuarioDTO.ToUsuario(), dataInicio, dataFim);

                if (participacao != null)
                {
                    participacoes.Add(participacao);
                }
            }

            return participacoes;
        }

        /// <summary>
        /// Obtem a lista de projetos do usuário passado por parâmetro
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private async Task<List<Projeto>> GetProjetosUsuario(Usuario usuario)
        {
            return await _context.Projeto.Where(p => p.EquipeProjeto.Any(e => e.Usuario == usuario) && p.Usuario == usuario).ToListAsync();
        }

        /// <summary>
        /// Obtem a lista de projetos do usuário passado por parâmetro
        /// Somente os projetos em que o usuário é membro
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private async Task<List<Projeto>> GetProjetosUsuarioMembro(Usuario usuario)
        {
            return await _context.Projeto.Where(p => p.EquipeProjeto.Any(e => e.Usuario == usuario)).ToListAsync();
        }

        /// <summary>
        /// Obtém a lista de prospecções do usuário passa por parâmetro nas quais ele é lider (somente)
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private List<Prospeccao> GetProspeccoesUsuarioLider(Usuario usuario)
        {
            return _prospeccoes.Where(p => p.Usuario.Id == usuario.Id && p.Status.OrderBy(f => f.Data).LastOrDefault().Status != StatusProspeccao.Planejada).ToList();
        }



        /// <summary>
        /// Obter todas as prospecções em que o usuário é um membro (apenas)
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private List<Prospeccao> GetProspeccoesUsuarioMembro(Usuario usuario)
        {
            // Somente membro
            return _prospeccoes.Where(p =>
                p.EquipeProspeccao != null &&
                p.EquipeProspeccao.Any(e => e.Usuario.Id == usuario.Id) &&
                (p.Status == null || p.Status.OrderBy(f => f.Data).LastOrDefault()?.Status != StatusProspeccao.Planejada)
            ).ToList();
        }

        /// <summary>
        /// Obter as prospecções de um usuário, incluindo as que ele é participante
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private List<Prospeccao> GetProspeccoesUsuarioMembroEquipe(Usuario usuario)
        {
            return _prospeccoes
                .Where(p =>
                (p.Usuario.Id == usuario.Id || (p.EquipeProspeccao != null && p.EquipeProspeccao.Any(e => e.Usuario.Id == usuario.Id))) &&
                (p.Status == null || p.Status.OrderBy(f => f.Data).LastOrDefault()?.Status != StatusProspeccao.Planejada)
            ).ToList();
        }

        private List<ParticipacaoTotalViewModel> PrepararDadosParticipacao(List<ParticipacaoTotalViewModel> participacoes)
        {
            if (participacoes.Count > 0)
            {
                RankearParticipacoes(participacoes, false);
                DefinirValoresMinMax(participacoes);

                participacoes = participacoes.OrderByDescending(p => p.MediaFatores).ToList();
            }

            return participacoes;
        }
    }
}