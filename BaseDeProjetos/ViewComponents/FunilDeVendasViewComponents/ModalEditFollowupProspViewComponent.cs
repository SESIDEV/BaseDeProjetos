using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalEditFollowupProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalEditFollowupProspViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id, int id2)
        {
            ViewData["prospeccao"] = await _context.Prospeccao.FindAsync(id);
            FollowUp followup = await _context.FollowUp.FindAsync(id2);
            return View(followup);
        }
    }
}