using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    public class ParticipacaoController : Controller
    {
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;

		public ParticipacaoController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_context = context;
			_logger = logger;
		}

        /// <summary>
        /// Efetua o cálculo relativo à participação de cada tipo de membro de acordo com a quantidade de membros de uma equipe
        /// </summary>
        /// <param name="pesquisadores">Número de Pesquisadores no Projeto</param>
        /// <param name="bolsistas">Número de Bolsistas no Projeto</param>
        /// <param name="estagiarios">Número de Estagiários no Projeto</param>
        /// <returns></returns>
        private Dictionary<string,decimal> CalculoParticipacao(int pesquisadores, int bolsistas, int estagiarios)
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

            return new Dictionary<string, decimal> { 
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
        /// Obtém uma participação de acordo com um usuário específico.
        /// </summary>
        /// <param name="usuario">Usuário do sistema a ter participações retornadas</param>
        /// <returns></returns>
        private async Task<ParticipacaoTotalViewModel> GetParticipacaoTotalUsuario(Usuario usuario, string mes, string ano)
        {
            ParticipacaoTotalViewModel participacao = new ParticipacaoTotalViewModel() { Participacoes = new List<ParticipacaoViewModel>() };

            List<Prospeccao> prospeccoesUsuario = await _context.Prospeccao.Where(p => p.Usuario == usuario).ToListAsync();
            List<Prospeccao> prospeccoesUsuarioComProposta = await _context.Prospeccao.Where(p => p.Usuario == usuario && p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ComProposta).ToListAsync();
            List<Prospeccao> prospeccoesUsuarioConvertidas = await _context.Prospeccao.Where(p => p.Usuario == usuario && p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida).ToListAsync();

            // Filtro, ano necessáriamente precisa estar preenchido para periodização
            if (!string.IsNullOrEmpty(ano))
            {
                // Filtrar somente ano
                if (string.IsNullOrEmpty(mes))
                {
                    prospeccoesUsuario = prospeccoesUsuario.Where(p => p.Status.Any(f => f.Data.Year == int.Parse(ano))).ToList();
                    prospeccoesUsuarioComProposta = prospeccoesUsuarioComProposta.Where(p => p.Status.Any(f => f.Data.Year == int.Parse(ano))).ToList();
                    prospeccoesUsuarioConvertidas = prospeccoesUsuarioConvertidas.Where(p => p.Status.Any(f => f.Data.Year == int.Parse(ano))).ToList();
                }
                else // Filtrar também o mês
                {
                    prospeccoesUsuario = prospeccoesUsuario.Where(p => p.Status.Any(f => f.Data.Year == int.Parse(ano) && f.Data.Month == int.Parse(mes))).ToList();
                    prospeccoesUsuarioComProposta = prospeccoesUsuarioComProposta.Where(p => p.Status.Any(f => f.Data.Year == int.Parse(ano) && f.Data.Month == int.Parse(mes))).ToList();
                    prospeccoesUsuarioConvertidas = prospeccoesUsuarioConvertidas.Where(p => p.Status.Any(f => f.Data.Year == int.Parse(ano) && f.Data.Month == int.Parse(mes))).ToList();
                } 
            }

            AtribuirParticipacoesIndividuais(participacao, prospeccoesUsuario);

            decimal valorTotalProspeccoes;
            decimal valorTotalProspeccoesComProposta;
            decimal valorMedioProspeccoes;
            decimal valorMedioProspeccoesComProposta;
            decimal taxaConversaoProposta;
            decimal taxaConversaoProjeto;
            int quantidadeProspeccoes;
            int quantidadeProspeccoesComProposta;
            int quantidadeProspeccoesProjetizadas;

            participacao.ValorTotalProspeccoes = valorTotalProspeccoes = prospeccoesUsuario.Sum(p => p.ValorProposta);
            participacao.ValorTotalProspeccoesComProposta = valorTotalProspeccoesComProposta = prospeccoesUsuarioComProposta.Sum(p => p.ValorProposta);
            participacao.QuantidadeProspeccoes = quantidadeProspeccoes = prospeccoesUsuario.Count();
            participacao.QuantidadeProspeccoesComProposta = quantidadeProspeccoesComProposta = prospeccoesUsuarioComProposta.Count();
            participacao.QuantidadeProspeccoesProjetizadas = quantidadeProspeccoesProjetizadas = prospeccoesUsuarioConvertidas.Count();
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
                participacao.TaxaConversaoProposta = taxaConversaoProposta = (quantidadeProspeccoesComProposta / (decimal)quantidadeProspeccoes) * 100;
                participacao.ValorMedioProspeccoes = valorMedioProspeccoes = valorTotalProspeccoes / quantidadeProspeccoes;

                if (quantidadeProspeccoesComProposta > 0)
                {
                    participacao.ValorMedioProspeccoesComProposta = valorMedioProspeccoesComProposta = valorTotalProspeccoesComProposta / quantidadeProspeccoesComProposta;
                }      
                
                participacao.TaxaConversaoProjeto = taxaConversaoProjeto = (quantidadeProspeccoesProjetizadas / (decimal)quantidadeProspeccoes) * 100;
                participacao.Rank = valorTotalProspeccoes + valorMedioProspeccoes + quantidadeProspeccoes + quantidadeProspeccoesProjetizadas;
            }

            return participacao;
        }

        /// <summary>
        /// Realiza a atribuição de uma participação de acordo com as prospecções de um usuário.
        /// </summary>
        /// <param name="participacao">Objeto para as participação total de um usuário (ou genérico)</param>
        /// <param name="prospeccoesUsuario">Prospecções de um usuário</param>
        private void AtribuirParticipacoesIndividuais(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
        {
            foreach (var prospeccao in prospeccoesUsuario)
            {
                bool prospConvertida = false;
                bool prospPlanejada = false;
                bool prospSuspensa = false;
                bool prospNaoConvertida = false;
                bool prospEmDiscussao = false;
                bool prospComProposta = false;

                string nomeProjeto = !string.IsNullOrEmpty(prospeccao.NomeProspeccao) ? prospeccao.NomeProspeccao : $"{prospeccao.Empresa.Nome} (Empresa)";

                // Tratar prospecções que tem "projeto" no nome (...)
                // i.e: Remover na hora de apresentar o nome casos em que temos "Projeto projeto XYZ"
                // TODO: Transformar em REGEX
                if (!string.IsNullOrEmpty(nomeProjeto) && nomeProjeto.ToLowerInvariant().Contains("projeto") && !nomeProjeto.ToLowerInvariant().Contains("projetos"))
                {
                    nomeProjeto = nomeProjeto.Replace("projeto", "");
                    nomeProjeto = nomeProjeto.Replace("Projeto", "");
                }

                // ---- TEMP ----

                int tempMax = 10;
                Random rnd = new Random();
                int pesqBolsTemp = rnd.Next(0, tempMax + 1);
                int estagTemp = rnd.Next(0, tempMax - pesqBolsTemp);
                int pesqNormTemp = 0;
                try
                {
                    pesqNormTemp = rnd.Next(0, pesqBolsTemp - estagTemp);
                }
                catch
                {
                    pesqNormTemp = 0;
                }

                // ---- TEMP ----

                Dictionary<string, decimal> calculoParticipantes = CalculoParticipacao(pesqNormTemp, pesqBolsTemp, estagTemp);

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
                    QuantidadeBolsistas = pesqBolsTemp,
                    QuantidadeEstagiarios = estagTemp,
                    QuantidadePesquisadores = pesqNormTemp,
                    QuantidadeMembros = pesqBolsTemp + estagTemp + pesqNormTemp + 1 // 1 == Líder
                });
            }
        }

        /// <summary>
        /// Obtém uma lista de participações de todos os usuários, com base na casa do usuário que está acessando.
        /// </summary>
        /// <returns></returns>
        private async Task<List<ParticipacaoTotalViewModel>> GetParticipacoesTotaisUsuarios(string mes, string ano)
		{
            Usuario usuarioAtivo = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
            List<Usuario> usuarios;

			if (usuarioAtivo.Casa == Instituto.ISIQV || usuarioAtivo.Casa == Instituto.CISHO)
            {
				usuarios = await _context.Users.Where(u => u.Casa == Instituto.ISIQV || u.Casa == Instituto.CISHO).ToListAsync();
			}
            else
            {
                usuarios = await _context.Users.Where(u => u.Casa == usuarioAtivo.Casa).ToListAsync();
			}

			List<ParticipacaoTotalViewModel> participacoes = new List<ParticipacaoTotalViewModel>();
			
			foreach (var usuario in usuarios)
			{

                var participacao = await GetParticipacaoTotalUsuario(usuario, mes, ano);
				participacoes.Add(participacao);
			}

			return participacoes;
		}

        // TODO: Filtrar por ANO, MÊS
		[HttpGet]
		public async Task<IActionResult> Index(string mes, string ano)
        {
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            var participacoes = await GetParticipacoesTotaisUsuarios(mes, ano);

            if (participacoes.Count > 0)
            {
                RankearParticipacoes(participacoes);
                participacoes = participacoes.OrderByDescending(p => p.Rank).ToList();
            }            

			ViewBag.usuarioFoto = usuario.Foto;
			ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

            return View(participacoes);
        }

        /// <summary>
        /// Atribui os rankings as participações passadas por parâmetro, para que sejam exibidas na View. Valores de 0 a 1, multiplicados por 100.
        /// </summary>
        /// <param name="participacoes">Lista de participações (normalmente de um usuário específico mas pode ser genérica)</param>
        private static void RankearParticipacoes(List<ParticipacaoTotalViewModel> participacoes)
        {
            decimal rankValorTotalProspeccoesComProposta = 0;
            decimal rankValorMedioProspeccoesComProposta = 0;

            decimal maxTotalProsp = participacoes.Max(p => p.ValorTotalProspeccoes);
            decimal maxValorMedioProsp = participacoes.Max(p => p.ValorMedioProspeccoes);
            decimal maxValorMedioProspComProposta = participacoes.Max(p => p.ValorMedioProspeccoesComProposta);
            decimal maxTotalProspComProposta = participacoes.Max(p => p.ValorTotalProspeccoesComProposta);
            decimal maxQtdProspeccoes = participacoes.Max(p => p.QuantidadeProspeccoes);
            decimal maxQtdProspeccoesComProposta = participacoes.Max(p => p.QuantidadeProspeccoesComProposta);
            decimal maxQtdProspProjetizadas = participacoes.Max(p => p.QuantidadeProspeccoesProjetizadas);
            decimal maxConversaoProjeto = participacoes.Max(p => p.TaxaConversaoProjeto);
            decimal maxConversaoProposta = participacoes.Max(p => p.TaxaConversaoProposta);

            foreach (var participacao in participacoes)
            {
                participacao.RankPorIndicador = new Dictionary<string, decimal>();

                decimal calculoRank = 0;
                decimal rankQuantidadeProspeccoesComProposta = 0;

                if (maxTotalProsp != 0)
                {
                    calculoRank += participacao.ValorTotalProspeccoes / maxTotalProsp;
                }

                if (maxValorMedioProsp != 0)
                {
                    calculoRank += participacao.ValorMedioProspeccoes / maxValorMedioProsp;
                }
                if (maxQtdProspeccoes != 0)
                {
                    calculoRank += participacao.QuantidadeProspeccoes / maxQtdProspeccoes;
                }
                if (maxQtdProspProjetizadas != 0)
                {
                    calculoRank += participacao.QuantidadeProspeccoesProjetizadas / maxQtdProspProjetizadas;
                }
                if (maxConversaoProjeto != 0)
                {
                    calculoRank += participacao.TaxaConversaoProjeto / maxConversaoProjeto;
                }
                if (maxConversaoProposta != 0)
                {
                    calculoRank += participacao.TaxaConversaoProposta / maxConversaoProposta;
                }

                calculoRank = calculoRank / 6;

                participacao.Rank = calculoRank;

                decimal rankValorTotalProspeccoes = participacao.ValorTotalProspeccoes / maxTotalProsp;
                decimal rankValorMedioProspeccoes = participacao.ValorMedioProspeccoes / maxValorMedioProsp;

                if (maxTotalProspComProposta != 0)
                {
                    rankValorTotalProspeccoesComProposta = participacao.ValorTotalProspeccoesComProposta / maxTotalProspComProposta;
                }
                
                if (maxValorMedioProspComProposta != 0)
                {
                    rankValorMedioProspeccoesComProposta = participacao.ValorTotalProspeccoesComProposta / maxValorMedioProspComProposta;
                }
                
                decimal rankQuantidadeProspeccoes = participacao.QuantidadeProspeccoes / maxQtdProspeccoes;
                decimal rankQuantidadeProspeccoesProjetizadas = participacao.QuantidadeProspeccoesProjetizadas / maxQtdProspProjetizadas;

                if (maxQtdProspeccoesComProposta != 0)
                {
                    rankQuantidadeProspeccoesComProposta = participacao.QuantidadeProspeccoesComProposta / maxQtdProspeccoesComProposta;
                }

                participacao.RankPorIndicador["RankValorTotalProspeccoes"] = rankValorTotalProspeccoes;
                participacao.RankPorIndicador["RankValorTotalProspeccoesComProposta"] = rankValorTotalProspeccoesComProposta;
                participacao.RankPorIndicador["RankValorMedioProspeccoes"] = rankValorMedioProspeccoes;
                participacao.RankPorIndicador["RankValorMedioProspeccoesComProposta"] = rankValorMedioProspeccoesComProposta;
                participacao.RankPorIndicador["RankQuantidadeProspeccoes"] = rankQuantidadeProspeccoes;
                participacao.RankPorIndicador["RankQuantidadeProspeccoesComProposta"] = rankQuantidadeProspeccoesComProposta;
                participacao.RankPorIndicador["RankQuantidadeProspeccoesProjetizadas"] = rankQuantidadeProspeccoesProjetizadas;
            }
        }
    }
}
