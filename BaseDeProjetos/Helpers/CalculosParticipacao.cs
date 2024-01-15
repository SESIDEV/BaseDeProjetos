using BaseDeProjetos.Controllers;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseDeProjetos.Helpers
{
    public class CalculosParticipacao
    {
        private const string nomeCargoBolsista = "Pesquisador Bolsista";
        private const string nomeCargoEstagiário = "Estagiário";

        // TODO: Precisamos não utilizar esses valores mágicos de string no futuro!!
        private const string nomeCargoPesquisador = "Pesquisador QMS";

        /// <summary>
        /// Efetua o cálculo das despesas do ISI com base no período selecionado
        /// </summary>
        /// <param name="mesInicial"></param>
        /// <param name="anoInicial"></param>
        /// <param name="mesFinal"></param>
        /// <param name="anoFinal"></param>
        /// <returns></returns>
        internal static decimal CalculoDespesa(DateTime dataInicio, DateTime dataFim, Dictionary<int, decimal> despesas)
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
        /// Retorna o número médio de pesquisadores presentes em um determinado período
        /// </summary>
        /// <param name="anoInicial"></param>
        /// <param name="anoFinal"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        internal double CalculoNumeroPesquisadores(int anoInicial, int anoFinal, Dictionary<int, int> pesquisadores)
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


        /// <summary>
        /// Realiza o cálculo da média dos fatores
        /// </summary>
        /// <param name="participacoes"></param>
        /// <param name="participacao"></param>
        public static void CalcularMediaFatores(List<ParticipacaoTotalViewModel> participacoes, ParticipacaoTotalViewModel participacao)
        {
            decimal calculoMediaFatores = 0;

            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoes, participacoes.Max(p => p.ValorTotalProspeccoes));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.ValorMedioProspeccoes, participacoes.Max(p => p.ValorMedioProspeccoes));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoes, participacoes.Max(p => p.QuantidadeProspeccoes));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesLider, participacoes.Max(p => p.QuantidadeProspeccoesLider));
            calculoMediaFatores += IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesConvertidas, participacoes.Max(p => p.QuantidadeProspeccoesConvertidas));
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

        /// <summary>
        /// Calcula a quantidade de prospeccoes com proposta de acordo com um usuario e suas prospeccoes
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="prospeccoesUsuario"></param>
        /// <returns></returns>
        public static decimal CalcularQuantidadeDeProspeccoesComProposta(Usuario usuario, ProspeccoesUsuarioParticipacao prospeccoesUsuario, ApplicationDbContext _context)
        {
            decimal quantidadeProspeccoesComProposta = 0;

            foreach (var prospeccao in prospeccoesUsuario.ProspeccoesTotais)
            {
                if (prospeccao.Status.Any(f => f.Status == StatusProspeccao.ComProposta || f.Status == StatusProspeccao.Convertida))
                {
                    List<Usuario> membrosEquipe = prospeccao.EquipeProspeccao.Select(e => e.Usuario).ToList();

                    if (prospeccao.Usuario.Id == usuario.Id)
                    {
                        var percBolsista = CalculoPercentualBolsista(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoBolsista).Count());
                        var percEstagiario = CalculoPercentualEstagiario(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoEstagiário).Count());
                        var percPesquisador = CalculoPercentualPesquisador(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoPesquisador).Count());

                        var percLider = 1 - (percBolsista + percEstagiario + percPesquisador);

                        quantidadeProspeccoesComProposta += percLider;
                    }
                    else if (membrosEquipe.Any(u => u.Id == usuario.Id))
                    {
                        quantidadeProspeccoesComProposta += CalculoPercentualPesquisador(membrosEquipe.Count() + 1, membrosEquipe.Where(m => m.Cargo?.Nome == nomeCargoPesquisador).Count());
                    }
                }
            }

            return Math.Round(quantidadeProspeccoesComProposta);
        }

        /// <summary>
        /// Calcula a quantidade de prospeccoes convertidas de acordo com um usuario e suas prospeccoes
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="prospeccoesUsuario"></param>
        /// <returns></returns>
        public static decimal CalcularQuantidadeDeProspeccoesConvertidas(Usuario usuario, ProspeccoesUsuarioParticipacao prospeccoesUsuario)
        {
            decimal quantidadeProspeccoesConvertidas = 0;

            foreach (var prospeccao in prospeccoesUsuario.ProspeccoesTotais)
            {
                if (prospeccao.Status.Any(f => f.Status == StatusProspeccao.Convertida))
                {
                    if (prospeccao.EquipeProspeccao != null)
                    {
                        List<Usuario> membrosEquipe = prospeccao.EquipeProspeccao.Select(e => e.Usuario).ToList();

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
                }
            }

            return quantidadeProspeccoesConvertidas;
        }

        /// <summary>
        /// Ajusta o valor do FCF de acordo com a média de todas as participacões para cada participação
        /// </summary>
        /// <param name="participacoes"></param>
        public static void CalcularValorSobreMediaDoFCF(List<ParticipacaoTotalViewModel> participacoes)
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
        /// Efetua o cálculo relativo à participação de cada tipo de membro de acordo com a quantidade de membros de uma equipe
        /// </summary>
        /// <param name="qtdPesquisadores">Número de Pesquisadores no Projeto</param>
        /// <param name="qtdBolsistas">Número de Bolsistas no Projeto</param>
        /// <param name="qtdEstagiarios">Número de Estagiários no Projeto</param>
        /// <returns></returns>
        public static Dictionary<string, decimal> CalculoParticipacao(int qtdPesquisadores, int qtdBolsistas, int qtdEstagiarios)
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
        /// Método para cálculo da porcentagem relativa ao bolsista
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto</param>
        /// <param name="bolsistas">Número de bolsistas no projeto</param>
        /// <returns></returns>
        public static decimal CalculoPercentualBolsista(decimal numeroMembros, decimal bolsistas)
        {
            if (bolsistas == 0)
            {
                return 0;
            }
            else
            {
                decimal resultado = (1 - 1 / numeroMembros) * 3 / 10 * (1 / (bolsistas + 1));
                return resultado;
            }
        }

        /// <summary>
        /// Método para cálculo da porcentagem relativa ao estagiário
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto</param>
        /// <param name="estagiarios">Número de estagiários no projeto</param>
        /// <returns></returns>
        public static decimal CalculoPercentualEstagiario(decimal numeroMembros, decimal estagiarios)
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
        /// Método para calculo da porcentagem relativa ao pesquisador
        /// </summary>
        /// <param name="numeroMembros">Número de membros no projeto (inclui o lider)</param>
        /// <param name="qtdPesquisadores">Número de pesquisadores no projeto</param>
        /// <returns></returns>
        public static decimal CalculoPercentualPesquisador(decimal numeroMembros, decimal qtdPesquisadores)
        {
            if (qtdPesquisadores == 0)
            {
                return 0;
            }
            else
            {
                decimal percentual = (1 - 1 / numeroMembros) * 3 / 5 * (1 / (qtdPesquisadores + 1));
                return percentual;
            }
        }
    }

}