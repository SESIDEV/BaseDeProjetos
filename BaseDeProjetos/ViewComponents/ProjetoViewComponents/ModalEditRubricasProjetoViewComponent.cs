using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.ProjetoViewComponents
{
    public class ModalEditRubricasProjetoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalEditRubricasProjetoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = await _context.Rubrica.FindAsync(id);
            return View(model);
        }
    }
}