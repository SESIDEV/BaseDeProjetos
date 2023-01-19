using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents
{
	public class ModalCreateProjViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;

		public ModalCreateProjViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View();
		}
	}
}
