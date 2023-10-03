using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.CargoViewComponents
{
    public class ModalDeleteCargoViewComponent : ViewComponent
    {
        public readonly ApplicationDbContext _context;

        public ModalDeleteCargoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = await _context.Cargo.FindAsync(int.Parse(id));
            return View(model);
        }
    }
}