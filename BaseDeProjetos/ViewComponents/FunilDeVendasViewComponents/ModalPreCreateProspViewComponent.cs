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

        private readonly DbCache _cache;

        public ModalPreCreateProspViewComponent(ApplicationDbContext context, DbCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var prospeccoes = await _cache.GetCachedAsync("AllProspeccoes", () => _context.Prospeccao.ToListAsync());

            ViewData["prospPlan"] = prospeccoes;

            return View(new Prospeccao(new FollowUp()));
        }
    }
}