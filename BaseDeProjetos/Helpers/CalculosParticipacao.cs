using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Helpers
{
    public class CalculosParticipacao
    {
        private const string nomeCargoBolsista = "Pesquisador Bolsista";
        private const string nomeCargoEstagiário = "Estagiário";

        // TODO: Precisamos não utilizar esses valores mágicos de string no futuro!!
        private const string nomeCargoPesquisador = "Pesquisador QMS";

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
        public static decimal CalcularQuantidadeDeProspeccoesComProposta(Usuario usuario, ProspeccoesUsuarioParticipacao prospeccoesUsuario)
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

            return Math.Ceiling(quantidadeProspeccoesConvertidas);
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

        /// <summary>
        /// TODO: SRP
        /// TODO: Apenas como lider??
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        /// <returns></returns>
        public static async Task<decimal> ExtrairValorProjetos(Usuario usuario, DateTime dataInicio, DateTime dataFim, ApplicationDbContext context)
        {
            var projetosUsuario = await context.Projeto.Where(p => p.UsuarioId == usuario.Id).ToListAsync();

            projetosUsuario = AcertarPrecificacaoProjetos(dataInicio, dataFim, projetosUsuario);
            return (decimal)projetosUsuario.Sum(p => p.ValorTotalProjeto);
        }

        /// <summary>
        /// Extrai o valor total das prospecções do usuário considerando o valor da proposta se presente, se não, o valor estimado
        /// </summary>
        /// <param name="prospeccoes"></param>
        /// <param name="valorTotalProspeccoes"></param>
        /// <returns></returns>
        public static decimal ExtrairValorProspeccoes(List<Prospeccao> prospeccoes, Usuario usuario)
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
                                valorProspeccoes += percentualEstagiario * (prospeccao.ValorProposta != 0 ? prospeccao.ValorProposta : prospeccao.ValorEstimado);
                                break;

                            case nomeCargoPesquisador:
                                valorProspeccoes += percentualPesquisador * (prospeccao.ValorProposta != 0 ? prospeccao.ValorProposta : prospeccao.ValorEstimado);
                                break;

                            case nomeCargoBolsista:
                                valorProspeccoes += percentualBolsista * (prospeccao.ValorProposta != 0 ? prospeccao.ValorProposta : prospeccao.ValorEstimado);
                                break;
                        }
                    }
                }
            }

            return valorProspeccoes;
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
        internal static List<Projeto> AcertarPrecificacaoProjetos(DateTime dataInicio, DateTime dataFim, List<Projeto> projetos)
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
        /// Reatribui os valores de um projeto de acordo com uma data inicial de filtro
        /// </summary>
        /// <param name="dataInicialFiltro"></param>
        /// <param name="projeto"></param>
        internal static void ReatribuirValorProjeto(DateTime dataInicialFiltro, Projeto projeto)
        {
            int qtdMeses = Helpers.DiferencaMeses(projeto.DataEncerramento, dataInicialFiltro, true);
            int diferencaMeses = Helpers.DiferencaMeses(projeto.DataEncerramento, projeto.DataInicio, true);
            double valorProjetoPorMes = projeto.ValorTotalProjeto / diferencaMeses;
            projeto.ValorTotalProjeto = valorProjetoPorMes * qtdMeses;
        }

        /// <summary>
        /// Reatribui os valores de um projeto de acordo com uma data final de filtro
        /// </summary>
        /// <param name="projeto"></param>
        /// <param name="dataFinalFiltro"></param>
        internal static void ReatribuirValorProjeto(Projeto projeto, DateTime dataFinalFiltro, DateTime? dataInicialFiltro = null)
        {
            int qtdMeses;
            if (dataFinalFiltro < projeto.DataInicio)
            {
                throw new ArgumentException($"{nameof(dataFinalFiltro)} não pode ser inferior a {nameof(projeto.DataInicio)}");
            }

            if (dataInicialFiltro != null)
            {
                qtdMeses = Helpers.DiferencaMeses(dataFinalFiltro, (DateTime)dataInicialFiltro, true);
            }
            else
            {
                qtdMeses = Helpers.DiferencaMeses(dataFinalFiltro, projeto.DataInicio, true);
            }

            int diferencaMeses = Helpers.DiferencaMeses(projeto.DataEncerramento, projeto.DataInicio, true);
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
        /// Atribui as propriedades de RankMedio de acordo com as participacoes (incluindo a do próprio usuario)
        /// </summary>
        /// <param name="participacao"></param>
        /// <param name="participacoes"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static void ObterRankingMedioPesquisador(ParticipacaoTotalViewModel participacao, List<ParticipacaoTotalViewModel> participacoes)
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

    }

}