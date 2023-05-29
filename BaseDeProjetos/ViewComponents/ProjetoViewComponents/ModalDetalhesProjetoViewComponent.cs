using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.ProjetoViewComponents
{
    public class ModalDetalhesProjetoViewComponent : ViewComponent

    {
        private readonly ApplicationDbContext _context;

        public ModalDetalhesProjetoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
            ViewData["NivelUsuario"] = usuario.Nivel;
            var model = await _context.Projeto.FindAsync(id);
            return View(model);
        }
    }
}
