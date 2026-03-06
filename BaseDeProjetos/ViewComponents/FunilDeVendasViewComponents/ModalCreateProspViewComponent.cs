using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalCreateProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ModalCreateProspViewComponent(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var empresas = await _context.Empresa
                .Select(e => new { e.Id, e.Nome })
                .ToListAsync();

            ViewData["Empresas"] = new SelectList(empresas, "Id", "Nome");

            // Procurar apoio.csv em caminhos comuns
            var possiblePaths = new[]
            {
                Path.Combine(_env.ContentRootPath, "wwwroot", "apoio", "apoio.csv"),
                Path.Combine(_env.ContentRootPath, "wwwroot", "apoio.csv"),
                Path.Combine(_env.ContentRootPath, "App_Data", "apoio.csv"),
                Path.Combine(_env.ContentRootPath, "apoio.csv")
            };

            var pessoas = new List<PessoaModel>();
            foreach (var p in possiblePaths)
            {
                if (File.Exists(p))
                {
                    pessoas = ApoioCsv.LerPessoasDeCsv(p);
                    break;
                }
            }

            ViewData["PessoasApoio"] = pessoas;

            return View(new Prospeccao(new FollowUp()));
        }
    }
}