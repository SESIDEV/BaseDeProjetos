using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaseDeProjetos.ViewComponents.ProducaoViewComponents
{
    public class ModalCreateProducaoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new Producao());
        }
    }
}