using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.ProjetoViewComponents
{
    public class ModalIncluirMacroentregaProjetoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalIncluirMacroentregaProjetoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            ViewData["Projeto"] = await _context.Projeto.FindAsync(id);
            return View();
        }        
    }
}
