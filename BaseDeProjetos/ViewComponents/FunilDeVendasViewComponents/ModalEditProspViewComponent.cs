using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalEditProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly DbCache _cache;

        public ModalEditProspViewComponent(ApplicationDbContext context, DbCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = await _cache.GetCachedAsync($"Prospeccao:{id}", () => _context.Prospeccao.Where(p => p.Id == id).FirstOrDefaultAsync());
            return View(model);
        }
    }
}