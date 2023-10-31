using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaseDeProjetos.ViewComponents.ProjetoViewComponents
{
    public class ModalCreateProjetoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(new Projeto(new ProjetoIndicadores()));
        }
    }
}