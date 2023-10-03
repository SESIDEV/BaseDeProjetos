using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaseDeProjetos.ViewComponents.EditalViewComponents
{
    public class ModalCreateSubmissaoEditalViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalCreateSubmissaoEditalViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(new Submissao());
        }
    }
}