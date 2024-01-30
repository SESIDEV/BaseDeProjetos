using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using BaseDeProjetos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private static readonly Dictionary<int, decimal> despesas = new Dictionary<int, decimal>();

        private readonly DbCache _cache;
        private readonly IAdministradorDadosParticipacao _adminDadosParticipacao;
        private readonly ApplicationDbContext _context;

        private List<Prospeccao> _prospeccoes = new List<Prospeccao>();


        public ParticipacaoController(ApplicationDbContext context, DbCache cache, IConstrutorParticipacao construtorParticipacao, IAdministradorDadosParticipacao administradorDadosParticipacao)
        {
            _context = context;
            _cache = cache;
            _adminDadosParticipacao = administradorDadosParticipacao;
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
            if (string.IsNullOrEmpty(nomeIndicador))
            {
                throw new ArgumentNullException("O nome do indicador jamais pode estar vazio");
            }

            dataInicio ??= new DateTime(2021, 01, 01);
            dataFim ??= new DateTime(DateTime.Now.Year, 12, 31);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context, _cache);

                List<IndicadoresFinanceirosDTO> indicadores = await ObterIndicadoresFinanceirosParaParticipacao();

                foreach (var indicador in indicadores)
                {
                    despesas[indicador.Data.Year] = indicador.Despesa;
                }

                if (_prospeccoes.Count == 0)
                {
                    _prospeccoes = await _adminDadosParticipacao.ObterProspeccoesParaParticipacao();
                }

                string chaveCache = $"Participacoes:{dataInicio.Value.Month}:{dataInicio.Value.Year}:{dataFim.Value.Month}:{dataFim.Value.Year}";
                List<ParticipacaoTotalViewModel> participacoes = await _cache.GetCachedAsync(chaveCache, () => _adminDadosParticipacao.GetParticipacoesTotaisUsuarios(UsuarioAtivo, (DateTime)dataInicio, (DateTime)dataFim));
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
                _prospeccoes = await _adminDadosParticipacao.ObterProspeccoesParaParticipacao();

                var participacoes = await _adminDadosParticipacao.GetParticipacoesTotaisUsuarios(UsuarioAtivo, (DateTime)dataInicio, (DateTime)dataFim);

                var participacao = participacoes.Where(p => p.Lider.Id == idUsuario).First();

                CalculosParticipacao.ObterRankingMedioPesquisador(participacao, participacoes);
                ComputarRangeGraficoParticipacao(participacao, _prospeccoes.Where(p => p.Usuario.Id == idUsuario || p.EquipeProspeccao.Any(e => e.Usuario.Id == idUsuario)).ToList());

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
        /// Obtém os rankings médios para serem utilizados no gráfico de aranha
        /// </summary>
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
                }

                _prospeccoes = await _adminDadosParticipacao.ObterProspeccoesParaParticipacao();

                var participacoes = await _adminDadosParticipacao.GetParticipacoesTotaisUsuarios(UsuarioAtivo,new DateTime(2021, 01, 01), new DateTime(DateTime.Now.Year, 12, 31));
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