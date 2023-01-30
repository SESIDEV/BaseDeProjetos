using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents
{
    public class ModalEditEditalViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalEditEditalViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        { 
            var model = await _context.Editais.FindAsync(Int32.Parse(id));
            return View(model);
        }
    }
}
