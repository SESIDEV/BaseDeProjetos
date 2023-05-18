using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using BaseDeProjetos.Models;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.EditalViewComponents
{
    public class ModalCreateSubmissaoEditalViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalCreateSubmissaoEditalViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new Submissao());
        }

    }
}
