using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
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

        public async Task<IViewComponentResult> InvokeAsync(string id, string id2)
        {
            ViewData["prospeccao"] = await _context.Prospeccao.FindAsync(id);
            FollowUp followup = await _context.FollowUp.FindAsync(int.Parse(id2));
            return View(followup);
        }
    }
}
