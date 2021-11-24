using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace BaseDeProjetos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;

        }

        public IActionResult Index()
        {
            ViewData["receita_isiqv"] = ReceitaCasa(Instituto.ISIQV);
            ViewData["receita_isiii"] = ReceitaCasa(Instituto.ISIII);
            ViewData["receita_cisho"] = ReceitaCasa(Instituto.CISHO);
            ViewData["n_prosp"] = _context.Prospeccao.ToList().Where(p => prospeccaoAtiva(p) == true).ToList().Count;
            ViewData["n_proj"] = _context.Projeto.Where(p => p.status != StatusProjeto.Concluido && p.DataEncerramento > DateTime.Now).ToList().Count;
            ViewData["n_empresas"] = _context.Empresa.ToList().Count;
            ViewData["satisfacao"] = 0.8872;
            return View();
        }

        private bool prospeccaoAtiva(Prospeccao p)
        {
            if (p.Status.Count < 0)
            {
                return false;
            }

            DateTime hoje = DateTime.Today;

            //Prospecções sem movimentação a mais de 30 dias
            if (p.Status.OrderBy(k => k.Data).First().Data > hoje.AddDays(-30))
            {
                return false;
            }

            return true;
        }

        private decimal ReceitaCasa(Instituto casa)
        {
            IQueryable<decimal> valores = _context.Projeto.
                Where(p => p.Casa == casa && p.status == StatusProjeto.EmExecucao).
                Select(p => CalcularReceita(p));

            decimal sum = 0;
            foreach (decimal valor in valores)
            {
                sum += valor;
            }

            return sum;
        }

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
