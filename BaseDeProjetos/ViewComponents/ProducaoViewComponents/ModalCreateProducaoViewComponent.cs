using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using BaseDeProjetos.Models;
using System.Threading.Tasks;

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
