using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalPreCreateProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalPreCreateProspViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Prospeccao> prosp = await _context.Prospeccao.ToListAsync();

            ViewData["prospPlan"] = prosp;

            return View(new Prospeccao(new FollowUp()));
        }
    }
}