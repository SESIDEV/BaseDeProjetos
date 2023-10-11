using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
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
    public class ParticipacaoController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        private readonly static Dictionary<int, decimal> despesas = new Dictionary<int, decimal>
        {
            { 2021, 290000M },
            { 2022, 400000M },
            { 2023, 440000M },
        };

        private readonly static Dictionary<int, int> pesquisadores = new Dictionary<int, int>
        {
            {2021, 20},
            {2022, 20},
            {2023, 23}
        };

        public ParticipacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Efetua o cálculo das despesas do ISI com base no período selecionado
        /// </summary>
        /// <param name="mesInicial"></param>
        /// <param name="anoInicial"></param>
        /// <param name="mesFinal"></param>
        /// <param name="anoFinal"></param>
        /// <returns></returns>
        internal static decimal CalculoDespesa(int mesInicial, int anoInicial, int mesFinal, int anoFinal)
        {
            if (anoInicial > anoFinal)
            {
                throw new ArgumentException($"{nameof(anoInicial)} não pode ser maior que {nameof(anoFinal)}");
            }

            decimal valorCustoFinal = 0;

            if (anoInicial == anoFinal)
            {
                if (mesInicial > mesFinal)
                {
                    throw new ArgumentException($"{nameof(mesInicial)} não pode ser maior que {nameof(mesFinal)}");
                }

                int quantidadeDeMesesAno = mesFinal - mesInicial + 1;
                valorCustoFinal = despesas[anoInicial] / 12 * quantidadeDeMesesAno;
                return valorCustoFinal;
            }
            else
            {
                int quantidadeDeMesesAnoInicial = 12 - mesInicial + 1;
                valorCustoFinal += despesas[anoInicial] / 12 * quantidadeDeMesesAnoInicial;

                int quantidadeDeMesesAnoFinal = mesFinal;
                valorCustoFinal += despesas[anoFinal] / 12 * quantidadeDeMesesAnoFinal;

                int subtracaoAno = anoFinal - anoInicial - 1;

                if (subtracaoAno <= 0)
                {
                    return valorCustoFinal;
                }
                else
                {
                    for (int i = 1; i <= subtracaoAno; i++)
                    {
                        valorCustoFinal += despesas[anoInicial + i];
                    }
                }

                return valorCustoFinal;
            }
        }

        /// <summary>
        /// Efetua o cálculo relativo à participação de cada tipo de membro de acordo com a quantidade de membros de uma equipe
        /// </summary>
        /// <param name="pesquisadores">Número de Pesquisadores no Projeto</param>
        /// <param name="bolsistas">Número de Bolsistas no Projeto</param>
        /// <param name="estagiarios">Número de Estagiários no Projeto</param>
        /// <returns></returns>
        private Dictionary<string, decimal> CalculoParticipacao(int pesquisadores, int bolsistas, int estagiarios)
        {
            int numeroMembros = 1 + pesquisadores + bolsistas + estagiarios;

            decimal valorPorPesquisador = CalculoValorPesquisador(numeroMembros, pesquisadores);
            decimal valorPorBolsista = CalculoValorBolsista(numeroMembros, bolsistas);
            decimal valorPorEstagiario = CalculoValorEstagiario(numeroMembros, estagiarios);

            decimal totalPesquisadores = valorPorPesquisador * pesquisadores;
            decimal totalBolsistas = valorPorBolsista * bolsistas;
            decimal totalEstagiarios = valorPorEstagiario * estagiarios;
            decimal totalLider = 1 - (totalBolsistas + totalEstagiarios + totalPesquisadores);

            //decimal totalProjeto = totalLider + totalBolsistas + totalEstagiarios + totalPesquisadores;

            return new Dictionary<string, decimal>
            {
                { "totalPesquisadores", totalPesquisadores },
                { "totalBolsistas", totalBolsistas },
                { "totalEstagiarios", totalEstagiarios },
                { "totalLider", totalLider },
                { "valorPorPesquisador", valorPorPesquisador },
                { "valorPorBolsista", valorPorBolsista },
                { "valorPorEstagiario", valorPorEstagiario },
            };
        }

        /// <summary>
        /// Método para cálculo da porcentagem relativa ao estagiário
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto</param>
        /// <param name="estagiarios">Número de estagiários no projeto</param>
        /// <returns></returns>
        private decimal CalculoValorEstagiario(decimal numeroMembros, decimal estagiarios)
        {
            if (estagiarios == 0)
            {
                return 0;
            }
            else
            {
                decimal resultado = (1 - 1 / numeroMembros) * 1 / 10 * (1 / (estagiarios + 1));
                return resultado;
            }
        }

        /// <summary>
        /// Método para cálculo da porcentagem relativa ao bolsista
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto</param>
        /// <param name="bolsistas">Número de bolsistas no projeto</param>
        /// <returns></returns>
        private decimal CalculoValorBolsista(decimal numeroMembros, decimal bolsistas)
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
        /// <param name="numeroMembros">Número de membros no projeto</param>
        /// <param name="pesquisadores">Número de pesquisadores no projeto</param>
        /// <returns></returns>
        private decimal CalculoValorPesquisador(decimal numeroMembros, decimal pesquisadores)
        {
            if (pesquisadores == 0)
            {
                return 0;
            }
            else
            {
                decimal resultado = (1 - (1 / numeroMembros)) * 3 / 5 * (1 / (pesquisadores + 1));
                return resultado;
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
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var participacoes = await GetParticipacoesTotaisUsuarios();
                Dictionary<string, object> dadosGrafico = new Dictionary<string, object>();
                List<decimal> rankingsMedios = new List<decimal>();

                if (participacoes.Count > 0)
                {
                    RankearParticipacoes(participacoes, true);
                    CalcularValorSobreMediaDoFCF(participacoes);
                    rankingsMedios = ObterRankingsMedios(participacoes);
                }

                var participacaoUsuario = participacoes.FirstOrDefault(p => p.Lider == usuario);

                dadosGrafico["Participacao"] = participacaoUsuario;
                dadosGrafico["Rankings"] = rankingsMedios;

                if (usuario != null)
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
        private async Task<List<Prospeccao>> GetProspeccoesUsuario(Usuario usuario)
        {
            return await _context.Prospeccao.Where(p => p.Usuario == usuario || p.MembrosEquipe.Contains(usuario.UserName)).ToListAsync();
        }

        /// <summary>
        /// Percorre o range de data inicio até data fim e cria valores e labels para o gráfico de prospeccoes
        /// </summary>
        /// <param name="participacao"></param>
        /// <param name="prospeccoesUsuario"></param>
        private void ComputarRangeGraficoParticipacao(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
        {
            DateTime dataInicial = prospeccoesUsuario.Min(p => p.Status.Min(f => f.Data));
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

        /// <summary>
        /// Obtém uma participação de acordo com um usuário específico.
        /// </summary>
        /// <param name="usuario">Usuário do sistema a ter participações retornadas</param>
        /// <returns></returns>
        private async Task<ParticipacaoTotalViewModel> GetParticipacaoTotalUsuario(Usuario usuario, string mesInicio = null, string anoInicio = null, string mesFim = null, string anoFim = null)
        {
            decimal despesaIsiMeses = 0;
            double quantidadePesquisadores = 0;

            ParticipacaoTotalViewModel participacao = new ParticipacaoTotalViewModel() { Participacoes = new List<ParticipacaoViewModel>() };
            List<Projeto> projetosUsuarioEmExecucaoFiltrados = new List<Projeto>();

            // Líder e Membro
            var prospeccoesUsuario = await GetProspeccoesUsuario(usuario);
            var prospeccoesUsuarioMembro = await GetProspeccoesUsuarioMembro(usuario);

            var projetosUsuario = await GetProjetosUsuario(usuario);
            var projetosUsuarioMembro = await GetProjetosUsuarioMembro(usuario);

            // Evitar exibir usuários sem prospecção
            if (prospeccoesUsuario.Count == 0)
            {
                return null;
            }
            else
            {
                ComputarRangeGraficoParticipacao(participacao, prospeccoesUsuario);
            }

            var prospeccoesUsuarioComProposta = prospeccoesUsuario.Where(p => p.Status.Any(f => f.Status == StatusProspeccao.ComProposta)).ToList();
            var prospeccoesUsuarioConvertidas = prospeccoesUsuario.Where(p => p.Status.Any(f => f.Status == StatusProspeccao.Convertida)).ToList();
            var projetosUsuarioEmExecucao = projetosUsuario.Where(p => p.Status == StatusProjeto.EmExecucao).ToList();

            if (!string.IsNullOrEmpty(anoFim) && !string.IsNullOrEmpty(mesFim))
            {
                if (!string.IsNullOrEmpty(mesInicio) && !string.IsNullOrEmpty(anoInicio))
                {
                    prospeccoesUsuario = FiltrarProspeccoesPorPeriodo(mesInicio, anoInicio, mesFim, anoFim, prospeccoesUsuario);
                    prospeccoesUsuarioComProposta = FiltrarProspeccoesPorPeriodo(mesInicio, anoInicio, mesFim, anoFim, prospeccoesUsuarioComProposta);
                    prospeccoesUsuarioConvertidas = FiltrarProspeccoesPorPeriodo(mesInicio, anoInicio, mesFim, anoFim, prospeccoesUsuarioConvertidas);
                    projetosUsuarioEmExecucao = FiltrarProjetosPorPeriodo(mesInicio, anoInicio, mesFim, anoFim, projetosUsuario);
                    projetosUsuarioEmExecucaoFiltrados = AcertarPrecificacaoProjetos(mesInicio, anoInicio, mesFim, anoFim, projetosUsuarioEmExecucao);
                }
                else
                {
                    prospeccoesUsuario = FiltrarProspeccoesPorPeriodo(mesFim, anoFim, prospeccoesUsuario);
                    prospeccoesUsuarioComProposta = FiltrarProspeccoesPorPeriodo(mesFim, anoFim, prospeccoesUsuarioComProposta);
                    prospeccoesUsuarioConvertidas = FiltrarProspeccoesPorPeriodo(mesFim, anoFim, prospeccoesUsuarioConvertidas);
                    projetosUsuarioEmExecucao = FiltrarProjetosPorPeriodo(mesFim, anoFim, projetosUsuario);
                    projetosUsuarioEmExecucaoFiltrados = AcertarPrecificacaoProjetos(mesFim, anoFim, projetosUsuarioEmExecucao);
                }
            }

            // Evitar exibir usuários sem prospecção
            if (prospeccoesUsuario.Count == 0)
            {
                return null;
            }

            await AtribuirParticipacoesIndividuais(participacao, prospeccoesUsuario);

            decimal valorTotalProspeccoes = 0;
            decimal valorTotalProspeccoesComProposta = 0;
            decimal valorMedioProspeccoes = 0;
            decimal valorMedioProspeccoesComProposta = 0;
            decimal valorMedioProspeccoesConvertidas = 0;
            decimal valorTotalProspeccoesConvertidas = 0;
            decimal taxaConversaoProposta;
            decimal taxaConversaoProjeto;
            int quantidadeProspeccoes;
            int quantidadeProspeccoesMembro;
            int quantidadeProspeccoesComProposta;
            int quantidadeProspeccoesConvertidas;

            participacao.ValorTotalProspeccoes = valorTotalProspeccoes = ExtrairValorProspeccoes(prospeccoesUsuario);
            participacao.ValorTotalProspeccoesComProposta = valorTotalProspeccoesComProposta = ExtrairValorProspeccoes(prospeccoesUsuarioComProposta);
            participacao.ValorTotalProspeccoesConvertidas = valorTotalProspeccoesConvertidas = ExtrairValorProspeccoes(prospeccoesUsuarioConvertidas);

            participacao.QuantidadeProspeccoes = quantidadeProspeccoes = prospeccoesUsuario.Count();
            participacao.QuantidadeProspeccoesComProposta = quantidadeProspeccoesComProposta = prospeccoesUsuarioComProposta.Count();
            participacao.QuantidadeProspeccoesProjeto = quantidadeProspeccoesConvertidas = prospeccoesUsuarioConvertidas.Count();
            participacao.QuantidadeProspeccoesMembro = quantidadeProspeccoesMembro = prospeccoesUsuarioMembro.Count();

            participacao.Lider = usuario;

            // Evita divisão por 0
            if (prospeccoesUsuario.Count() == 0)
            {
                participacao.ValorMedioProspeccoes = 0;
                participacao.ValorMedioProspeccoesComProposta = 0;
                participacao.TaxaConversaoProposta = 0;
                participacao.TaxaConversaoProjeto = 0;
            }
            else
            {
                participacao.TaxaConversaoProposta = taxaConversaoProposta = quantidadeProspeccoesComProposta / (decimal)quantidadeProspeccoes;
                participacao.ValorMedioProspeccoes = valorMedioProspeccoes = valorTotalProspeccoes / quantidadeProspeccoes;

                if (quantidadeProspeccoesComProposta > 0)
                {
                    participacao.ValorMedioProspeccoesComProposta = valorMedioProspeccoesComProposta = valorTotalProspeccoesComProposta / quantidadeProspeccoesComProposta;
                }

                if (quantidadeProspeccoesConvertidas > 0)
                {
                    participacao.ValorMedioProspeccoesConvertidas = valorMedioProspeccoesConvertidas = valorTotalProspeccoesConvertidas / quantidadeProspeccoesConvertidas;
                }

                if (valorMedioProspeccoesConvertidas != 0)
                {
                    var calculoAbsoluto = Math.Abs((valorMedioProspeccoesConvertidas - valorMedioProspeccoesComProposta) / valorMedioProspeccoesComProposta);
                    // var calculoAbsoluto = ((valorMedioProspeccoesComProposta - valorMedioProspeccoesConvertidas) / valorMedioProspeccoesConvertidas);
                    if (calculoAbsoluto != 0)
                    {
                        participacao.AssertividadePrecificacao = 1 / calculoAbsoluto;
                    }
                }

                if (quantidadeProspeccoesComProposta != 0)
                {
                    participacao.TaxaConversaoProjeto = taxaConversaoProjeto = quantidadeProspeccoesConvertidas / (decimal)quantidadeProspeccoesComProposta;
                }
                else
                {
                    participacao.TaxaConversaoProjeto = taxaConversaoProjeto = 0;
                }

                decimal somaProjetosProspeccoesUsuario = prospeccoesUsuarioConvertidas.Sum(p => p.ValorProposta) + (decimal)projetosUsuarioEmExecucao.Sum(p => p.ValorTotalProjeto);

                if (mesInicio == null || anoInicio == null)
                {
                    mesInicio = "01";
                    anoInicio = "2021";
                }

                HandleMesFimAnoFimInvalido(ref mesFim, ref anoFim);

                despesaIsiMeses = CalculoDespesa(int.Parse(mesInicio), int.Parse(anoInicio), int.Parse(mesFim), int.Parse(anoFim));

                quantidadePesquisadores = CalculoNumeroPesquisadores(int.Parse(anoInicio), int.Parse(anoFim));

                participacao.FatorContribuicaoFinanceira = somaProjetosProspeccoesUsuario / despesaIsiMeses / (decimal)quantidadePesquisadores;
            }

            return participacao;
        }

        /// <summary>
        /// Retorna uma lista de projetos com o seu valor ajustado para o custo de acordo com o filtro
        /// (Apenas considera uma data limite)
        /// </summary>
        /// <param name="mesFim">Mes de fim do filtro</param>
        /// <param name="anoFim">Ano de fim do filtro</param>
        /// <param name="projetos">Projetos a serem ajustados</param>
        /// <returns></returns>
        private List<Projeto> AcertarPrecificacaoProjetos(string mesFim, string anoFim, List<Projeto> projetos)
        {
            int qtdMesesCalculavel = 0;
            var dataFinalFiltro = Helpers.Helpers.ObterUltimoDiaMes(int.Parse(anoFim), int.Parse(mesFim));

            foreach (var projeto in projetos)
            {
                if (dataFinalFiltro > projeto.DataInicio)
                {
                    if (dataFinalFiltro < projeto.DataEncerramento)
                    {
                        qtdMesesCalculavel = Helpers.Helpers.DiferencaMeses(dataFinalFiltro, projeto.DataInicio);
                        double valorProjetoPorMes = projeto.ValorTotalProjeto / Helpers.Helpers.DiferencaMeses(projeto.DataEncerramento, projeto.DataInicio);
                        projeto.ValorTotalProjeto = valorProjetoPorMes * qtdMesesCalculavel;
                    }
                }
                else
                {
                    // Não considerar o custo caso o filtro esteja fora?
                    projeto.ValorTotalProjeto = 0;
                }
            }

            return projetos;
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
        private List<Projeto> AcertarPrecificacaoProjetos(string mesInicio, string anoInicio, string mesFim, string anoFim, List<Projeto> projetos)
        {
            var dataInicialFiltro = new DateTime(int.Parse(anoInicio), int.Parse(mesInicio), 1);
            var dataFinalFiltro = Helpers.Helpers.ObterUltimoDiaMes(int.Parse(anoFim), int.Parse(mesFim));

            foreach (var projeto in projetos)
            {
                if (dataInicialFiltro > projeto.DataEncerramento || dataFinalFiltro < projeto.DataInicio)
                {
                    projeto.ValorTotalProjeto = 0;
                }
                else
                {
                    if (dataFinalFiltro < projeto.DataEncerramento)
                    {
                        ReatribuirValorProjeto(projeto, dataFinalFiltro);
                    }
                    else
                    {
                        ReatribuirValorProjeto(dataInicialFiltro, projeto);
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
        internal void ReatribuirValorProjeto(Projeto projeto, DateTime dataFinalFiltro)
        {
            if (dataFinalFiltro > projeto.DataEncerramento)
            {
                throw new ArgumentException($"{nameof(dataFinalFiltro)} não pode ser superior a {nameof(projeto.DataEncerramento)}");
            }

            if (dataFinalFiltro < projeto.DataInicio)
            {
                throw new ArgumentException($"{nameof(dataFinalFiltro)} não pode ser inferior a {nameof(projeto.DataEncerramento)}");
            }

            int qtdMeses = Helpers.Helpers.DiferencaMeses(dataFinalFiltro, projeto.DataInicio, true);
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

        /// <summary>
        /// Extrai o valor total das prospecções considerando o valor da proposta se presente, se não, o valor estimado
        /// </summary>
        /// <param name="prospeccoes"></param>
        /// <param name="valorTotalProspeccoes"></param>
        /// <returns></returns>
        private static decimal ExtrairValorProspeccoes(List<Prospeccao> prospeccoes)
        {
            decimal valorTotalProspeccoes = 0;
            foreach (var prospeccao in prospeccoes)
            {
                if (prospeccao.ValorProposta != 0)
                {
                    valorTotalProspeccoes += prospeccao.ValorProposta;
                }
                else
                {
                    valorTotalProspeccoes += prospeccao.ValorEstimado;
                }
            }

            return valorTotalProspeccoes;
        }

        /// <summary>
        /// Filtra as prospecções passadas por parâmetro com status até o mês e ano especificado
        /// </summary>
        /// <param name="mesFim"></param>
        /// <param name="anoFim"></param>
        /// <param name="prospeccoes"></param>
        /// <returns></returns>
        private static List<Prospeccao> FiltrarProspeccoesPorPeriodo(string mesFim, string anoFim, List<Prospeccao> prospeccoes)
        {
            return prospeccoes.Where(p => p.Status.Any(f => f.Data.Year <= int.Parse(anoFim) && f.Data.Month <= int.Parse(mesFim))).ToList();
        }

        /// <summary>
        /// Filtra as prospecções passadas por parâmetro com status do mês e ano inicial até o mês e ano final
        /// </summary>
        /// <param name="mesInicio"></param>
        /// <param name="anoInicio"></param>
        /// <param name="mesFim"></param>
        /// <param name="anoFim"></param>
        /// <param name="prospeccoes"></param>
        /// <returns></returns>
        private static List<Prospeccao> FiltrarProspeccoesPorPeriodo(string mesInicio, string anoInicio, string mesFim, string anoFim, List<Prospeccao> prospeccoes)
        {
            return prospeccoes.Where(p => p.Status.Any(f => f.Data.Year >= int.Parse(anoInicio) && f.Data.Year <= int.Parse(anoFim) && f.Data.Month >= int.Parse(mesInicio) && f.Data.Month <= int.Parse(mesFim))).ToList();
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
        private async Task<List<Prospeccao>> GetProspeccoesUsuarioMembro(Usuario usuario)
        {
            // Somente membro
            return await _context.Prospeccao.Where(p => p.MembrosEquipe.Contains(usuario.UserName)).ToListAsync();
        }

        /// <summary>
        /// Realiza a atribuição de uma participação de acordo com as prospecções de um usuário.
        /// </summary>
        /// <param name="participacao">Objeto para as participação total de um usuário (ou genérico)</param>
        /// <param name="prospeccoesUsuario">Prospecções de um usuário</param>
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
                            Usuario usuarioEquivalente = usuarios.Find(u => u.UserName == membro);
                            if (usuarioEquivalente != null)
                            {
                                membrosEquipe.Add(usuarioEquivalente);
                            }
                        }
                    }

                    qtdBolsistas = membrosEquipe.Count(u => u.Cargo?.Nome == "Pesquisador Bolsista"); // TODO: Temporário, precisa estar definido de forma mais clara?
                    qtdEstagiarios = membrosEquipe.Count(u => u.Cargo?.Nome == "Estagiário"); // TODO: Temporário, precisa estar definido de forma mais clara?
                    qtdPesquisadores = membrosEquipe.Count(u => u.Cargo?.Nome == "Pesquisador QMS"); // TODO: Temporário, precisa estar definido de forma mais clara?

                    qtdMembros = qtdBolsistas + qtdEstagiarios + qtdPesquisadores;
                }

                Dictionary<string, decimal> calculoParticipantes = CalculoParticipacao(qtdPesquisadores, qtdBolsistas, qtdEstagiarios);

                var valorLider = calculoParticipantes["totalLider"] * prospeccao.ValorProposta;
                var valorPesquisadores = calculoParticipantes["totalPesquisadores"] * prospeccao.ValorProposta;
                var valorBolsistas = calculoParticipantes["totalBolsistas"] * prospeccao.ValorProposta;
                var valorEstagiarios = calculoParticipantes["totalEstagiarios"] * prospeccao.ValorProposta;
                var valorPorBolsista = calculoParticipantes["valorPorBolsista"] * prospeccao.ValorProposta;
                var valorPorPesquisador = calculoParticipantes["valorPorPesquisador"] * prospeccao.ValorProposta;
                var valorPorEstagiario = calculoParticipantes["valorPorEstagiario"] * prospeccao.ValorProposta;

                if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida)
                {
                    prospConvertida = true;
                }
                else if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Planejada)
                {
                    prospPlanejada = true;
                }
                else if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Suspensa)
                {
                    prospSuspensa = true;
                }
                else if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.NaoConvertida)
                {
                    prospNaoConvertida = true;
                }
                else if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ContatoInicial ||
                    prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Discussao_EsbocoProjeto)
                {
                    prospEmDiscussao = true;
                }
                else if (prospeccao.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ComProposta)
                {
                    prospComProposta = true;
                }
                else
                {
                    prospSuspensa = false;
                    prospConvertida = false;
                    prospPlanejada = false;
                    prospNaoConvertida = false;
                    prospEmDiscussao = false;
                    prospComProposta = false;
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
        private async Task<List<ParticipacaoTotalViewModel>> GetParticipacoesTotaisUsuarios(string mesInicio = null, string anoInicio = null, string mesFim = null, string anoFim = null)
        {
            Usuario usuarioAtivo = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
            List<Usuario> usuarios;

            // List<Prospeccao> prospeccoesTotais = _context.Prospeccao.ToList();

            if (usuarioAtivo.Casa == Instituto.ISIQV || usuarioAtivo.Casa == Instituto.CISHO)
            {
                usuarios = await _context.Users.Where(u => u.Casa == Instituto.ISIQV || u.Casa == Instituto.CISHO).ToListAsync();
            }
            else
            {
                usuarios = await _context.Users.Where(u => u.Casa == usuarioAtivo.Casa).ToListAsync();
            }

            // List<Prospeccao> prospeccoesUsuarios = prospeccoesTotais.Where(p => usuarios.Contains(p.Usuario)).ToList();

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
        public async Task<IActionResult> Index(string mesInicio, string anoInicio, string mesFim, string anoFim)
        {
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewData["mesInicio"] = mesInicio;
            ViewData["anoInicio"] = anoInicio;
            ViewData["mesFim"] = mesFim;
            ViewData["anoFim"] = anoFim;

            var participacoes = await GetParticipacoesTotaisUsuarios(mesInicio, anoInicio, mesFim, anoFim);

            if (participacoes.Count > 0)
            {
                RankearParticipacoes(participacoes, false);
                // CalcularValorSobreMediaDoFCF(participacoes);
                //ObterRankingsMedios(participacoes);
                DefinirValoresMinMax(participacoes);

                participacoes = participacoes.OrderByDescending(p => p.MediaFatores).ToList();
            }

            ViewBag.usuarioFoto = usuario.Foto;
            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;
            ViewBag.usuarioId = usuario.Id;

            return View(participacoes);
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
            decimal medQtdProspeccoesMembro = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesMembro);
            decimal medQtdProspeccoesComProposta = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesComProposta);
            decimal medQtdProspProjetizadas = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesProjeto);
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

                if (medValorTotalProsp != 0)
                {
                    RankValorTotalProspeccoes = rankPorMaximo ? participacao.ValorTotalProspeccoes / participacoes.Max(p => p.ValorTotalProspeccoes) : participacao.ValorTotalProspeccoes / medValorTotalProsp;
                }
                if (medValorMedioProsp != 0)
                {
                    RankValorMedioProspeccoes = rankPorMaximo ? participacao.ValorMedioProspeccoes / participacoes.Max(p => p.ValorMedioProspeccoes) : participacao.ValorMedioProspeccoes / medValorMedioProsp;
                }
                if (medTotalProspComProposta != 0)
                {
                    RankValorTotalProspeccoesComProposta = rankPorMaximo ? participacao.ValorTotalProspeccoesComProposta / participacoes.Max(p => p.ValorTotalProspeccoesComProposta) : participacao.ValorTotalProspeccoesComProposta / medTotalProspComProposta;
                }
                if (medTotalProspConvertidas != 0)
                {
                    RankValorTotalProspeccoesConvertidas = rankPorMaximo ? participacao.ValorTotalProspeccoesConvertidas / participacoes.Max(p => p.ValorTotalProspeccoesConvertidas) : participacao.ValorTotalProspeccoesConvertidas / medTotalProspConvertidas;
                }
                if (medValorMedioProspComProposta != 0)
                {
                    RankValorMedioProspeccoesComProposta = rankPorMaximo ? participacao.ValorMedioProspeccoesComProposta / participacoes.Max(p => p.ValorMedioProspeccoesComProposta) : participacao.ValorMedioProspeccoesComProposta / medValorMedioProspComProposta;
                }
                if (medValorMedioProspConvertidas != 0)
                {
                    RankValorMedioProspeccoesConvertidas = rankPorMaximo ? participacao.ValorMedioProspeccoesConvertidas / participacoes.Max(p => p.ValorMedioProspeccoesConvertidas) : participacao.ValorMedioProspeccoesConvertidas / medValorMedioProspConvertidas;
                }
                if (medQtdProspeccoes != 0)
                {
                    RankQuantidadeProspeccoes = rankPorMaximo ? participacao.QuantidadeProspeccoes / (decimal)participacoes.Max(p => p.QuantidadeProspeccoes) : participacao.QuantidadeProspeccoes / medQtdProspeccoes;
                }
                if (medQtdProspProjetizadas != 0)
                {
                    RankQuantidadeProspeccoesProjeto = rankPorMaximo ? participacao.QuantidadeProspeccoesProjeto / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesProjeto) : participacao.QuantidadeProspeccoesProjeto / medQtdProspProjetizadas;
                }
                if (medQtdProspeccoesComProposta != 0)
                {
                    RankQuantidadeProspeccoesComProposta = rankPorMaximo ? participacao.QuantidadeProspeccoesComProposta / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesComProposta) : participacao.QuantidadeProspeccoesComProposta / medQtdProspeccoesComProposta;
                }
                if (medQtdProspeccoesMembro != 0)
                {
                    RankQuantidadeProspeccoesMembro = rankPorMaximo ? participacao.QuantidadeProspeccoesMembro / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesMembro) : participacao.QuantidadeProspeccoesMembro / medQtdProspeccoesMembro;
                }
                if (participacoes.Average(p => p.AssertividadePrecificacao) != 0)
                {
                    RankAssertividadePrecificacao = rankPorMaximo ? participacao.AssertividadePrecificacao / participacoes.Max(p => p.AssertividadePrecificacao) : participacao.AssertividadePrecificacao / participacoes.Average(p => p.AssertividadePrecificacao);
                }
                if (medFatorContribuicaoFinanceira != 0)
                {
                    RankFatorContribuicaoFinanceira = rankPorMaximo ? participacao.FatorContribuicaoFinanceira / participacoes.Max(p => p.FatorContribuicaoFinanceira) : participacao.FatorContribuicaoFinanceira / participacoes.Average(p => p.FatorContribuicaoFinanceira);
                }

                participacao.RankPorIndicador[nameof(RankValorTotalProspeccoes)] = RankValorTotalProspeccoes;
                participacao.RankPorIndicador[nameof(RankValorTotalProspeccoesComProposta)] = RankValorTotalProspeccoesComProposta;
                participacao.RankPorIndicador[nameof(RankValorTotalProspeccoesConvertidas)] = RankValorTotalProspeccoesConvertidas;
                participacao.RankPorIndicador[nameof(RankValorMedioProspeccoes)] = RankValorMedioProspeccoes;
                participacao.RankPorIndicador[nameof(RankValorMedioProspeccoesComProposta)] = RankValorMedioProspeccoesComProposta;
                participacao.RankPorIndicador[nameof(RankValorMedioProspeccoesConvertidas)] = RankValorMedioProspeccoesConvertidas;
                participacao.RankPorIndicador[nameof(RankQuantidadeProspeccoes)] = RankQuantidadeProspeccoes;
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

            if (participacoes.Max(p => p.ValorTotalProspeccoes) != 0)
            {
                calculoMediaFatores += participacao.ValorTotalProspeccoes / participacoes.Max(p => p.ValorTotalProspeccoes);
            }
            if (participacoes.Max(p => p.ValorMedioProspeccoes) != 0)
            {
                calculoMediaFatores += participacao.ValorMedioProspeccoes / participacoes.Max(p => p.ValorMedioProspeccoes);
            }
            if ((decimal)participacoes.Max(p => p.QuantidadeProspeccoes) != 0)
            {
                calculoMediaFatores += participacao.QuantidadeProspeccoes / (decimal)participacoes.Max(p => p.QuantidadeProspeccoes);
            }
            if ((decimal)participacoes.Max(p => p.QuantidadeProspeccoesProjeto) != 0)
            {
                calculoMediaFatores += participacao.QuantidadeProspeccoesProjeto / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesProjeto);
            }
            if ((decimal)participacoes.Max(p => p.QuantidadeProspeccoesComProposta) != 0)
            {
                calculoMediaFatores += participacao.QuantidadeProspeccoesComProposta / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesComProposta);
            }
            if (participacoes.Max(p => p.ValorMedioProspeccoesComProposta) != 0)
            {
                calculoMediaFatores += participacao.ValorMedioProspeccoesComProposta / participacoes.Max(p => p.ValorMedioProspeccoesComProposta);
            }
            if (participacoes.Max(p => p.TaxaConversaoProjeto) != 0)
            {
                calculoMediaFatores += participacao.TaxaConversaoProjeto / participacoes.Max(p => p.TaxaConversaoProjeto);
            }
            if (participacoes.Max(p => p.TaxaConversaoProposta) != 0)
            {
                calculoMediaFatores += participacao.TaxaConversaoProposta / participacoes.Max(p => p.TaxaConversaoProposta);
            }
            if ((decimal)participacoes.Max(p => p.QuantidadeProspeccoesMembro) != 0)
            {
                calculoMediaFatores += participacao.QuantidadeProspeccoesMembro / (decimal)participacoes.Max(p => p.QuantidadeProspeccoesMembro);
            }
            if (participacoes.Max(p => p.AssertividadePrecificacao) != 0)
            {
                calculoMediaFatores += participacao.AssertividadePrecificacao / participacoes.Max(p => p.AssertividadePrecificacao);
            }
            if (participacoes.Max(p => p.ValorMedioProspeccoesConvertidas != 0))
            {
                calculoMediaFatores += participacao.ValorMedioProspeccoesConvertidas / participacoes.Max(p => p.ValorMedioProspeccoesConvertidas);
            }
            if (participacoes.Max(p => p.ValorTotalProspeccoesConvertidas != 0))
            {
                calculoMediaFatores += participacao.ValorTotalProspeccoesConvertidas / participacoes.Max(p => p.ValorTotalProspeccoesConvertidas);
            }

            participacao.MediaFatores = calculoMediaFatores /= 12;
        }
    }
}