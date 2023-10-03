using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents
{
    public class ModalDeleteEmpresaViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalDeleteEmpresaViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var model = await _context.Empresa.FindAsync(id);
            return View(model);
        }
    }
}