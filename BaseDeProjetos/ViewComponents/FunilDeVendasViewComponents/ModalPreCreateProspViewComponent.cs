using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalPreCreateProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ModalPreCreateProspViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ProspeccaoModalPreCreateProspDTO> prosp = await _context.Prospeccao.Select(p => new ProspeccaoModalPreCreateProspDTO { Empresa = p.Empresa, Status = p.Status, Usuario = p.Usuario, Id = p.Id }).ToListAsync();

            ViewData["prospPlan"] = prosp;

            return View(new Prospeccao(new FollowUp()));
        }
    }
}