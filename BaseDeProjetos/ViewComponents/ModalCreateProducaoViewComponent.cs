using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using BaseDeProjetos.Models;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents
{
    public class ModalCreateProducaoViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            return View();
        }

    }
}
