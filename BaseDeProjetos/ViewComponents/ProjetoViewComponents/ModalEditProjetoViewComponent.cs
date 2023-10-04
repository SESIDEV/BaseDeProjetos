using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.ProjetoViewComponents
{
    public class ModalEditProjetoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalEditProjetoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = await _context.Projeto.FindAsync(id);
            var usuarios = _context.Users.ToList();
            ViewData["Usuarios"] = usuarios;
            return View(model);
        }
    }
}