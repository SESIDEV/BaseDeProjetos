using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using SendGrid.Helpers.Mail;
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
        // TODO: Precisamos não utilizar esses valores mágicos de string no futuro!!
        private const string nomeCargoPesquisador = "Pesquisador QMS";
        private const string nomeCargoEstagiário = "Estagiário";
        private const string nomeCargoBolsista = "Pesquisador Bolsista";
        private readonly ApplicationDbContext _context;
        private readonly DbCache _cache;
        private readonly ILogger<ParticipacaoController> _logger;
        private List<Prospeccao> _prospeccoes = new List<Prospeccao>();
        private const int AnoPadrao = 2021;
        private const int MesInicioPadrao = 1;
        private const int MesFimPadrao = 12;

        private readonly static Dictionary<int, decimal> despesas = new Dictionary<int, decimal>();
        private readonly static Dictionary<int, int> pesquisadores = new Dictionary<int, int>();

        public ParticipacaoController(ApplicationDbContext context, DbCache cache, ILogger<ParticipacaoController> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Efetua o cálculo das despesas do ISI com base no período selecionado
        /// </summary>
        /// <param name="mesInicial"></param>
        /// <param name="anoInicial"></param>
        /// <param name="mesFinal"></param>
        /// <param name="anoFinal"></param>
        /// <returns></returns>
        internal static decimal CalculoDespesa(DateTime dataInicio, DateTime dataFim)
        {
            if (dataInicio > dataFim)
            {
                throw new ArgumentException($"{nameof(dataInicio)} não pode ser maior que {nameof(dataFim)}");
            }

            decimal valorCustoFinal = 0;

            if (dataFim.Year == dataInicio.Year)
            {
                if (dataInicio.Month > dataFim.Month)
                {
                    throw new ArgumentException($"O mês da data inicial não pode ser maior que o mês da data final");
                }

                int quantidadeDeMesesAno = dataFim.Month - dataInicio.Month + 1;
                valorCustoFinal = despesas[dataInicio.Year] / 12 * quantidadeDeMesesAno;
                return valorCustoFinal;
            }
            else
            {
                int quantidadeDeMesesAnoInicial = 12 - dataInicio.Month + 1;
                // Evitar misses do dicionário
                int anoFinalTruncado = Math.Min(dataFim.Year, despesas.Keys.Last());

                valorCustoFinal += despesas[dataInicio.Year] / 12 * quantidadeDeMesesAnoInicial;

                int quantidadeDeMesesAnoFinal = dataFim.Month;

                valorCustoFinal += despesas[anoFinalTruncado] / 12 * quantidadeDeMesesAnoFinal;

                int subtracaoAno = anoFinalTruncado - dataInicio.Year - 1;

                if (subtracaoAno <= 0)
                {
                    return valorCustoFinal;
                }
                else
                {
                    for (int i = 1; i <= subtracaoAno; i++)
                    {
                        valorCustoFinal += despesas[dataInicio.Year + i];
                    }
                }

                return valorCustoFinal;
            }
        }

        /// <summary>
        /// Efetua o cálculo relativo à participação de cada tipo de membro de acordo com a quantidade de membros de uma equipe
        /// </summary>
        /// <param name="qtdPesquisadores">Número de Pesquisadores no Projeto</param>
        /// <param name="qtdBolsistas">Número de Bolsistas no Projeto</param>
        /// <param name="qtdEstagiarios">Número de Estagiários no Projeto</param>
        /// <returns></returns>
        private Dictionary<string, decimal> CalculoParticipacao(int qtdPesquisadores, int qtdBolsistas, int qtdEstagiarios)
        {
            int numeroMembros = 1 + qtdPesquisadores + qtdBolsistas + qtdEstagiarios;

            decimal percentualPorPesquisador = CalculoPercentualPesquisador(numeroMembros, qtdPesquisadores);
            decimal percentualPorBolsista = CalculoPercentualBolsista(numeroMembros, qtdBolsistas);
            decimal percentualPorEstagiario = CalculoPercentualEstagiario(numeroMembros, qtdEstagiarios);

            decimal percentualTodosPesquisadores = percentualPorPesquisador * qtdPesquisadores;
            decimal percentualTodosBolsistas = percentualPorBolsista * qtdBolsistas;
            decimal percentualTodosEstagiarios = percentualPorEstagiario * qtdEstagiarios;
            decimal percentualLider = 1 - (percentualTodosBolsistas + percentualTodosEstagiarios + percentualTodosPesquisadores);

            return new Dictionary<string, decimal>
            {
                { "percentualPesquisadores", percentualTodosPesquisadores },
                { "percentualBolsistas", percentualTodosBolsistas },
                { "percentualEstagiarios", percentualTodosEstagiarios },
                { "percentualLider", percentualLider },
                { "percentualPorPesquisador", percentualPorPesquisador },
                { "percentualPorBolsista", percentualPorBolsista },
                { "percentualPorEstagiario", percentualPorEstagiario },
            };
        }

        /// <summary>
        /// Método para cálculo da porcentagem relativa ao estagiário
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto</param>
        /// <param name="estagiarios">Número de estagiários no projeto</param>
        /// <returns></returns>
        private decimal CalculoPercentualEstagiario(decimal numeroMembros, decimal estagiarios)
        {
            if (estagiarios == 0)
            {
                return 0;
            }
            else
            {
                decimal percentual = (1 - 1 / numeroMembros) * 1 / 10 * (1 / (estagiarios + 1));
                return percentual;
            }
        }

        /// <summary>
        /// Método para cálculo da porcentagem relativa ao bolsista
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto</param>
        /// <param name="bolsistas">Número de bolsistas no projeto</param>
        /// <returns></returns>
        private decimal CalculoPercentualBolsista(decimal numeroMembros, decimal bolsistas)
        {
            if (bolsistas == 0)
            {
                return 0;
            }
            else
            {
                decimal resultado = (1 - (1 / numeroMembros)) * 3 / 10 * (1 / (bolsistas + 1));
                return resultado;
            }
        }

        /// <summary>
        /// Método para calculo da porcentagem relativa ao pesquisador
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto (inclui o lider)</param>
        /// <param name="qtdPesquisadores">Número de pesquisadores no projeto</param>
        /// <returns></returns>
        private decimal CalculoPercentualPesquisador(decimal numeroMembros, decimal qtdPesquisadores)
        {
            if (qtdPesquisadores == 0)
            {
                return 0;
            }
            else
            {
                decimal percentual = (1 - (1 / numeroMembros)) * 3 / 5 * (1 / (qtdPesquisadores + 1));
                return percentual;
            }
        }

        /// <summary>
        /// Retorna os dados do gráfico temporal considerando o id do usuário passado por parâmetro
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("Participacao/RetornarDadosGraficoTemporal/{idUsuario}")]
        public async Task<IActionResult> RetornarDadosGraficoTemporal(string idUsuario)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = new Usuario();

                if (!string.IsNullOrEmpty(idUsuario))
                {
                    usuario = await _context.Users.Where(u => u.Id == idUsuario).FirstOrDefaultAsync();
                }

                _prospeccoes = await _cache.GetCachedAsync("Prospeccoes:Participacao", () => _context.Prospeccao.Include(p => p.Usuario).Include(p => p.Empresa).ToListAsync());

                var participacao = await GetParticipacaoTotalUsuario(usuario);

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

        /// <summary>
        /// Retorna os dados para o gráfico de participação do usuário
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("Participacao/RetornarDadosGrafico")]
        public async Task<IActionResult> RetornarDadosGrafico()
        {
            ViewbagizarUsuario(_context);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                _prospeccoes = await _cache.GetCachedAsync("Prospeccoes:Participacao", () => _context.Prospeccao.Include(p => p.Usuario).Include(p => p.Empresa).ToListAsync());

                var participacoes = await GetParticipacoesTotaisUsuarios();
                Dictionary<string, object> dadosGrafico = new Dictionary<string, object>();
                List<decimal> rankingsMedios = new List<decimal>();

                if (participacoes.Count > 0)
                {
                    RankearParticipacoes(participacoes, true);
                    CalcularValorSobreMediaDoFCF(participacoes);
                    rankingsMedios = ObterRankingsMedios(participacoes);
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

                    valoresMaximos[$"{property.Name}"] = (decimal)valorMaximo;
                    valoresMinimos[$"{property.Name}"] = (decimal)valorMinimo;
                }
            }

            ViewData["ValoresMaximos"] = valoresMaximos;
            ViewData["ValoresMinimos"] = valoresMinimos;
        }

        /// <summary>
        /// Obter as prospecções de um usuário, incluindo as que ele é participante
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private List<Prospeccao> GetProspeccoesUsuarioMembroEquipe(Usuario usuario)
        {
            return _prospeccoes.Where(p =>
                (p.Usuario == usuario || (p.MembrosEquipe != null && p.MembrosEquipe.Contains(usuario.Email))) &&
                (p.Status == null || p.Status.OrderBy(f => f.Data).LastOrDefault()?.Status != StatusProspeccao.Planejada)
            ).ToList();
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
        /// Converter strings contendo mes e ano em datetimes considerando ano e mes padrões
        /// </summary>
        /// <param name="mes">string do mes: "01"</param>
        /// <param name="ano">string do ano: "2023"</param>
        /// <param name="anoPadrao"></param>
        /// <param name="mesPadrao"></param>
        /// <returns></returns>
        private DateTime InicializarDatetimes(string mes, string ano, int anoPadrao, int mesPadrao)
        {
            if (!int.TryParse(ano, out int anoParse))
            {
                anoParse = anoPadrao;
            }

            if (!int.TryParse(mes, out int mesParse))
            {
                mesParse = mesPadrao;
            }

            return new DateTime(anoParse, mesParse, 1);
        }

        /// <summary>
        /// Filtra as prospecções do usuario de acordo com a data de inicio e data de fim do filtro
        /// </summary>
        /// <param name="prospeccoes"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        private void FiltrarProspeccoesUsuario(ProspeccoesUsuarioParticipacao prospeccoes, DateTime dataInicio, DateTime dataFim)
        {
            prospeccoes.ProspeccoesMembro = FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesMembro);
            prospeccoes.ProspeccoesLider = FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesLider);
            prospeccoes.ProspeccoesTotais = FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesTotais);
        }

        /// <summary>
        /// Obtém uma participação de acordo com um usuário específico.
        /// </summary>
        /// <param name="usuario">Usuário do sistema a ter participações retornadas</param>
        /// <returns></returns>
        private async Task<ParticipacaoTotalViewModel> GetParticipacaoTotalUsuario(Usuario usuario, string mesInicio = null, string anoInicio = null, string mesFim = null, string anoFim = null)
        {

        private async Task AtribuirAssertividadePrecificacao(Usuario usuario, DateTime dataInicio, DateTime dataFim, ParticipacaoTotalViewModel participacao, ProspeccoesUsuarioParticipacao prospeccoesUsuario)
        {
            var prospeccoesUsuarioLiderComProposta = prospeccoesUsuario.ProspeccoesLider.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ComProposta);
            decimal quantidadeProspeccoesComPropostaLider = prospeccoesUsuarioLiderComProposta.Count();
            decimal valorTotalProspeccoesComPropostaLider = prospeccoesUsuarioLiderComProposta.Sum(p => p.ValorProposta);
            var prospeccoesUsuarioLiderConvertida = prospeccoesUsuario.ProspeccoesLider.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida);
            decimal quantidadeProspeccoesConvertidasLider = prospeccoesUsuarioLiderConvertida.Count();
            decimal valorTotalProspeccoesConvertidasLider = prospeccoesUsuarioLiderConvertida.Sum(p => p.ValorProposta);
            decimal valorMedioProspeccoesComPropostaLider = IndicadorHelper.DivisaoSegura(valorTotalProspeccoesComPropostaLider, quantidadeProspeccoesComPropostaLider);
            decimal valorMedioProspeccoesConvertidasLider = IndicadorHelper.DivisaoSegura(valorTotalProspeccoesConvertidasLider, quantidadeProspeccoesConvertidasLider);

            // Erro relativo
            participacao.AssertividadePrecificacao = IndicadorHelper.DivisaoSegura(Math.Abs(valorMedioProspeccoesConvertidasLider - valorMedioProspeccoesComPropostaLider), Math.Abs(valorMedioProspeccoesConvertidasLider));
            participacao.ValorMedioProspeccoesConvertidas = valorMedioProspeccoesConvertidasLider;
            participacao.TaxaConversaoProjeto = IndicadorHelper.DivisaoSegura(quantidadeProspeccoesConvertidasLider, quantidadeProspeccoesComPropostaLider);
            participacao.TaxaConversaoProposta = IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesComProposta, participacao.QuantidadeProspeccoes);

            decimal despesaIsiMeses = CalculoDespesa(dataInicio, dataFim);
            decimal valorTotalProjetosParaFCF = await ExtrairValorProjetos(usuario, dataInicio, dataFim);

            participacao.FatorContribuicaoFinanceira = IndicadorHelper.DivisaoSegura(valorTotalProjetosParaFCF, despesaIsiMeses);
        }

        private void AtribuirValoresFinanceirosDeProspeccao(Usuario usuario, DateTime dataInicio, DateTime dataFim, ParticipacaoTotalViewModel participacao)
        {
            participacao.ValorTotalProspeccoes = ExtrairValorProspeccoes(usuario, null, dataInicio, dataFim);
            participacao.ValorTotalProspeccoesComProposta = ExtrairValorProspeccoes(usuario, StatusProspeccao.ComProposta, dataInicio, dataFim);
            participacao.ValorTotalProspeccoesConvertidas = ExtrairValorProspeccoes(usuario, StatusProspeccao.Convertida, dataInicio, dataFim);
            participacao.ValorMedioProspeccoes = IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoes, participacao.QuantidadeProspeccoes);
            participacao.ValorMedioProspeccoesComProposta = IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesComProposta, participacao.QuantidadeProspeccoesComProposta);
        }


            foreach (var prospeccao in prospeccoesUsuarioComProposta)
            {
                var membrosEquipe = TratarMembrosEquipeString(prospeccao);

                if (prospeccao.Usuario.Id == usuario.Id)
                {
                    var percBolsista = CalculoPercentualBolsista(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoBolsista).Count());
                    var percEstagiario = CalculoPercentualEstagiario(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoEstagiário).Count());
                    var percPesquisador = CalculoPercentualPesquisador(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoPesquisador).Count());

                    var percLider = 1 - (percBolsista + percEstagiario + percPesquisador);

                    quantidadeProspeccoesComProposta += percLider;
                }
                else if (prospeccao.MembrosEquipe.Contains(usuario.Email))
                {
                    quantidadeProspeccoesComProposta += CalculoPercentualPesquisador(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoPesquisador).Count());
                }
            }

            participacao.QuantidadeProspeccoesComProposta = quantidadeProspeccoesComProposta;

            foreach (var prospeccao in prospeccoesUsuarioConvertidas)
            {
                var membrosEquipe = TratarMembrosEquipeString(prospeccao);

                if (prospeccao.Usuario.Id == usuario.Id)
                {
                    var percBolsista = CalculoPercentualBolsista(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoBolsista).Count());
                    var percEstagiario = CalculoPercentualEstagiario(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoEstagiário).Count());
                    var percPesquisador = CalculoPercentualPesquisador(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoPesquisador).Count());

                    var percLider = 1 - (percBolsista + percEstagiario + percPesquisador);

                    quantidadeProspeccoesConvertidas += percLider;
                }
                else
                {
                    quantidadeProspeccoesConvertidas += CalculoPercentualPesquisador(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoPesquisador).Count());
                }
            }

            participacao.QuantidadeProspeccoesProjeto = quantidadeProspeccoesConvertidas;

            participacao.QuantidadeProspeccoesMembro = quantidadeProspeccoesMembro = prospeccoesUsuarioMembro.Count();
            participacao.QuantidadeProspeccoesLider = quantidadeProspeccoesLider = prospeccoesUsuarioLider.Count();

            // Apenas para calculo da Assertividade
            quantidadeProspeccoesComPropostaLider = prospeccoesUsuarioLider.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ComProposta).Count();
            quantidadeProspeccoesConvertidasLider = prospeccoesUsuarioLider.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida).Count();
            valorTotalProspeccoesComPropostaLider = prospeccoesUsuarioLider.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ComProposta).Sum(p => p.ValorProposta);
            valorTotalProspeccoesConvertidasLider = prospeccoesUsuarioLider.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida).Sum(p => p.ValorProposta);

            participacao.Lider = usuario;

            // Evita divisão por 0
            if (prospeccoesUsuarioMembroEquipe.Count() == 0)
            {
                participacao.ValorMedioProspeccoes = 0;
                participacao.ValorMedioProspeccoesComProposta = 0;
                participacao.TaxaConversaoProposta = 0;
                participacao.TaxaConversaoProjeto = 0;
            }
            else
            {
                participacao.TaxaConversaoProposta = taxaConversaoProposta = quantidadeProspeccoesComProposta / quantidadeProspeccoesComPeso;
                participacao.ValorMedioProspeccoes = valorMedioProspeccoes = valorTotalProspeccoes / quantidadeProspeccoes;

                if (quantidadeProspeccoesComPropostaLider > 0 && quantidadeProspeccoesComProposta > 0)
                {
                    participacao.ValorMedioProspeccoesComProposta = valorMedioProspeccoesComProposta = valorTotalProspeccoesComProposta / quantidadeProspeccoesComProposta;
                    valorMedioProspeccoesComPropostaLider = valorTotalProspeccoesComPropostaLider / quantidadeProspeccoesComPropostaLider;
                }

                if (quantidadeProspeccoesConvertidasLider > 0 && quantidadeProspeccoesConvertidas > 0)
                {
                    participacao.ValorMedioProspeccoesConvertidas = valorMedioProspeccoesConvertidas = valorTotalProspeccoesConvertidas / quantidadeProspeccoesConvertidas;
                    valorMedioProspeccoesConvertidasLider = valorTotalProspeccoesConvertidasLider / quantidadeProspeccoesConvertidasLider;
                }

                if (valorMedioProspeccoesConvertidasLider != 0)
                {
                    var erroRelativo = Math.Abs(valorMedioProspeccoesConvertidasLider - valorMedioProspeccoesComPropostaLider) / Math.Abs(valorMedioProspeccoesConvertidasLider);
                    participacao.AssertividadePrecificacao = erroRelativo;
                }

                if (quantidadeProspeccoesComProposta != 0)
                {
                    participacao.TaxaConversaoProjeto = taxaConversaoProjeto = quantidadeProspeccoesConvertidas / quantidadeProspeccoesComProposta;
                }
                else
                {
                    participacao.TaxaConversaoProjeto = taxaConversaoProjeto = 0;
                }


                if (mesInicio == null || anoInicio == null)
                {
                    mesInicio = "01";
                    anoInicio = "2021";
                }

                HandleMesFimAnoFimInvalido(ref mesFim, ref anoFim);

                despesaIsiMeses = await CalculoDespesa(_context, int.Parse(mesInicio), int.Parse(anoInicio), int.Parse(mesFim), int.Parse(anoFim));

                quantidadePesquisadores = CalculoNumeroPesquisadores(int.Parse(anoInicio), int.Parse(anoFim));

                participacao.FatorContribuicaoFinanceira = valorTotalProjetosParaFCF / despesaIsiMeses;
            }

            return participacao;
        }


      /// <summary>
        /// Popula os dicionários contendo os dados sobre as despesas e a quantidade de pesquisadores de forma a não causar erros no código
        /// Isso nunca deveria ter sido feito...
        /// </summary>
        /// <param name="anoFim"></param>
        private static void GambiarraAnoInvalido(string anoFim)
        {
            int anoFimParse = int.Parse(anoFim);
            if (!despesas.ContainsKey(anoFimParse))
            {
                despesas[anoFimParse] = 0;
            }

            if (!pesquisadores.ContainsKey(int.Parse(anoFim)))
            {
                try
                {
                    pesquisadores[anoFimParse] = pesquisadores[anoFimParse - 1];
                }
                // Em ultimo caso, o ano anterior também não existe (2023 não está lá no momento)
                catch (KeyNotFoundException)
                {
                    pesquisadores[anoFimParse - 1] = pesquisadores[anoFimParse - 2];
                    pesquisadores[anoFimParse] = pesquisadores[anoFimParse - 2];
                }
            }
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

        private List<Prospeccao> GetProspeccoesUsuarioLider(Usuario usuario)
        {
            return _prospeccoes.Where(p => p.Usuario.Id == usuario.Id && p.Status.OrderBy(f => f.Data).LastOrDefault().Status != StatusProspeccao.Planejada).ToList();
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
        /// Retorna o número médio de pesquisadores presentes em um determinado período
        /// </summary>
        /// <param name="anoInicial"></param>
        /// <param name="anoFinal"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal double CalculoNumeroPesquisadores(int anoInicial, int anoFinal)
        {
            double qtdPesquisadores = 0;
            if (anoInicial > anoFinal)
            {
                throw new ArgumentException($"{nameof(anoInicial)} não pode ser maior que {nameof(anoFinal)}");
            }

            if (anoInicial == anoFinal)
            {
                return pesquisadores[anoFinal];
            }
            else
            {
                int index = 0;

                for (int i = anoInicial; i <= anoFinal; i++)
                {
                    qtdPesquisadores += pesquisadores[i];
                    index++;
                }

                qtdPesquisadores /= index;
                return qtdPesquisadores;
            }
        }

        private static void HandleMesFimAnoFimInvalido(ref string mesFim, ref string anoFim)
        {
            // Ano/Mes nulo?
            mesFim ??= DateTime.Now.Month.ToString();
            anoFim ??= DateTime.Now.Year.ToString();
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
        /// Obtem a lista de projetos do usuário passado por parâmetro
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private async Task<List<Projeto>> GetProjetosUsuario(Usuario usuario)
        {
            return await _context.Projeto.Where(p => p.EquipeProjeto.Any(e => e.Usuario == usuario) && p.Usuario == usuario).ToListAsync();
        }

        private List<Usuario> TratarMembrosEquipeString(Prospeccao prospeccao)
        {
            List<string> membrosNaoTratados = prospeccao.MembrosEquipe?.Split(";").ToList();
            List<Usuario> usuarios = _context.Users.ToList();
            List<Usuario> membrosEquipe = new List<Usuario>();

            if (membrosNaoTratados != null)
            {
                foreach (var membro in membrosNaoTratados)
                {
                    if (!string.IsNullOrEmpty(membro))
                    {
                        Usuario usuarioEquivalente = usuarios.Find(u => u.Email == membro);
                        if (usuarioEquivalente != null)
                        {
                            membrosEquipe.Add(usuarioEquivalente);
                        }
                    }
                }
            }

            return membrosEquipe;
        }

        /// <summary>
        /// Extrai o valor total das prospecções considerando o valor da proposta se presente, se não, o valor estimado
        /// </summary>
        /// <param name="prospeccoes"></param>
        /// <param name="valorTotalProspeccoes"></param>
        /// <returns></returns>
        private decimal ExtrairValorProspeccoes(Usuario usuario, StatusProspeccao? statusProspeccao, string mesInicio, string anoInicio, string mesFim, string anoFim)
        {
            decimal valorProspeccoes = 0;
            List<Prospeccao> prospeccoes = new List<Prospeccao>();

            if (statusProspeccao != null)
            {
                prospeccoes = _prospeccoes.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == statusProspeccao).ToList();
                prospeccoes = FiltrarProspeccoesPorPeriodo(mesInicio, anoInicio, mesFim, anoFim, prospeccoes);
            }
            else
            {
                prospeccoes = _prospeccoes.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status != StatusProspeccao.Planejada).ToList();
                prospeccoes = FiltrarProspeccoesPorPeriodo(mesInicio, anoInicio, mesFim, anoFim, prospeccoes);
            }

            foreach (var prospeccao in prospeccoes)
            {
                // Verificar se a prospecção tem como líder o usuário passado ou se a prospecção tem como membro o usuário passado
                if (prospeccao.Usuario.Id == usuario.Id || (prospeccao.MembrosEquipe?.Contains(usuario.Email) ?? false))
                {
                    List<Usuario> membrosProspeccao = TratarMembrosEquipeString(prospeccao);
                    // TODO: Remover hardcoding no futuro!!
                    int qtdBolsistas = membrosProspeccao.Where(m => m.Cargo?.Nome == nomeCargoBolsista).Count();
                    int qtdPesquisadores = membrosProspeccao.Where(m => m.Cargo?.Nome == nomeCargoPesquisador).Count();
                    int qtdEstagiarios = membrosProspeccao.Where(m => m.Cargo?.Nome == nomeCargoEstagiário).Count();

                    decimal percentualPesquisador = CalculoPercentualPesquisador(1 + membrosProspeccao.Count, qtdPesquisadores);
                    decimal percentualBolsista = CalculoPercentualBolsista(1 + membrosProspeccao.Count, qtdBolsistas);
                    decimal percentualEstagiario = CalculoPercentualEstagiario(1 + membrosProspeccao.Count, qtdEstagiarios);
                    decimal percentualLider = 1 - (percentualBolsista + percentualPesquisador + percentualEstagiario);

                    decimal percentualTotalPesquisador = percentualPesquisador * qtdPesquisadores;
                    decimal percentualTotalBolsistas = percentualBolsista * qtdBolsistas;
                    decimal percentualTotalEstagiarios = percentualEstagiario * qtdEstagiarios;

                    // Se for lider...
                    if (prospeccao.Usuario.Id == usuario.Id)
                    {
                        valorProspeccoes += percentualLider * prospeccao.ValorProposta;
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
        /// Filtra as prospecções passadas por parâmetro com status do mês e ano inicial até o mês e ano final
        /// </summary>
        /// <returns></returns>
        private static List<Prospeccao> FiltrarProspeccoesPorPeriodo(DateTime dataInicio, DateTime dataFim, List<Prospeccao> prospeccoes)
        {
            return prospeccoes
                .Where(p => p.Status.Any(f => f.Data >= dataInicio && f.Data <= dataFim))
                .ToList();
        }


        /// <summary>
        /// Filtra os projetos passados por parâmetro com status do mês e ano inicial até o mês e ano final
        /// </summary>
        /// <param name="mesInicio"></param>
        /// <param name="anoInicio"></param>
        /// <param name="mesFim"></param>
        /// <param name="anoFim"></param>
        /// <param name="projetos"></param>
        /// <returns></returns>
        private static List<Projeto> FiltrarProjetosPorPeriodo(string mesInicio, string anoInicio, string mesFim, string anoFim, List<Projeto> projetos)
        {
            int yearInicio = int.Parse(anoInicio);
            int monthInicio = int.Parse(mesInicio);

            int yearFim = int.Parse(anoFim);
            int monthFim = int.Parse(mesFim);

            DateTime dataInicioFiltro = new DateTime(yearInicio, monthInicio, 1);
            DateTime dataFimFiltro = new DateTime(yearFim, monthFim, DateTime.DaysInMonth(yearFim, monthFim));

            return projetos
                .Where(p => p.DataInicio <= dataFimFiltro && p.DataEncerramento >= dataInicioFiltro)
                .ToList();
        }

        /// <summary>
        /// Filtrar os projetos passados por parâmetro até a data de encerramento
        /// </summary>
        /// <param name="mesFim"></param>
        /// <param name="anoFim"></param>
        /// <param name="projetosUsuario"></param>
        /// <returns></returns>
        private List<Projeto> FiltrarProjetosPorPeriodo(string mesFim, string anoFim, List<Projeto> projetos)
        {
            int yearFim = int.Parse(anoFim);
            int monthFim = int.Parse(mesFim);

            DateTime dataFiltro = new DateTime(yearFim, monthFim, DateTime.DaysInMonth(yearFim, monthFim));

            return projetos
                .Where(p => dataFiltro <= p.DataEncerramento)
                .ToList();
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
                p.MembrosEquipe != null &&
                p.MembrosEquipe.Contains(usuario.Email) &&
                (p.Status == null || p.Status.OrderBy(f => f.Data).LastOrDefault()?.Status != StatusProspeccao.Planejada)
            ).ToList();

        }

        /// <summary>
        /// Realiza a atribuição de uma participação de acordo com as prospecções de um usuário.
        /// </summary>
        /// <param name="participacao">Objeto para as participação total de um usuário (ou genérico)</param>
        /// <param name="prospeccoesUsuario">Prospecções de um usuário (membro e lider)</param>
        private async Task AtribuirParticipacoesIndividuais(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
        {
            List<Usuario> usuarios = await _context.Users.ToListAsync();

            foreach (var prospeccao in prospeccoesUsuario)
            {
                List<Usuario> membrosEquipe = new List<Usuario>();

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

                List<string> membrosNaoTratados = new List<string>();

                if (!string.IsNullOrEmpty(prospeccao.MembrosEquipe))
                {
                    membrosNaoTratados = prospeccao.MembrosEquipe.Split(";").ToList();

                    foreach (var membro in membrosNaoTratados)
                    {
                        if (!string.IsNullOrEmpty(membro))
                        {
                            Usuario usuarioEquivalente = usuarios.Find(u => u.Email == membro);
                            if (usuarioEquivalente != null)
                            {
                                membrosEquipe.Add(usuarioEquivalente);
                            }
                        }
                    }

                    qtdBolsistas = membrosEquipe.Count(u => u.Cargo?.Nome == nomeCargoBolsista); // TODO: Temporário, precisa estar definido de forma mais clara?
                    qtdEstagiarios = membrosEquipe.Count(u => u.Cargo?.Nome == nomeCargoEstagiário); // TODO: Temporário, precisa estar definido de forma mais clara?
                    qtdPesquisadores = membrosEquipe.Count(u => u.Cargo?.Nome == nomeCargoPesquisador); // TODO: Temporário, precisa estar definido de forma mais clara?

                    qtdMembros = qtdBolsistas + qtdEstagiarios + qtdPesquisadores;
                }

                Dictionary<string, decimal> calculoParticipantes = CalculoParticipacao(qtdPesquisadores, qtdBolsistas, qtdEstagiarios);

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
                    MembrosEquipe = prospeccao.MembrosEquipe,
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
        /// Obtém uma lista de participações de todos os usuários, com base na casa do usuário que está acessando.
        /// </summary>
        /// <returns></returns>
        private async Task<List<ParticipacaoTotalViewModel>> GetParticipacoesTotaisUsuarios(DateTime dataInicio, DateTime dataFim)
        {
            // TODO: Isso precisava mesmo estar aqui?
            ViewbagizarUsuario(_context);

            List<Usuario> usuarios;

            if (UsuarioAtivo.Casa == Instituto.ISIQV || UsuarioAtivo.Casa == Instituto.CISHO)
            {
                usuarios = await _context.Users.Where(u => (u.Casa == Instituto.ISIQV || u.Casa == Instituto.CISHO) && u.Cargo.Nome == nomeCargoPesquisador && u.EmailConfirmed == true && u.Nivel == Nivel.Usuario).ToListAsync();
            }
            else
            {
                usuarios = await _context.Users.Where(u => u.Casa == UsuarioAtivo.Casa && u.EmailConfirmed == true && u.Cargo.Nome == nomeCargoPesquisador && u.Nivel == Nivel.Usuario).ToListAsync();
            }

            List<ParticipacaoTotalViewModel> participacoes = new List<ParticipacaoTotalViewModel>();

            foreach (var usuario in usuarios)
            {
                var participacao = await GetParticipacaoTotalUsuario(usuario, mesInicio, anoInicio, mesFim, anoFim);

                if (participacao != null)
                {
                    participacoes.Add(participacao);
                }
            }

            return participacoes;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? dataInicio, DateTime? dataFim)
        {
            ViewbagizarUsuario(_context);

            var indicadores = await _context.IndicadoresFinanceiros.ToListAsync();

            foreach (var indicador in indicadores)
            {
                despesas[indicador.Data.Year] = indicador.Despesa;
                pesquisadores[indicador.Data.Year] = indicador.QtdPesquisadores;
            }

            if (dataInicio != null && dataFim != null)
            {
                ViewData["mesInicio"] = dataInicio.Value.Month;
                ViewData["anoInicio"] = dataInicio.Value.Year;
                ViewData["mesFim"] = dataFim.Value.Month;
                ViewData["anoFim"] = dataFim.Value.Year;
            }
            else
            {
                ViewData["mesInicio"] = 01;
                ViewData["anoInicio"] = 2021;
                ViewData["mesFim"] = 12;
                ViewData["anoFim"] = DateTime.Now.Year;
                dataInicio = new DateTime(2021, 01, 21);
                dataFim = new DateTime(DateTime.Now.Year, 12, 31);
            }

            _prospeccoes = await _cache.GetCachedAsync("Prospeccoes:Participacao", () => _context.Prospeccao.Include(p => p.Usuario).Include(p => p.Empresa).ToListAsync());

            string chaveCache = $"Participacoes:{dataInicio.Value.Month}:{dataInicio.Value.Year}:{dataFim.Value.Month}:{dataFim.Value.Year}";
            var participacoes = await _cache.GetCachedAsync(chaveCache, () => GetParticipacoesTotaisUsuarios((DateTime)dataInicio, (DateTime)dataFim));
            participacoes = PrepararDadosParticipacao(participacoes);

            if (UsuarioAtivo.Nivel == Nivel.Dev || UsuarioAtivo.Nivel == Nivel.PMO)
            {
                return View(participacoes);
            }
            else
            {
                var participacoesFiltradas = participacoes.Where(p => p.Lider.Id == UsuarioAtivo.Id).ToList();
                return View(participacoesFiltradas);
            }

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

        /// <summary>
        /// Cálculo o rank médio de todos os atributos de participacação total
        /// </summary>
        /// <param name="participacoes"></param>
        /// <returns></returns>
        private List<decimal> ObterRankingsMedios(List<ParticipacaoTotalViewModel> participacoes)
        {
            List<decimal> rankings = new List<decimal>();

            decimal rankMedio = participacoes.Average(p => p.MediaFatores);
            decimal rankMedioValorTotalProspeccao = participacoes.Average(p => p.RankPorIndicador["RankValorTotalProspeccoes"]);
            decimal rankMedioValorTotalProspeccoesComProposta = participacoes.Average(p => p.RankPorIndicador["RankValorTotalProspeccoesComProposta"]);
            decimal rankMedioValorMedioProspeccoes = participacoes.Average(p => p.RankPorIndicador["RankValorMedioProspeccoes"]);
            decimal rankMedioValorMedioProspeccoesComProposta = participacoes.Average(p => p.RankPorIndicador["RankValorMedioProspeccoesComProposta"]);
            decimal rankMedioValorTotalProspeccoesConvertidas = participacoes.Average(p => p.RankPorIndicador["RankValorTotalProspeccoesConvertidas"]);
            decimal rankMedioValorMedioProspeccoesConvertidas = participacoes.Average(p => p.RankPorIndicador["RankValorMedioProspeccoesConvertidas"]);
            decimal rankMedioQuantidadeProspeccoes = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoes"]);
            decimal rankMedioQuantidadeProspeccoesLider = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoesLider"]);
            decimal rankMedioQuantidadeProspeccoesComProposta = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoesComProposta"]);
            decimal rankMedioQuantidadeProspeccoesProjeto = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoesProjeto"]);
            decimal rankMedioQuantidadeProspeccoesMembro = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoesMembro"]);
            decimal rankMedioAssertividadePrecificacao = participacoes.Average(p => p.RankPorIndicador["RankAssertividadePrecificacao"]);
            decimal rankMedioFatorContribuicaoFinanceira = participacoes.Average(p => p.RankPorIndicador["RankFatorContribuicaoFinanceira"]);

            rankings.AddRange(new List<decimal> {
                rankMedio,
                rankMedioValorTotalProspeccao,
                rankMedioValorTotalProspeccoesComProposta,
                rankMedioValorMedioProspeccoes,
                rankMedioValorMedioProspeccoesComProposta,
                rankMedioValorTotalProspeccoesConvertidas,
                rankMedioQuantidadeProspeccoes,
                rankMedioQuantidadeProspeccoesLider,
                rankMedioQuantidadeProspeccoesComProposta,
                rankMedioQuantidadeProspeccoesProjeto,
                rankMedioQuantidadeProspeccoesMembro,
                rankMedioAssertividadePrecificacao,
                rankMedioFatorContribuicaoFinanceira,
            });

            ViewData[nameof(rankMedio)] = rankMedio;
            ViewData[nameof(rankMedioValorTotalProspeccao)] = rankMedioValorTotalProspeccao;
            ViewData[nameof(rankMedioValorTotalProspeccoesComProposta)] = rankMedioValorTotalProspeccoesComProposta;
            ViewData[nameof(rankMedioValorMedioProspeccoes)] = rankMedioValorMedioProspeccoes;
            ViewData[nameof(rankMedioValorMedioProspeccoesComProposta)] = rankMedioValorMedioProspeccoesComProposta;
            ViewData[nameof(rankMedioValorTotalProspeccoesConvertidas)] = rankMedioValorTotalProspeccoesConvertidas;
            ViewData[nameof(rankMedioValorTotalProspeccoesConvertidas)] = rankMedioValorMedioProspeccoesConvertidas;
            ViewData[nameof(rankMedioQuantidadeProspeccoes)] = rankMedioQuantidadeProspeccoes;
            ViewData[nameof(rankMedioQuantidadeProspeccoesLider)] = rankMedioQuantidadeProspeccoesLider;
            ViewData[nameof(rankMedioQuantidadeProspeccoesComProposta)] = rankMedioQuantidadeProspeccoesComProposta;
            ViewData[nameof(rankMedioQuantidadeProspeccoesProjeto)] = rankMedioQuantidadeProspeccoesProjeto;
            ViewData[nameof(rankMedioQuantidadeProspeccoesMembro)] = rankMedioQuantidadeProspeccoesMembro;
            ViewData[nameof(rankMedioAssertividadePrecificacao)] = rankMedioAssertividadePrecificacao;
            ViewData[nameof(rankMedioFatorContribuicaoFinanceira)] = rankMedioFatorContribuicaoFinanceira;

            return rankings;
        }

        /// <summary>
        /// Ajusta o valor do FCF de acordo com a média de todas as participacões para cada participação
        /// </summary>
        /// <param name="participacoes"></param>
        private void CalcularValorSobreMediaDoFCF(List<ParticipacaoTotalViewModel> participacoes)
        {
            decimal mediaFatorContribuicaoFinanceira = participacoes.Average(p => p.FatorContribuicaoFinanceira);

            foreach (var participacao in participacoes)
            {
                if (mediaFatorContribuicaoFinanceira != 0)
                {
                    participacao.FatorContribuicaoFinanceira /= mediaFatorContribuicaoFinanceira;
                }
                else
                {
                    participacao.FatorContribuicaoFinanceira = 0;
                }
            }
        }

        /// <summary>
        /// Atribui os rankings as participações passadas por parâmetro, para que sejam exibidas na View. Valores de 0 a 1
        /// </summary>
        /// <param name="participacoes">Lista de participações (normalmente de um usuário específico mas pode ser genérica)</param>
        private static void RankearParticipacoes(List<ParticipacaoTotalViewModel> participacoes, bool rankPorMaximo)
        {
            decimal RankValorTotalProspeccoesComProposta = 0;
            decimal RankValorTotalProspeccoesConvertidas = 0;
            decimal RankValorMedioProspeccoesComProposta = 0;
            decimal RankValorMedioProspeccoesConvertidas = 0;
            decimal RankQuantidadeProspeccoesProjeto = 0;
            decimal RankQuantidadeProspeccoes = 0;
            decimal RankQuantidadeProspeccoesLider = 0;
            decimal RankQuantidadeProspeccoesMembro = 0;
            decimal RankAssertividadePrecificacao = 0;
            decimal RankFatorContribuicaoFinanceira = 0;

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
            decimal medQtdProspProjetizadas = participacoes.Average(p => p.QuantidadeProspeccoesProjeto);
            decimal medConversaoProjeto = participacoes.Average(p => p.TaxaConversaoProjeto);
            decimal medConversaoProposta = participacoes.Average(p => p.TaxaConversaoProposta);
            decimal medFatorContribuicaoFinanceira = participacoes.Average(p => p.FatorContribuicaoFinanceira);

            foreach (var participacao in participacoes)
            {
                participacao.RankPorIndicador = new Dictionary<string, decimal>();

                decimal RankQuantidadeProspeccoesComProposta = 0;
                decimal RankValorTotalProspeccoes = 0;
                decimal RankValorMedioProspeccoes = 0;

                CalcularMediaFatores(participacoes, participacao);

                RankValorTotalProspeccoes = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoes, participacoes.Max(p => p.ValorTotalProspeccoes)) : IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoes, medValorTotalProsp);
                RankValorMedioProspeccoes = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoes, participacoes.Max(p => p.ValorMedioProspeccoes)) : IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoes, medValorMedioProsp);
                RankValorTotalProspeccoesComProposta = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesComProposta, participacoes.Max(p => p.ValorTotalProspeccoesComProposta)) : IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesComProposta, medTotalProspComProposta);
                RankValorTotalProspeccoesConvertidas = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesConvertidas, participacoes.Max(p => p.ValorTotalProspeccoesConvertidas)) : IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesConvertidas, medTotalProspConvertidas);
                RankValorMedioProspeccoesComProposta = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoesComProposta, participacoes.Max(p => p.ValorMedioProspeccoesComProposta)) : IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoesComProposta, medValorMedioProspComProposta);
                RankValorMedioProspeccoesConvertidas = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoesConvertidas, participacoes.Max(p => p.ValorMedioProspeccoesConvertidas)) : IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoesConvertidas, medValorMedioProspConvertidas);
                RankQuantidadeProspeccoes = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoes, participacoes.Max(p => p.QuantidadeProspeccoes)) : IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoes, medQtdProspeccoes);
                RankQuantidadeProspeccoesLider = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesLider, participacoes.Max(p => p.QuantidadeProspeccoes)) : IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesLider, medQtdProspeccoes);
                RankQuantidadeProspeccoesProjeto = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesProjeto, participacoes.Max(p => p.QuantidadeProspeccoesProjeto)) : IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesProjeto, medQtdProspProjetizadas);
                RankQuantidadeProspeccoesComProposta = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesComProposta, participacoes.Max(p => p.QuantidadeProspeccoesComProposta)) : IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesComProposta, medQtdProspeccoesComProposta);
                RankQuantidadeProspeccoesMembro = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesMembro, participacoes.Max(p => p.QuantidadeProspeccoesMembro)) : IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesMembro, medQtdProspeccoesMembro);
                RankAssertividadePrecificacao = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.AssertividadePrecificacao, participacoes.Max(p => p.AssertividadePrecificacao)) : IndicadorHelper.DivisaoSegura(participacao.AssertividadePrecificacao, participacoes.Average(p => p.AssertividadePrecificacao));
                RankFatorContribuicaoFinanceira = rankPorMaximo ? IndicadorHelper.DivisaoSegura(participacao.FatorContribuicaoFinanceira, participacoes.Max(p => p.FatorContribuicaoFinanceira)) : IndicadorHelper.DivisaoSegura(participacao.FatorContribuicaoFinanceira, participacoes.Average(p => p.FatorContribuicaoFinanceira));

                participacao.RankPorIndicador[nameof(RankValorTotalProspeccoes)] = RankValorTotalProspeccoes;
                participacao.RankPorIndicador[nameof(RankValorTotalProspeccoesComProposta)] = RankValorTotalProspeccoesComProposta;
                participacao.RankPorIndicador[nameof(RankValorTotalProspeccoesConvertidas)] = RankValorTotalProspeccoesConvertidas;
                participacao.RankPorIndicador[nameof(RankValorMedioProspeccoes)] = RankValorMedioProspeccoes;
                participacao.RankPorIndicador[nameof(RankValorMedioProspeccoesComProposta)] = RankValorMedioProspeccoesComProposta;
                participacao.RankPorIndicador[nameof(RankValorMedioProspeccoesConvertidas)] = RankValorMedioProspeccoesConvertidas;
                participacao.RankPorIndicador[nameof(RankQuantidadeProspeccoes)] = RankQuantidadeProspeccoes;
                participacao.RankPorIndicador[nameof(RankQuantidadeProspeccoesLider)] = RankQuantidadeProspeccoesLider;
                participacao.RankPorIndicador[nameof(RankQuantidadeProspeccoesComProposta)] = RankQuantidadeProspeccoesComProposta;
                participacao.RankPorIndicador[nameof(RankQuantidadeProspeccoesProjeto)] = RankQuantidadeProspeccoesProjeto;
                participacao.RankPorIndicador[nameof(RankQuantidadeProspeccoesMembro)] = RankQuantidadeProspeccoesMembro;
                participacao.RankPorIndicador[nameof(RankAssertividadePrecificacao)] = RankAssertividadePrecificacao;
                participacao.RankPorIndicador[nameof(RankFatorContribuicaoFinanceira)] = RankFatorContribuicaoFinanceira;
            }
        }

        /// <summary>
        /// Realiza o cálculo da média dos fatores
        /// </summary>
        /// <param name="participacoes"></param>
        /// <param name="participacao"></param>
        private static void CalcularMediaFatores(List<ParticipacaoTotalViewModel> participacoes, ParticipacaoTotalViewModel participacao)
        {
            decimal calculoMediaFatores = 0;
            
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoes, participacoes.Max(p => p.ValorTotalProspeccoes));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoes, participacoes.Max(p => p.ValorMedioProspeccoes));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoes, participacoes.Max(p => p.QuantidadeProspeccoes));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesLider, participacoes.Max(p => p.QuantidadeProspeccoesLider));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesProjeto, participacoes.Max(p => p.QuantidadeProspeccoesProjeto));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesComProposta, participacoes.Max(p => p.QuantidadeProspeccoesComProposta));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoesComProposta, participacoes.Max(p => p.ValorMedioProspeccoesComProposta));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.TaxaConversaoProjeto, participacoes.Max(p => p.TaxaConversaoProjeto));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.TaxaConversaoProposta, participacoes.Max(p => p.TaxaConversaoProposta));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesMembro, participacoes.Max(p => p.QuantidadeProspeccoesMembro));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.AssertividadePrecificacao, participacoes.Max(p => p.AssertividadePrecificacao));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoesConvertidas, participacoes.Max(p => p.ValorMedioProspeccoesConvertidas));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesConvertidas, participacoes.Max(p => p.ValorTotalProspeccoesConvertidas));

            participacao.MediaFatores = calculoMediaFatores /= 13;
        }
    }
}