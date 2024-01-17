using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalHistoryProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly DbCache _cache;

        public ModalHistoryProspViewComponent(ApplicationDbContext context, DbCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = await _cache.GetCachedAsync($"Prospeccao:{id}", () => _context.Prospeccao.FindAsync(id).AsTask());
            return View(model);
        }
    }
}