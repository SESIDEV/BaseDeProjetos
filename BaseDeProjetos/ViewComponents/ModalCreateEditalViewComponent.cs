using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using BaseDeProjetos.Models;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents
{
	public class ModalCreateEditalViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;

		public ModalCreateEditalViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(new Editais());
		}

	}
}
