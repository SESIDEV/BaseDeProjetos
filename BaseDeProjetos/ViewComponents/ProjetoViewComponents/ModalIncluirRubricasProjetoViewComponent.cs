using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.ProjetoViewComponents
{
    public class ModalIncluirRubricasProjetoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalIncluirRubricasProjetoViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            ViewData["Projeto"] = await _context.Projeto.FindAsync(id);
            return View(new Rubrica());
        }
    }
}