using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BaseDeProjetos.Models;

namespace BaseDeProjetos.ViewComponents
{
    public class ModalCreateViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalCreateViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new Prospeccao(new FollowUp()));
        }
    }
}
