using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Services
{
    public class ConstrutorParticipacao : IConstrutorParticipacao
    {
        // TODO: Precisamos não utilizar esses valores mágicos de string no futuro!!
        private const string nomeCargoBolsista = "Pesquisador Bolsista";
        private const string nomeCargoEstagiário = "Estagiário";
        private const string nomeCargoPesquisador = "Pesquisador QMS";

        private static readonly Dictionary<int, decimal> despesas = new Dictionary<int, decimal>();
        private readonly ApplicationDbContext _context;

        public ConstrutorParticipacao(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task AtribuirAssertividadePrecificacao(Usuario usuario, DateTime dataInicio, DateTime dataFim, ParticipacaoTotalViewModel participacao, ProspeccoesUsuarioParticipacao prospeccoesUsuario)
        {
            List<IndicadoresFinanceirosDTO> indicadores = await ObterIndicadoresFinanceirosParaParticipacao();

            foreach (var indicador in indicadores)
            {
                despesas[indicador.Data.Year] = indicador.Despesa;
            }

            // As prospecções são apenas do Líder para Assertividade
            var prospeccoesComProposta = prospeccoesUsuario.ProspeccoesLider.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ComProposta || p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida);
            var prospeccoesConvertidas = prospeccoesUsuario.ProspeccoesLider.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida);

            decimal quantidadeProspeccoesComProposta = prospeccoesComProposta.Count();
            decimal quantidadeProspeccoesConvertidas = prospeccoesConvertidas.Count();

            decimal totalValorPropostaComProposta = prospeccoesComProposta.Sum(p => p.ValorProposta);
            decimal totalValorEstimadoComProposta = prospeccoesComProposta.Sum(p => p.ValorEstimado);
            decimal totalValorPropostaConvertidas = prospeccoesConvertidas.Sum(p => p.ValorProposta);
            decimal totalValorEstimadoConvertidas = prospeccoesConvertidas.Sum(p => p.ValorEstimado);

            decimal valorMedioPropostaComProposta = IndicadorHelper.DivisaoSegura(totalValorPropostaComProposta, quantidadeProspeccoesComProposta);
            decimal valorMedioPropostaConvertidas = IndicadorHelper.DivisaoSegura(totalValorPropostaConvertidas, quantidadeProspeccoesConvertidas);
            decimal valorMedioEstimadoComProposta = IndicadorHelper.DivisaoSegura(totalValorEstimadoComProposta, quantidadeProspeccoesComProposta);
            decimal valorMedioEstimadoConvertidas = IndicadorHelper.DivisaoSegura(totalValorEstimadoConvertidas, quantidadeProspeccoesConvertidas);

            decimal mediaValoresEstimados = (valorMedioEstimadoComProposta + valorMedioEstimadoConvertidas) / 2;
            decimal mediaValoresReais = (valorMedioPropostaComProposta + valorMedioPropostaConvertidas) / 2;

            participacao.AssertividadePrecificacao = 1 - Math.Abs(IndicadorHelper.DivisaoSegura((mediaValoresEstimados - mediaValoresReais), mediaValoresReais));

            participacao.TaxaConversaoProposta = IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesComProposta, participacao.QuantidadeProspeccoes);
            participacao.TaxaConversaoProjeto = IndicadorHelper.DivisaoSegura(participacao.QuantidadeProspeccoesConvertidas, participacao.QuantidadeProspeccoesComProposta);

            decimal despesaIsiMeses = CalculosParticipacao.CalculoDespesa(dataInicio, dataFim, despesas);
            decimal valorTotalProjetosParaFCF = await CalculosParticipacao.ExtrairValorProjetos(usuario, dataInicio, dataFim, _context);

            // Prospecções convertidas / despesa
            participacao.FatorContribuicaoFinanceira = IndicadorHelper.DivisaoSegura(totalValorPropostaConvertidas, despesaIsiMeses);
        }

        /// <summary>
        /// Realiza a atribuição de uma participação de acordo com as prospecções de um usuário.
        /// </summary>
        /// <param name="participacao">Objeto para as participação total de um usuário (ou genérico)</param>
        /// <param name="prospeccoesUsuario">Prospecções de um usuário (membro e lider)</param>
        public async Task AtribuirParticipacoesIndividuais(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
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
                    ValorNominal = prospeccao.ValorEstimado != 0 ? prospeccao.ValorEstimado : prospeccao.ValorProposta,
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
        public void AtribuirQuantidadesDeProspeccao(Usuario usuario, ParticipacaoTotalViewModel participacao, ProspeccoesUsuarioParticipacao prospeccoesUsuario)
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
        /// <param name="participacao"></param>
        public void AtribuirValoresFinanceirosDeProspeccao(Usuario usuario, ParticipacaoTotalViewModel participacao, ProspeccoesUsuarioParticipacao prospeccoesUsuario)
        {
            participacao.ValorTotalProspeccoes = CalculosParticipacao.ExtrairValorProspeccoes(prospeccoesUsuario.ProspeccoesTotais, usuario);
            participacao.ValorTotalProspeccoesComProposta = CalculosParticipacao.ExtrairValorProspeccoes(prospeccoesUsuario.ProspeccoesTotaisComProposta, usuario);
            participacao.ValorTotalProspeccoesConvertidas = CalculosParticipacao.ExtrairValorProspeccoes(prospeccoesUsuario.ProspeccoesTotaisConvertidas, usuario);
            participacao.ValorMedioProspeccoes = IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoes, participacao.QuantidadeProspeccoes);
            participacao.ValorMedioProspeccoesComProposta = IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesComProposta, participacao.QuantidadeProspeccoesComProposta);
            participacao.ValorMedioProspeccoesConvertidas = IndicadorHelper.DivisaoSegura(participacao.ValorTotalProspeccoesConvertidas, Math.Ceiling(participacao.QuantidadeProspeccoesConvertidas));
        }

        private async Task<List<IndicadoresFinanceirosDTO>> ObterIndicadoresFinanceirosParaParticipacao()
        {
            return await _context.IndicadoresFinanceiros.Select(i => new IndicadoresFinanceirosDTO { Despesa = i.Despesa, QtdPesquisadores = i.QtdPesquisadores, Data = i.Data }).ToListAsync();
        }
    }
}
