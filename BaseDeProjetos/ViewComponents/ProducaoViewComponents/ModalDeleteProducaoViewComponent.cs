using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.ProducaoViewComponents
{
    public class ModalDeleteProducaoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalDeleteProducaoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = await _context.Producao.FindAsync(id);
            return View(model);
        }
    }
}