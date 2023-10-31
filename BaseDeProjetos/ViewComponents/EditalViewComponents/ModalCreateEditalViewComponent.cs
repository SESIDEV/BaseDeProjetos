using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaseDeProjetos.ViewComponents.EditalViewComponents
{
    public class ModalCreateEditalViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalCreateEditalViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(new Editais());
        }
    }
}