using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.ProjetoViewComponents
{
    public class ModalDetalhesIndicadorProjetoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalDetalhesIndicadorProjetoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var model = await _context.Projeto.FindAsync(id);
            return View(model);
        }
    }
}