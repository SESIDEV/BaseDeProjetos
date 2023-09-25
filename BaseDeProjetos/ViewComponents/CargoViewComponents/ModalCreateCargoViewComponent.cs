using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using BaseDeProjetos.Models;

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
