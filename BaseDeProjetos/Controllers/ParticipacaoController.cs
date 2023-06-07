using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

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
                { "totalLider", totalLider } 
            };
        }

        private decimal CalculoValorEstagiario(decimal numeroMembros, decimal estagiarios)
        {
            decimal resultado = (1 - 1 / numeroMembros) * 1 / 10 * (1 / (estagiarios + 1));
            return resultado;
        }

        private decimal CalculoValorBolsista(decimal numeroMembros, decimal bolsistas)
        {
            decimal resultado = (1 - (1 / numeroMembros)) * 3 / 10 * (1 / (bolsistas + 1));
            return resultado;
        }

        private decimal CalculoValorPesquisador(decimal numeroMembros, decimal pesquisadores)
        {
            decimal resultado = (1 - (1 / numeroMembros)) * 3 / 5 * (1 / (pesquisadores + 1));
            return resultado;
        }

        private ParticipacaoTotalViewModel GetParticipacaoTotalUsuario(Usuario usuario)
        {
            ParticipacaoTotalViewModel participacao = new ParticipacaoTotalViewModel() { Participacoes = new List<ParticipacaoViewModel>() };

            List<Prospeccao> prospeccoesUsuario = _context.Prospeccao.Where(p => p.Usuario == usuario).ToList();
            List<Prospeccao> prospeccoesUsuarioComProposta = _context.Prospeccao.Where(p => p.Usuario == usuario && p.Status.Any(f => f.Status == StatusProspeccao.ComProposta)).ToList();
            List<Prospeccao> prospeccoesUsuarioProjetizadas = _context.Prospeccao.Where(p => p.Usuario == usuario && p.Status.Any(f => f.Status == StatusProspeccao.Convertida && f.Status != StatusProspeccao.Suspensa)).ToList();
            List<Projeto> projetosUsuario = _context.Projeto.Where(p => p.MembrosEquipe.Contains(usuario.UserName)).ToList();

            AtribuirParticipacoesIndividuais(participacao, prospeccoesUsuario);

            decimal valorTotalProspeccoes;
            decimal valorMedioProspeccoes;
            decimal valorTotalProjetos;
            decimal taxaConversaoProposta;
            decimal taxaConversaoProjeto;
            int quantidadeProspeccoes;
            int quantidadeProspeccoesComProposta;
            int quantidadeProspeccoesProjetizadas;
            int quantidadeProjetos;

            participacao.ValorTotalProspeccoes = valorTotalProspeccoes = prospeccoesUsuario.Sum(p => p.ValorProposta);
            participacao.ValorTotalProjetos = valorTotalProjetos = (decimal)projetosUsuario.Sum(p => p.ValorTotalProjeto);
            participacao.QuantidadeProspeccoes = quantidadeProspeccoes = prospeccoesUsuario.Count();
            participacao.QuantidadeProspeccoesComProposta = quantidadeProspeccoesComProposta = prospeccoesUsuarioComProposta.Count();
            participacao.QuantidadeProspeccoesProjetizadas = quantidadeProspeccoesProjetizadas = prospeccoesUsuarioProjetizadas.Count();
            participacao.QuantidadeProjetos = quantidadeProjetos = projetosUsuario.Count();
            participacao.Lider = usuario;
            

            // Evita divisão por 0
            if (prospeccoesUsuario.Count() == 0)
            {
                participacao.ValorMedioProspeccoes = 0;
                participacao.TaxaConversaoProposta = 0;
                participacao.TaxaConversaoProjeto = 0;
            }
            else
            {
                participacao.TaxaConversaoProposta = taxaConversaoProposta = (quantidadeProspeccoesComProposta / (decimal)quantidadeProspeccoes) * 100;
                participacao.ValorMedioProspeccoes = valorMedioProspeccoes = valorTotalProspeccoes / quantidadeProspeccoes;
                participacao.TaxaConversaoProjeto = taxaConversaoProjeto = (quantidadeProspeccoesProjetizadas / (decimal)quantidadeProspeccoes) * 100;
                participacao.Rank = valorTotalProspeccoes + valorMedioProspeccoes + quantidadeProspeccoes + quantidadeProspeccoesProjetizadas;
            }

            if (projetosUsuario.Count() == 0)
            {
                participacao.ValorMedioProjetos = 0;
            }
            else
            {
                participacao.ValorMedioProjetos = valorTotalProjetos / quantidadeProjetos;
            }

            return participacao;
        }

        private void AtribuirParticipacoesIndividuais(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
        {
            foreach (var prospeccao in prospeccoesUsuario)
            {
                var nomeProjeto = !string.IsNullOrEmpty(prospeccao.NomeProspeccao) ? prospeccao.NomeProspeccao : prospeccao.Empresa.Nome;

                // Tratar prospecções que tem "projeto" no nome (...)
                // i.e: Remover na hora de apresentar o nome casos em que temos "Projeto projeto XYZ"
                if (nomeProjeto != null && nomeProjeto != "" && nomeProjeto.ToLowerInvariant().Contains("projeto"))
                {
                    nomeProjeto = nomeProjeto.Replace("projeto", "");
                    nomeProjeto = nomeProjeto.Replace("Projeto", "");
                }

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

                Dictionary<string, decimal> calculoParticipantes = CalculoParticipacao(pesqNormTemp, pesqBolsTemp, estagTemp);

                var valorLider = calculoParticipantes["totalLider"] * prospeccao.ValorProposta;
                var valorPesquisadores = calculoParticipantes["totalPesquisadores"] * prospeccao.ValorProposta;

                ParticipacaoViewModel participacaoUnitaria = new ParticipacaoViewModel()
                {
                    Id = Guid.NewGuid(),
                    NomeProjeto = nomeProjeto,
                    ValorNominal = prospeccao.ValorProposta,
                    MembrosEquipe = prospeccao.MembrosEquipe,
                    ValorLider = valorLider,
                    ValorPesquisadores = valorPesquisadores,
                    QuantidadeBolsistas = pesqBolsTemp,
                    QuantidadeEstagiarios = estagTemp,
                    QuantidadePesquisadores = pesqNormTemp,
                    QuantidadeMembros = pesqBolsTemp + estagTemp + pesqNormTemp + 1 // 1 == Líder
                };

                participacao.Participacoes.Add(participacaoUnitaria);
            }
        }

        private List<ParticipacaoTotalViewModel> GetParticipacoesTotaisUsuarios()
		{
            Usuario usuarioAtivo = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
            List<Usuario> usuarios;

			if (usuarioAtivo.Casa == Instituto.ISIQV || usuarioAtivo.Casa == Instituto.CISHO)
            {
				usuarios = _context.Users.Where(u => u.Casa == Instituto.ISIQV || u.Casa == Instituto.CISHO).ToList();
			}
            else
            {
                usuarios = _context.Users.Where(u => u.Casa == usuarioAtivo.Casa).ToList();
			}

			List<ParticipacaoTotalViewModel> participacoes = new List<ParticipacaoTotalViewModel>();
			
			foreach (var usuario in usuarios)
			{
                var participacao = GetParticipacaoTotalUsuario(usuario);
				participacoes.Add(participacao);
			}

			return participacoes;
		}


		[HttpGet]
		public IActionResult Index()
        {
			Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

			var participacoes = GetParticipacoesTotaisUsuarios();

            decimal maxTotalProsp = participacoes.Max(p => p.ValorTotalProspeccoes);
            decimal maxValorMedioProsp = participacoes.Max(p => p.ValorMedioProspeccoes);
            decimal maxQtdProspeccoes = participacoes.Max(p => p.QuantidadeProspeccoes);
            decimal maxQtdProspProjetizadas = participacoes.Max(p => p.QuantidadeProspeccoesProjetizadas);

            foreach (var participacao in participacoes)
            {
                decimal calculoRank = (participacao.ValorTotalProspeccoes / maxTotalProsp) + 
                    (participacao.ValorMedioProspeccoes / maxValorMedioProsp) + 
                    (participacao.QuantidadeProspeccoes / maxQtdProspeccoes) + 
                    (participacao.QuantidadeProspeccoesProjetizadas / maxQtdProspProjetizadas);
                calculoRank = calculoRank / 4;

                participacao.Rank = calculoRank * 100;
            }

            participacoes = participacoes.OrderByDescending(p => p.Rank).ToList();

			ViewBag.usuarioCasa = usuario.Casa;
			ViewBag.usuarioNivel = usuario.Nivel;

			return View(participacoes);
        }
    }
}
