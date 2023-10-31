using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaseDeProjetos.ViewComponents
{
    public class ModalCreateEmpresaViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new Empresa());
        }
    }
}