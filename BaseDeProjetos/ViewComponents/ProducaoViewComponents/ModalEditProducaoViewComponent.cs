using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.ProducaoViewComponents
{
    public class ModalEditProducaoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalEditProducaoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            Producao model = await _context.Producao.FindAsync(id);
            return View(model);
        }
    }
}