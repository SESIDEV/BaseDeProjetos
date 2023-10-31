using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.EditalViewComponents
{
    public class ModalDeleteEditalViewComponent : ViewComponent
    {
        public readonly ApplicationDbContext _context;

        public ModalDeleteEditalViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = await _context.Editais.FindAsync(int.Parse(id));
            return View(model);
        }
    }
}