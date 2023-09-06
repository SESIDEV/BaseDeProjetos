using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BaseDeProjetos.Models;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalCreateProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalCreateProspViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }


        public IViewComponentResult Invoke()
        {
            return View(new Prospeccao(new FollowUp()));
        }
    }
}
