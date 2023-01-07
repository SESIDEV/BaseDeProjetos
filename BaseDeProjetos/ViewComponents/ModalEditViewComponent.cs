using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents
{
    public class ModalEditViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalEditViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = await _context.Prospeccao.FindAsync(id);
            return View(model);
        }
    }
}
