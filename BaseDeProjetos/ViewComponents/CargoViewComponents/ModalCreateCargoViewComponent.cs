using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaseDeProjetos.ViewComponents.CargoViewComponents
{
    public class ModalCreateCargoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new Cargo());
        }
    }
}