using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    public class HomeController : SGIController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        [Route("Home/Index/{id?}")]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                ViewData["satisfacao"] = 0.8750; // TODO: Implementar lógica?
            }
            return View();
        }

        /// <summary>
        /// Obtém os dados da receita de cada instituto, atribuindo-os ao ViewData
        /// </summary>
        [HttpGet("Home/ObterDadosReceita")]
        public async Task<IActionResult> ObterDadosReceita()
        {
            ViewbagizarUsuario(_context);

            Dictionary<string, object> dadosHome = new Dictionary<string, object>();
            Dictionary<string, decimal> dadosFinanceiros = new Dictionary<string, decimal>();
            Dictionary<string, int> dadosOperacionais = new Dictionary<string, int>();
            Dictionary<string, DateTime> data = new Dictionary<string, DateTime>();
            Dictionary<string, object> dadosGrafico = new Dictionary<string, object>();
            Dictionary<string, float> dadosProspeccoes = new Dictionary<string, float>();

            decimal receitaTotal = 0;
            decimal despesaTotal = 0;
            decimal investimentoTotal = 0;
            decimal sustentabilidade = 0;

            //Implementar quando base tiver atualizada
            // Quando? --HH
            dadosFinanceiros["receitaISIQV"] = ReceitaCasa(Instituto.ISIQV);
            dadosFinanceiros["receitaISIII"] = ReceitaCasa(Instituto.ISIII);
            dadosFinanceiros["receitaCISHO"] = ReceitaCasa(Instituto.CISHO);

            // var prospeccoes = await _context.Prospeccao.Where(p => p.Casa == UsuarioAtivo.Casa).ToListAsync();
            var prospeccoes = await _context.Prospeccao.Select(p => new { p.Casa, p.ValorEstimado, p.Status, p.Empresa, p.LinhaPequisa }).Where(p => p.Casa == UsuarioAtivo.Casa).ToListAsync();
            var prospeccoesAtivas = prospeccoes.Where(p => p.Status.OrderBy(k => k.Data).LastOrDefault().Status <= StatusProspeccao.ComProposta).ToList();
            var prospeccoesNaoPlanejadas = prospeccoes.Where(p => p.Status.Any(s => s.Status != StatusProspeccao.Planejada)).ToList();
            var prospeccoesComProposta = prospeccoes.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.ComProposta).ToList();
            var prospeccoesConcluidas = prospeccoes.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida).ToList();
            var prospeccoesPlanejadas = prospeccoes.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Planejada).ToList();
            var projetos = await _context.Projeto.Select(p => new { p.Casa, p.Status }).Where(p => p.Casa == UsuarioAtivo.Casa && p.Status == StatusProjeto.EmExecucao).ToListAsync();
            var empresas = prospeccoesNaoPlanejadas.Select(e => e.Empresa.Id).Distinct().ToList();
            var usuarios = await _context.Users.Select(u => new { id = u.Id, casa = u.Casa, emailConfirmed = u.EmailConfirmed, nivel = u.Nivel }).Where(u => u.casa == UsuarioAtivo.Casa).Where(u => u.emailConfirmed == true).Where(u => u.nivel != Nivel.Dev && u.nivel != Nivel.Externos).ToListAsync();
            var linhasDePesquisa = prospeccoesNaoPlanejadas.Select(p => p.LinhaPequisa).ToList();

            // Separação do query SQL em algumas variáveis para clareza
            var linhaDePesquisaDistintos = linhasDePesquisa.OrderByDescending(lp => linhasDePesquisa.Where(lp2 => lp2 == lp).Count()).Distinct();
            List<int> quantidades = linhaDePesquisaDistintos.Select(p => linhasDePesquisa.Where(k => k == p).Count()).ToList();
            List<string> labelsDePesquisa = linhaDePesquisaDistintos.Select(p => $"`{p.GetDisplayName().ToUpperInvariant()}`").ToList();

            dadosOperacionais["prospTotais"] = prospeccoes.Count;
            dadosOperacionais["prospAtivas"] = prospeccoesAtivas.Count;
            dadosOperacionais["projetos"] = projetos.Count;
            dadosOperacionais["empresas"] = empresas.Count;
            dadosOperacionais["usuarios"] = usuarios.Count;

            dadosGrafico["linhasDePesquisa"] = quantidades;
            dadosGrafico["labels"] = labelsDePesquisa;

            dadosProspeccoes["prospAtivas"] = prospeccoesAtivas.Count;
            dadosProspeccoes["prospComProposta"] = prospeccoesComProposta.Count;
            dadosProspeccoes["prospConcluidas"] = prospeccoesConcluidas.Count;
            dadosProspeccoes["prospNaoPlanejadas"] = prospeccoesNaoPlanejadas.Count;
            dadosProspeccoes["prospPlanejadas"] = prospeccoesPlanejadas.Count;

            dadosProspeccoes["proporcaoAtivas"] = prospeccoesAtivas.Count / (float)prospeccoes.Count;
            dadosProspeccoes["proporcaoComProposta"] = prospeccoesComProposta.Count / (float)prospeccoes.Count;
            dadosProspeccoes["proporcaoConcluidas"] = prospeccoesConcluidas.Count / (float)prospeccoes.Count;
            dadosProspeccoes["proporcaoPlanejadas"] = prospeccoesPlanejadas.Count / (float)prospeccoes.Count;

            try
            {
                if (UsuarioAtivo.Casa == Instituto.Super || UsuarioAtivo.Casa == Instituto.ISIQV || UsuarioAtivo.Casa == Instituto.CISHO)
                {
                    var indicadoresFinanceiros = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV || lista.Casa == Instituto.CISHO).ToList().LastOrDefault();
                    // Volume de negocios é o somatório de todos os valores de prospecções
                    dadosFinanceiros["volumeNegocios"] = prospeccoesAtivas.Where(p => p.Casa == Instituto.ISIQV || p.Casa == Instituto.CISHO).Sum(p => p.ValorEstimado);
                    dadosFinanceiros["receitaTotal"] = receitaTotal = indicadoresFinanceiros.Receita;
                    dadosFinanceiros["despesaTotal"] = despesaTotal = indicadoresFinanceiros.Despesa;
                    dadosFinanceiros["investimentoTotal"] = investimentoTotal = indicadoresFinanceiros.Investimento;
                    dadosFinanceiros["quali"] = (decimal)indicadoresFinanceiros.QualiSeguranca;
                    dadosFinanceiros["sustentabilidade"] = sustentabilidade = investimentoTotal / despesaTotal;
                    data["data"] = indicadoresFinanceiros.Data;
                }
                else
                {
                    var indicadoresFinanceiros = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == UsuarioAtivo.Casa).ToList().LastOrDefault();
                    dadosFinanceiros["volumeNegocios"] = prospeccoesAtivas.Sum(p => p.ValorEstimado);
                    dadosFinanceiros["receitaTotal"] = receitaTotal = indicadoresFinanceiros.Receita;
                    dadosFinanceiros["despesaTotal"] = despesaTotal = indicadoresFinanceiros.Despesa;
                    dadosFinanceiros["investimentoTotal"] = investimentoTotal = indicadoresFinanceiros.Investimento;
                    dadosFinanceiros["quali"] = (decimal)indicadoresFinanceiros.QualiSeguranca;
                    dadosFinanceiros["sustentabilidade"] = sustentabilidade = investimentoTotal / despesaTotal;
                    data["data"] = indicadoresFinanceiros.Data;
                }
            }
            catch (Exception)
            {
                dadosFinanceiros["volumeNegocios"] = 0;
                dadosFinanceiros["receitaTotal"] = 0;
                dadosFinanceiros["despesaTotal"] = 1;
                dadosFinanceiros["investimentoTotal"] = 0;
                dadosFinanceiros["quali"] = 0;
                dadosFinanceiros["sustentabilidade"] = 0;

                data["data"] = DateTime.Today;
            }

            dadosHome["dadosFinanceiros"] = dadosFinanceiros;
            dadosHome["dadosOperacionais"] = dadosOperacionais;
            dadosHome["dadosGrafico"] = dadosGrafico;
            dadosHome["dadosProspeccoes"] = dadosProspeccoes;
            dadosHome["data"] = data;

            return Ok(JsonConvert.SerializeObject(dadosHome));
        }

        /// <summary>
        /// Verifica se uma prospecção está com status considerado ativo
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool VerificarProspeccaoAtiva(Prospeccao p)
        {
            if (p.Status.Count <= 0)
            {
                return false;
            }

            return p.Status.OrderBy(k => k.Data).LastOrDefault().Status <= StatusProspeccao.ComProposta;
        }

        /// <summary>
        /// Retorna o valor da receita para um determinado Instituto
        /// </summary>
        /// <param name="casa">Instituto a se obter o valor</param>
        /// <returns></returns>
        private decimal ReceitaCasa(Instituto casa)
        {
            IQueryable<decimal> valores = _context.Projeto.
                Where(p => p.Casa == casa && p.Status == StatusProjeto.EmExecucao).
                Select(p => CalcularReceita(p));

            decimal sum = 0;
            foreach (decimal valor in valores)
            {
                sum += valor;
            }

            return sum;
        }

        /// <summary>
        /// Retorna a receita dado um projeto
        /// </summary>
        /// <param name="p">Projeto a se obter a receita</param>
        /// <returns></returns>
        private static decimal CalcularReceita(Projeto p)
        {
            decimal valor_aportado = 0M;
            if (p.DataEncerramento < DateTime.Today)
            {
                //O projeto está em execução, mas não recebe mais dinheiro
                return valor_aportado;
            }

            if (p.DataEncerramento.Year == DateTime.Today.Year)
            {
                if (p.DuracaoProjetoEmMeses >= 12)
                {
                    //Caso relativamente simples, o projeto é longo e acaba neste ano, recebendo até o seu término (Jan/19 a Mar/21)
                    return (decimal)((p.ValorAporteRecursos / p.DuracaoProjetoEmMeses) * p.DataEncerramento.Month);
                }
                else
                {
                    //É um projeto curto, que provavelmente começou e vai acabar neste ano (Abril a Outubro)
                    if (p.DataInicio.Year == DateTime.Today.Year)
                    {
                        return (decimal)p.ValorAporteRecursos;
                    }
                    else
                    {
                        //É um projeto curto que já vinha em andamento e que vai terminar neste ano (Ex.: De setembro a março)
                        return (decimal)((p.ValorAporteRecursos / p.DuracaoProjetoEmMeses) * p.DataEncerramento.Month);
                    }
                }
            }
            else
            {
                //Caso mais simples, o projeto tem mais de 12 meses e não começou nem termina este ano (jan/20 a Dez/24)
                if (p.DuracaoProjetoEmMeses >= 12 && p.DataInicio.Year != DateTime.Today.Year)
                {
                    return (decimal)((p.ValorAporteRecursos / p.DuracaoProjetoEmMeses) * 12);
                }

                //O projeto tem menos de 12 meses, mas só acaba no ano seguinte (Set/20 a Mar/21)
                if (p.DataEncerramento.Year == (DateTime.Today.Year + 1))
                {
                    return (decimal)((p.ValorAporteRecursos / p.DuracaoProjetoEmMeses) * (12 - p.DataInicio.Month));
                }
            }

            return valor_aportado;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}