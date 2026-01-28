using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalCreateProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalCreateProspViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var empresas = await _context.Empresa
                .Select(e => new { e.Id, e.Nome })
                .ToListAsync();

            ViewData["Empresas"] = new SelectList(empresas, "Id", "Nome");

            return View(new Prospeccao(new FollowUp()));
        }
    }
}