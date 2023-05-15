using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using BaseDeProjetos.Models;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents
{
    public class ModalCreateEmpresaViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(new Empresa());
        }

    }
}
