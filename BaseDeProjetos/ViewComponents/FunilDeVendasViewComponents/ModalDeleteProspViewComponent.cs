using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalDeleteProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalDeleteProspViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = await _context.Prospeccao
                .Include(p => p.Usuario)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(p => p.Id == id);
            return View(model);
        }
    }
}
