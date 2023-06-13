using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BaseDeProjetos.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
