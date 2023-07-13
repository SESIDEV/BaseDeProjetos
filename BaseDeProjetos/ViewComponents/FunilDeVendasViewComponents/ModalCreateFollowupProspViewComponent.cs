using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalCreateFollowupProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ModalCreateFollowupProspViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            ViewData["prospeccao"] = await _context.Prospeccao.FindAsync(id);
            return View(new FollowUp());
        }
    }
}
