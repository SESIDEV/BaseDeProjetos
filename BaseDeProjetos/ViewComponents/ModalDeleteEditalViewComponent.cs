using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BaseDeProjetos.Models;
using System;

namespace BaseDeProjetos.ViewComponents
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
            var model = await _context.Editais.FindAsync(Int32.Parse(id));
            return View(model);
        }
    }
}
