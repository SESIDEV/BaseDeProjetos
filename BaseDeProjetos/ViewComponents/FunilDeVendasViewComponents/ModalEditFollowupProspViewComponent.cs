using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalEditFollowupProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        private readonly DbCache _cache;

        public ModalEditFollowupProspViewComponent(ApplicationDbContext context, DbCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id, int id2)
        {
            ViewData["prospeccao"] = await _cache.GetCachedAsync($"Prospeccao:{id}", () => _context.Prospeccao.FindAsync(id).AsTask());
            var followup = await _cache.GetCachedAsync($"Followup:{id2}", () => _context.FollowUp.FindAsync(id2).AsTask());
            return View(followup);
        }
    }
}