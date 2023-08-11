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
        /// Obtém uma participação de acordo com um usuário específico.
        /// </summary>
        /// <param name="usuario">Usuário do sistema a ter participações retornadas</param>
        /// <returns></returns>
        private async Task<ParticipacaoTotalViewModel> GetParticipacaoTotalUsuario(Usuario usuario, string mesInicio, string anoInicio, string mesFim, string anoFim)
        {
            ParticipacaoTotalViewModel participacao = new ParticipacaoTotalViewModel() { Participacoes = new List<ParticipacaoViewModel>() };
            // Líder e Membro
            List<Prospeccao> prospeccoesUsuario = await _context.Prospeccao.Where(p => p.Usuario == usuario || p.MembrosEquipe.Contains(usuario.UserName)).ToListAsync();
            // Somente membro
            List<Prospeccao> prospeccoesUsuarioMembro = await _context.Prospeccao.Where(p => p.MembrosEquipe.Contains(usuario.UserName)).ToListAsync();


            // Evitar exibir usuários sem prospecção
            if (prospeccoesUsuario.Count == 0)
            {
                return null;
            }
            else
            {
                DateTime dataInicial = prospeccoesUsuario.Min(p => p.Status.Min(f => f.Data));
                DateTime dataIterada = dataInicial;
                DateTime dataAtual = DateTime.Now;
                List<string> mesAno = new List<string>();
                List<decimal> valoresProposta = new List<decimal>();

                while (dataIterada <= dataAtual)
                {
                    string dataFormatada = $"{dataIterada.ToString("MMM")} {dataIterada.Year}";
                    mesAno.Add(dataFormatada);
                    decimal valorSomado = prospeccoesUsuario.FindAll(p => p.Status.Any(f => f.Data.Year <= dataIterada.Year && f.Data.Month <= dataIterada.Month)).Sum(p => p.ValorProposta);
                    valoresProposta.Add(valorSomado);
                    dataIterada = dataIterada.AddMonths(1);
                }

                participacao.Valores = valoresProposta;
                participacao.Labels = mesAno;

                var x = JsonConvert.SerializeObject(valoresProposta);
                var y = JsonConvert.SerializeObject(mesAno);
            }

            List<Prospeccao> prospeccoesUsuarioComProposta = await _context.Prospeccao.Where(p => p.Usuario == usuario && p.Status.Any(f => f.Status == StatusProspeccao.ComProposta)).ToListAsync();
            List<Prospeccao> prospeccoesUsuarioConvertidas = await _context.Prospeccao.Where(p => p.Usuario == usuario && p.Status.Any(f => f.Status == StatusProspeccao.Convertida)).ToListAsync();

            if (!string.IsNullOrEmpty(anoFim) && !string.IsNullOrEmpty(mesFim))
            {
                if (!string.IsNullOrEmpty(mesInicio) && !string.IsNullOrEmpty(anoInicio))
                {
                    prospeccoesUsuario = prospeccoesUsuario.Where(p => p.Status.Any(f => f.Data.Year >= int.Parse(anoInicio) && f.Data.Year <= int.Parse(anoFim) && f.Data.Month >= int.Parse(mesInicio) && f.Data.Month <= int.Parse(mesFim))).ToList();
                    prospeccoesUsuarioComProposta = prospeccoesUsuarioComProposta.Where(p => p.Status.Any(f => f.Data.Year >= int.Parse(anoInicio) && f.Data.Year <= int.Parse(anoFim) && f.Data.Month >= int.Parse(mesInicio) && f.Data.Month <= int.Parse(mesFim))).ToList();
                    prospeccoesUsuarioConvertidas = prospeccoesUsuarioConvertidas.Where(p => p.Status.Any(f => f.Data.Year >= int.Parse(anoInicio) && f.Data.Year <= int.Parse(anoFim) && f.Data.Month >= int.Parse(mesInicio) && f.Data.Month <= int.Parse(mesFim))).ToList();
                }
                else
                {
                    prospeccoesUsuario = prospeccoesUsuario.Where(p => p.Status.Any(f => f.Data.Year <= int.Parse(anoFim) && f.Data.Month <= int.Parse(mesFim))).ToList();
                    prospeccoesUsuarioComProposta = prospeccoesUsuarioComProposta.Where(p => p.Status.Any(f => f.Data.Year <= int.Parse(anoFim) && f.Data.Month <= int.Parse(mesFim))).ToList();
                    prospeccoesUsuarioConvertidas = prospeccoesUsuarioConvertidas.Where(p => p.Status.Any(f => f.Data.Year <= int.Parse(anoFim) && f.Data.Month <= int.Parse(mesFim))).ToList();
                }
            }

            // Evitar exibir usuários sem prospecção
            if (prospeccoesUsuario.Count == 0)
            {
                return null;
            }

            await AtribuirParticipacoesIndividuais(participacao, prospeccoesUsuario);

            decimal valorTotalProspeccoes = 0;
            decimal valorTotalProspeccoesComProposta;
            decimal valorMedioProspeccoes;
            decimal valorMedioProspeccoesComProposta = 0;
            decimal taxaConversaoProposta;
            decimal taxaConversaoProjeto;
            int quantidadeProspeccoes;
            int quantidadeProspeccoesMembro;
            int quantidadeProspeccoesComProposta;
            int quantidadeProspeccoesProjetizadas;

            foreach (var prospeccao in prospeccoesUsuario)
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

            participacao.ValorTotalProspeccoes = valorTotalProspeccoes;
			participacao.ValorTotalProspeccoesComProposta = valorTotalProspeccoesComProposta = prospeccoesUsuarioComProposta.Sum(p => p.ValorProposta);
            participacao.QuantidadeProspeccoes = quantidadeProspeccoes = prospeccoesUsuario.Count();
            participacao.QuantidadeProspeccoesComProposta = quantidadeProspeccoesComProposta = prospeccoesUsuarioComProposta.Count();
            participacao.QuantidadeProspeccoesProjeto = quantidadeProspeccoesProjetizadas = prospeccoesUsuarioConvertidas.Count();
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
                participacao.TaxaConversaoProposta = taxaConversaoProposta = (quantidadeProspeccoesComProposta / (decimal)quantidadeProspeccoes);
                participacao.ValorMedioProspeccoes = valorMedioProspeccoes = valorTotalProspeccoes / quantidadeProspeccoes;

                if (quantidadeProspeccoesComProposta > 0)
                {
                    participacao.ValorMedioProspeccoesComProposta = valorMedioProspeccoesComProposta = valorTotalProspeccoesComProposta / quantidadeProspeccoesComProposta;
                }

                if (valorMedioProspeccoes != 0)
                {
                    participacao.Propositividade = valorMedioProspeccoesComProposta / valorMedioProspeccoes;
                }

                if (quantidadeProspeccoesComProposta != 0)
                {
                    participacao.TaxaConversaoProjeto = taxaConversaoProjeto = (quantidadeProspeccoesProjetizadas / (decimal)quantidadeProspeccoesComProposta);
                }
                else
                {
                    participacao.TaxaConversaoProjeto = taxaConversaoProjeto = 0;
                }

                // TODO: Preciso de valores de despesa do ISI
                participacao.Indice = taxaConversaoProposta * valorMedioProspeccoesComProposta * quantidadeProspeccoesComProposta;
            }

            return participacao;
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

                    qtdBolsistas = membrosEquipe.Count(u => u.Cargo == Cargo.PesquisadorBolsista);
                    qtdEstagiarios = membrosEquipe.Count(u => u.Cargo == Cargo.EstagiarioNivelSuperior || u.Cargo == Cargo.EstagiarioNivelTecnico);
                    qtdPesquisadores = membrosEquipe.Count(u => u.Cargo == Cargo.PesquisadorQMS);                    

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
        private async Task<List<ParticipacaoTotalViewModel>> GetParticipacoesTotaisUsuarios(string mesInicio, string anoInicio, string mesFim, string anoFim)
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
                RankearParticipacoes(participacoes);
                AcertarValorRankParticipacoes(participacoes);
                ObterRankingsMedios(participacoes);
                
                participacoes = participacoes.OrderByDescending(p => p.Rank).ToList();
            }            

			ViewBag.usuarioFoto = usuario.Foto;
			ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

            return View(participacoes);
        }

        private void ObterRankingsMedios(List<ParticipacaoTotalViewModel> participacoes)
        {
            decimal rankMedio = participacoes.Average(p => p.Rank);
            decimal rankMedioIndice = participacoes.Average(p => p.Indice);

            decimal rankMedioValorTotalProspeccao = participacoes.Average(p => p.RankPorIndicador["RankValorTotalProspeccoes"]);
            decimal rankMedioValorTotalProspeccoesComProposta = participacoes.Average(p => p.RankPorIndicador["RankValorTotalProspeccoesComProposta"]);
            decimal rankMedioValorMedioProspeccoes = participacoes.Average(p => p.RankPorIndicador["RankValorMedioProspeccoes"]);
            decimal rankMedioValorMedioProspeccoesComProposta = participacoes.Average(p => p.RankPorIndicador["RankValorMedioProspeccoesComProposta"]);
            decimal rankMedioQuantidadeProspeccoes = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoes"]);
            decimal rankMedioQuantidadeProspeccoesComProposta = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoesComProposta"]);
            decimal rankMedioQuantidadeProspeccoesProjeto = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoesProjeto"]);
            decimal rankMedioQuantidadeProspeccoesMembro = participacoes.Average(p => p.RankPorIndicador["RankQuantidadeProspeccoesMembro"]);
            decimal rankMedioPropositividade = participacoes.Average(p => p.RankPorIndicador["RankPropositividade"]);

            ViewData[nameof(rankMedio)] = rankMedio;
            ViewData[nameof(rankMedioIndice)] = rankMedioIndice;
            ViewData[nameof(rankMedioValorTotalProspeccao)] = rankMedioValorTotalProspeccao;
            ViewData[nameof(rankMedioValorTotalProspeccoesComProposta)] = rankMedioValorTotalProspeccoesComProposta;
            ViewData[nameof(rankMedioValorMedioProspeccoes)] = rankMedioValorMedioProspeccoes;
            ViewData[nameof(rankMedioValorMedioProspeccoesComProposta)] = rankMedioValorMedioProspeccoesComProposta;
            ViewData[nameof(rankMedioQuantidadeProspeccoes)] = rankMedioQuantidadeProspeccoes;
            ViewData[nameof(rankMedioQuantidadeProspeccoesComProposta)] = rankMedioQuantidadeProspeccoesComProposta;
            ViewData[nameof(rankMedioQuantidadeProspeccoesProjeto)] = rankMedioQuantidadeProspeccoesProjeto;
            ViewData[nameof(rankMedioQuantidadeProspeccoesMembro)] = rankMedioQuantidadeProspeccoesMembro;
            ViewData[nameof(rankMedioPropositividade)] = rankMedioPropositividade;
        }

        private void AcertarValorRankParticipacoes(List<ParticipacaoTotalViewModel> participacoes)
		{
            decimal mediaValorIndice = participacoes.Average(p => p.Indice);

            foreach (var participacao in participacoes)
            {
                if (mediaValorIndice != 0)
                {
                    participacao.Indice = participacao.Indice / mediaValorIndice;
                }
                else
                {
                    participacao.Indice = 0;
                }                
            }
		}

		/// <summary>
		/// Atribui os rankings as participações passadas por parâmetro, para que sejam exibidas na View. Valores de 0 a 1
		/// </summary>
		/// <param name="participacoes">Lista de participações (normalmente de um usuário específico mas pode ser genérica)</param>
		private static void RankearParticipacoes(List<ParticipacaoTotalViewModel> participacoes)
        {
            decimal rankValorTotalProspeccoesComProposta = 0;
            decimal rankValorMedioProspeccoesComProposta = 0;
            decimal rankQuantidadeProspeccoesProjeto = 0;
            decimal rankQuantidadeProspeccoes = 0;
            decimal rankQuantidadeProspeccoesMembro = 0;
            decimal rankPropositividade = 0;

            decimal medValorTotalProsp = participacoes.Average(p => p.ValorTotalProspeccoes);
            decimal medValorMedioProsp = participacoes.Average(p => p.ValorMedioProspeccoes);
            decimal medValorMedioProspComProposta = participacoes.Average(p => p.ValorMedioProspeccoesComProposta);
            decimal medTotalProspComProposta = participacoes.Average(p => p.ValorTotalProspeccoesComProposta);
            decimal medQtdProspeccoes = (decimal)participacoes.Average(p => p.QuantidadeProspeccoes);
            decimal medQtdProspeccoesMembro = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesMembro);
            decimal medQtdProspeccoesComProposta = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesComProposta);
            decimal medQtdProspProjetizadas = (decimal)participacoes.Average(p => p.QuantidadeProspeccoesProjeto);
            decimal medConversaoProjeto = participacoes.Average(p => p.TaxaConversaoProjeto);
            decimal medConversaoProposta = participacoes.Average(p => p.TaxaConversaoProposta);
            decimal medPropositividade = participacoes.Average(p => p.Propositividade);

            decimal maxValorTotalProsp = participacoes.Max(p => p.ValorTotalProspeccoes);
            decimal maxValorMedioProsp = participacoes.Max(p => p.ValorMedioProspeccoes);
            decimal maxValorMedioProspComProposta = participacoes.Max(p => p.ValorMedioProspeccoesComProposta);
            decimal maxTotalProspComProposta = participacoes.Max(p => p.ValorTotalProspeccoesComProposta);
            decimal maxQtdProspeccoes = participacoes.Max(p => p.QuantidadeProspeccoes);
            decimal maxQtdProspeccoesComProposta = participacoes.Max(p => p.QuantidadeProspeccoesComProposta);
            decimal maxQtdProspeccoesMembro = participacoes.Max(p => p.QuantidadeProspeccoesMembro);
            decimal maxQtdProspProjetizadas = participacoes.Max(p => p.QuantidadeProspeccoesProjeto);
            decimal maxConversaoProjeto = participacoes.Max(p => p.TaxaConversaoProjeto);
            decimal maxConversaoProposta = participacoes.Max(p => p.TaxaConversaoProposta);
            decimal maxPropositividade = participacoes.Max(p => p.Propositividade);

            foreach (var participacao in participacoes)
            {
                participacao.RankPorIndicador = new Dictionary<string, decimal>();

                decimal rankQuantidadeProspeccoesComProposta = 0;
				decimal rankValorTotalProspeccoes = 0;
				decimal rankValorMedioProspeccoes = 0;
                decimal calculoRank = 0;

                if (maxValorTotalProsp != 0)
                {
                    calculoRank += participacao.ValorTotalProspeccoes / maxValorTotalProsp;
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
                    calculoRank += participacao.QuantidadeProspeccoesProjeto / maxQtdProspProjetizadas;
                }
                if (maxQtdProspeccoesComProposta != 0)
                {
                    calculoRank += participacao.QuantidadeProspeccoesComProposta / maxQtdProspeccoesComProposta;
                }
                if (maxValorMedioProspComProposta != 0)
                {
                    calculoRank += participacao.ValorMedioProspeccoesComProposta / maxValorMedioProspComProposta;
                }
                if (maxConversaoProjeto != 0)
                {
                    calculoRank += participacao.TaxaConversaoProjeto / maxConversaoProjeto;
                }
                if (maxConversaoProposta != 0)
                {
                    calculoRank += participacao.TaxaConversaoProposta / maxConversaoProposta;
                }
                // TODO: CONFIRMAR \\//
                if (maxQtdProspeccoesMembro != 0)
                {
                    calculoRank += participacao.QuantidadeProspeccoesMembro / maxQtdProspeccoesMembro;
                }
                if (maxPropositividade != 0)
                {
                    calculoRank += participacao.Propositividade / maxPropositividade;
                }

                participacao.Rank = calculoRank /= 8;

                if (medValorTotalProsp != 0)
                {
					rankValorTotalProspeccoes = participacao.ValorTotalProspeccoes / medValorTotalProsp;
				}
                if (medValorMedioProsp != 0)
                {
					rankValorMedioProspeccoes = participacao.ValorMedioProspeccoes / medValorMedioProsp;
				}
                if (medTotalProspComProposta != 0)
                {
                    rankValorTotalProspeccoesComProposta = participacao.ValorTotalProspeccoesComProposta / medTotalProspComProposta;
                }
                if (medValorMedioProspComProposta != 0)
                {
                    rankValorMedioProspeccoesComProposta = participacao.ValorMedioProspeccoesComProposta / medValorMedioProspComProposta;
                }
                if (medQtdProspeccoes != 0)
                {
                    rankQuantidadeProspeccoes = participacao.QuantidadeProspeccoes / medQtdProspeccoes;
                }           
                if (medQtdProspProjetizadas != 0)
                {
                    rankQuantidadeProspeccoesProjeto = participacao.QuantidadeProspeccoesProjeto / medQtdProspProjetizadas;
                }
                if (medQtdProspeccoesComProposta != 0)
                {
                    rankQuantidadeProspeccoesComProposta = participacao.QuantidadeProspeccoesComProposta / medQtdProspeccoesComProposta;
                }
                if (medQtdProspeccoesMembro != 0)
                {
                    rankQuantidadeProspeccoesMembro = participacao.QuantidadeProspeccoesMembro / medQtdProspeccoesMembro;
                }
                if (medPropositividade != 0)
                {
                    rankPropositividade = participacao.Propositividade / medPropositividade;
                }

                participacao.RankPorIndicador["RankValorTotalProspeccoes"] = rankValorTotalProspeccoes;
                participacao.RankPorIndicador["RankValorTotalProspeccoesComProposta"] = rankValorTotalProspeccoesComProposta;
                participacao.RankPorIndicador["RankValorMedioProspeccoes"] = rankValorMedioProspeccoes;
                participacao.RankPorIndicador["RankValorMedioProspeccoesComProposta"] = rankValorMedioProspeccoesComProposta;
                participacao.RankPorIndicador["RankQuantidadeProspeccoes"] = rankQuantidadeProspeccoes;
                participacao.RankPorIndicador["RankQuantidadeProspeccoesComProposta"] = rankQuantidadeProspeccoesComProposta;
                participacao.RankPorIndicador["RankQuantidadeProspeccoesProjeto"] = rankQuantidadeProspeccoesProjeto;
                participacao.RankPorIndicador["RankQuantidadeProspeccoesMembro"] = rankQuantidadeProspeccoesMembro;
                participacao.RankPorIndicador["RankPropositividade"] = rankPropositividade;
            }
        }
    }
}
