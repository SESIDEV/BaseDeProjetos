using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BaseDeProjetos.Models;
using BaseDeProjetos.Data;

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
            ViewData["n_prosp"] = _context.Prospeccao.Where(p => p.Status.Count > 0).ToList<Prospeccao>().Count;
            ViewData["n_proj"] = _context.Projeto.Where(p => p.status != StatusProjeto.Concluido && p.DataEncerramento > DateTime.Now).ToList().Count;
            ViewData["n_empresas"] = _context.Empresa.ToList().Count;
            return View();
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
