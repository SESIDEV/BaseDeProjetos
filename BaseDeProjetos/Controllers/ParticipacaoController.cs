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

            AtribuirParticipacoesIndividuais(participacao, prospeccoesUsuario);

            decimal valorTotal;
            int quantidadeProspeccoes;

            participacao.ValorTotalProspeccoes = valorTotal = prospeccoesUsuario.Sum(p => p.ValorProposta);
            participacao.QuantidadeProspeccoes = quantidadeProspeccoes = prospeccoesUsuario.Count();
            participacao.Lider = usuario;

            // Evita divisão por 0
            if (prospeccoesUsuario.Count() == 0)
            {
                participacao.ValorMedioProspeccoes = 0;
            }
            else
            {
                participacao.ValorMedioProspeccoes = valorTotal / quantidadeProspeccoes;
            }

            return participacao;
        }

        private static void AtribuirParticipacoesIndividuais(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario)
        {
            foreach (var prospeccao in prospeccoesUsuario)
            {
                ParticipacaoViewModel participacaoUnitaria = new ParticipacaoViewModel()
                {
                    Id = Guid.NewGuid(),
                    NomeProjeto = prospeccao.NomeProspeccao,
                    ValorNominal = (prospeccao.ValorProposta != 0) ? (prospeccao.ValorProposta) : prospeccao.ValorEstimado
                };
                participacao.Participacoes.Add(participacaoUnitaria);
            }
        }

        private List<ParticipacaoTotalViewModel> GetParticipacoesTotaisUsuarios()
		{
			// TODO: Harcoded ISIQV/CISHO
			List<Usuario> usuarios = _context.Users.Where(u=> u.Casa == Instituto.ISIQV || u.Casa == Instituto.CISHO).ToList();
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
