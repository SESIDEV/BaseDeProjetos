using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
