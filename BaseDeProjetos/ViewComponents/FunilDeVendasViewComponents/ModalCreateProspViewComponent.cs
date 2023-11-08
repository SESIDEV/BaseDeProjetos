using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalCreateProspViewComponent : ViewComponent
    {
        public ModalCreateProspViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            return View(new Prospeccao(new FollowUp()));
        }
    }
}