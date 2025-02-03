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
        private readonly DbCache _cache;

        public ModalDetalhesProjetoViewComponent(ApplicationDbContext context, DbCache cache)
        {
            _cache = cache;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(HttpContext);
            ViewData["NivelUsuario"] = usuario.Nivel;
            var model = await _context.Projeto.FindAsync(id);
            return View(model);
        }
    }
}