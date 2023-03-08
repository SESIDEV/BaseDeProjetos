using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BaseDeProjetos.Models;
using System.Linq;
using System.Collections.Generic;

namespace BaseDeProjetos.ViewComponents
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
            List<Prospeccao> prosp = _context.Prospeccao.ToList();

            ViewData["prospPlan"] = prosp;
            
            return View(new Prospeccao(new FollowUp()));
        }
    }
}
