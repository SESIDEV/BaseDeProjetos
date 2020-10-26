using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
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
            ViewData["receita_isiqv"] = ReceitaCasa(Casa.ISIQV);
            ViewData["receita_isiii"] = ReceitaCasa(Casa.ISIII);
            ViewData["receita_cisho"] = ReceitaCasa(Casa.CISHO);
            ViewData["n_prosp"] = _context.Prospeccao.Where(p => p.Status.Count > 0).ToList<Prospeccao>().Count;
            ViewData["n_proj"] = _context.Projeto.Where(p => p.status != StatusProjeto.Concluido && p.DataEncerramento > DateTime.Now).ToList().Count;
            ViewData["n_empresas"] = _context.Empresa.ToList().Count;
            ViewData["satisfacao"] = 0.8872;
            return View();
        }

        private double ReceitaCasa(Casa casa)
        {
            return _context.Projeto.
                Where(p => p.Casa == casa && p.status == StatusProjeto.EmExecucao).
                Select(p => p.ValorAporteRecursos).
                Sum();
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
