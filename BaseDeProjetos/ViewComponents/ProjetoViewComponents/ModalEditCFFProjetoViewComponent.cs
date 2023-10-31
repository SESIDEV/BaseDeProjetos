using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.ProjetoViewComponents
{
    public class ModalEditCFFProjetoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalEditCFFProjetoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = await _context.CurvaFisicoFinanceira.FindAsync(id);
            return View(model);
        }
    }
}