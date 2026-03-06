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
    public class ModalEditProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ModalEditProspViewComponent(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ListaProsp chama Component.InvokeAsync("ModalEditProsp", new { id = prospeccao.Id })
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            // Empresas para o datalist
            var empresas = await _context.Empresa
                .Select(e => new { e.Id, e.Nome })
                .ToListAsync();

            ViewData["Empresas"] = new SelectList(empresas, "Id", "Nome");

            // Carregar Prospeccao do banco (se existir)
            Prospeccao model = null;
            if (!string.IsNullOrEmpty(id))
            {
                model = await _context.Prospeccao
                    .Include(p => p.Empresa)
                    .Include(p => p.Status)
                    .Include(p => p.Usuario)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }

            // Se não encontrou, cria um modelo vazio compatível
            if (model == null)
            {
                model = new Prospeccao(new FollowUp());
            }

            // Procurar apoio.csv em caminhos comuns e popular ViewData["PessoasApoio"]
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

            return View(model);
        }
    }
}