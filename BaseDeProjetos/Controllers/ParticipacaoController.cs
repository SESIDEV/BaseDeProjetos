using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private void CalculoParticipacao(int pesquisadores, int bolsistas, int estagiarios)
        {
            int numeroMembros = 1 + pesquisadores + bolsistas + estagiarios;

            double valorPorPesquisador = CalculoValorPesquisador(numeroMembros, pesquisadores);
            double valorPorBolsista = CalculoValorBolsista(numeroMembros, bolsistas);
            double valorPorEstagiario = CalculoValorEstagiario(numeroMembros, estagiarios);

            double totalPesquisadores = valorPorPesquisador * pesquisadores;
            double totalBolsistas = valorPorBolsista * bolsistas;
            double totalEstagiarios = valorPorEstagiario * estagiarios;
            double totalLider = 1 - (totalBolsistas + totalEstagiarios + totalPesquisadores);

            double totalProjeto = totalLider + totalBolsistas + totalEstagiarios + totalPesquisadores;
        }

        private double CalculoValorEstagiario(double numeroMembros, int estagiarios)
        {
            double resultado = (1 - 1 / numeroMembros) * 1 / 10 * (1 / (estagiarios + 1));
            return resultado;
        }

        private double CalculoValorBolsista(double numeroMembros, int bolsistas)
        {
            double resultado = (1 - (1 / numeroMembros)) * 3 / 10 * (1 / (bolsistas + 1));
            return resultado;
        }

        private double CalculoValorPesquisador(double numeroMembros, int pesquisadores)
        {
            double resultado = (1 - (1 / numeroMembros)) * 3 / 5 * (1 / (pesquisadores + 1));
            return resultado;
        }

        private ParticipacaoTotalViewModel GetParticipacaoTotalUsuario(Usuario usuario)
        {
            ParticipacaoTotalViewModel participacao = new ParticipacaoTotalViewModel();
            participacao.Participacoes = new List<ParticipacaoViewModel>();

            List<Prospeccao> prospeccoesUsuario = _context.Prospeccao.Where(p => p.Usuario == usuario).ToList();
            List<Projeto> projetosUsuario = _context.Projeto.Where(p => p.MembrosEquipe.Contains(usuario.UserName)).ToList();

            AtribuirParticipacoesIndividuais(participacao, prospeccoesUsuario);

            decimal valorTotalProspeccoes;
            decimal valorTotalProjetos;
            int quantidadeProspeccoes;
            int quantidadeProjetos;

            participacao.ValorTotalProspeccoes = valorTotalProspeccoes = prospeccoesUsuario.Sum(p => p.ValorProposta);
            participacao.ValorTotalProjetos = valorTotalProjetos = (decimal)projetosUsuario.Sum(p => p.ValorTotalProjeto);
            participacao.QuantidadeProspeccoes = quantidadeProspeccoes = prospeccoesUsuario.Count();
            participacao.QuantidadeProjetos = quantidadeProjetos = projetosUsuario.Count();
            participacao.Lider = usuario;

            // Evita divisão por 0
            if (prospeccoesUsuario.Count() == 0)
            {
                participacao.ValorMedioProspeccoes = 0;
            }
            else
            {
                participacao.ValorMedioProspeccoes = valorTotalProspeccoes / quantidadeProspeccoes;
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

        private static void AtribuirParticipacoesIndividuais(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
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

                ParticipacaoViewModel participacaoUnitaria = new ParticipacaoViewModel()
                {
                    Id = Guid.NewGuid(),
                    NomeProjeto = nomeProjeto,
                    ValorNominal = prospeccao.ValorProposta
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
				participacoes.Add(GetParticipacaoTotalUsuario(usuario));
			}

			return participacoes;
		}


		[HttpGet]
		public IActionResult Index()
        {
			var participacoes = GetParticipacoesTotaisUsuarios();

			return View(participacoes);
        }
    }
}
