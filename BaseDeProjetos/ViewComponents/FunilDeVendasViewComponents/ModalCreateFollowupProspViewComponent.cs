using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalCreateFollowupProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        private readonly DbCache _cache;
        public ModalCreateFollowupProspViewComponent(ApplicationDbContext context, DbCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var prospeccao = await _cache.GetCachedAsync($"Prospeccao:{id}", () => _context.Prospeccao.FindAsync(id).AsTask());
            ViewData["prospeccao"] = prospeccao;
            return View(new FollowUp());
        }
    }
}