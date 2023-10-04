using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalDetailsProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalDetailsProspViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            Prospeccao prospeccao = await _context.Prospeccao.FindAsync(id);
            return View(prospeccao);
        }
    }
}