using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        [Route("Home/Index/{id?}")]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                ObterDadosReceita();


                //PARA FINS DE DEBUG
                if(usuario.Casa == Instituto.Super)
                {
                    ViewData["Volume_Negocios"] = _context.Prospeccao.Where(p => p.Casa == Instituto.ISIQV).ToList().Where(p => prospeccaoAtiva(p) == true).Sum(p => p.ValorEstimado);
                }

                ViewData["Volume_Negocios"] = _context.Prospeccao.Where(p => p.Casa == Instituto.ISIQV).ToList().Where(p => prospeccaoAtiva(p) == true).Sum(p => p.ValorEstimado);

                ViewData["n_prosp"] = _context.Prospeccao.Where(p => p.Casa == usuario.Casa).ToList().Where(p => prospeccaoAtiva(p) == true).ToList().Count;
                //Volume de negocios é o somatório de todos os valores de prospecções
                ViewData["Volume_Negocios"] = _context.Prospeccao.Where(p => p.Casa == usuario.Casa).ToList().Where(p => prospeccaoAtiva(p) == true).Sum(p => p.ValorEstimado);
                ViewData["n_proj"] = _context.Projeto.Where(p => p.Casa == usuario.Casa).Where(p => p.Casa == usuario.Casa).Where(p => p.Status == StatusProjeto.EmExecucao).ToList().Count;
                ViewData["n_empresas"] = _context.Empresa.ToList().Count;
                ViewData["satisfacao"] = 0.8750;
                ViewData["Valor_Prosp_Proposta"] = _context.Prospeccao.Where(p => p.Casa == usuario.Casa).Where(p => p.Status.Any(s => s.Status == StatusProspeccao.ComProposta)).Sum(p => p.ValorProposta);
                ViewBag.Usuarios = _context.Users.AsEnumerable().Where(p => p.Casa == usuario.Casa).Where(u => ValidarCasa(u, usuario)).Where(a => a.EmailConfirmed == true).ToList().Count();
                ViewBag.Estados = _context.Prospeccao.Where(p => p.Casa == usuario.Casa).Select(p => p.Empresa.Estado).ToList();
                ViewBag.LinhaDePesquisa = _context.Prospeccao.Where(p => p.Casa == usuario.Casa).Select(p => p.LinhaPequisa).ToList();                

            }
            return View();

        }
        private static bool ValidarCasa(Usuario u, Usuario usuario)
        {
            if ((usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO))
                return usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO;

            return u.Casa == usuario.Casa;
        }

        private void ObterDadosReceita()
        {

            Usuario usuario = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
            //Implementar quando base tiver atualizada
            ViewData["receita_isiqv"] = ReceitaCasa(Instituto.ISIQV);
            ViewData["receita_isiii"] = ReceitaCasa(Instituto.ISIII);
            ViewData["receita_cisho"] = ReceitaCasa(Instituto.CISHO);

            try
            {

                if (usuario.Casa == Instituto.Super || usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO)
                {
                    ViewData["receita_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList().LastOrDefault().Receita;
                    ViewData["despesa_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList().LastOrDefault().Despesa;
                    ViewData["invest_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList().LastOrDefault().Investimento;
                    ViewData["Data"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList().LastOrDefault().Data;
                    ViewData["quali"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList().LastOrDefault().QualiSeguranca;

                }
                else
                {
                    ViewData["receita_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == usuario.Casa).ToList().LastOrDefault().Receita;
                    ViewData["despesa_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == usuario.Casa).ToList().LastOrDefault().Despesa;
                    ViewData["invest_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == usuario.Casa).ToList().LastOrDefault().Investimento;
                    ViewData["Data"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == usuario.Casa).ToList().LastOrDefault().Data;
                    ViewData["quali"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == usuario.Casa).ToList().LastOrDefault().QualiSeguranca;
                }


                              
                
            }
            catch (Exception)
            {
                ViewData["receita_total"] = 0;
                ViewData["despesa_total"] = 1;
                ViewData["invest_total"] = 0;
                ViewData["quali"] = 0;
                ViewData["Data"] = DateTime.Today;
            }
        }

        private bool prospeccaoAtiva(Prospeccao p)
        {
            if (p.Status.Count <= 0)
            {
                return false;
            }

            return p.Status.OrderBy(k=>k.Data).LastOrDefault().Status <= StatusProspeccao.ComProposta;
;
        }      

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