using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents
{
	public class ModalEditFollowupProspViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext _context;
		public ModalEditFollowupProspViewComponent(ApplicationDbContext context)
		{
			_context = context;
		}

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            ViewData["prospeccao"] = await _context.Prospeccao.FindAsync(id);
			Prospeccao prosp = await _context.Prospeccao.FindAsync(id);
			FollowUp follow = prosp.Status.OrderBy(f => f.Data).LastOrDefault();
            return View(follow);
        }
    }
}
