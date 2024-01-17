using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.ViewComponents.FunilDeVendasViewComponents
{
    public class ModalPreCreateProspViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly DbCache _cache;

        public ModalPreCreateProspViewComponent(ApplicationDbContext context, DbCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var UsuarioAtivo = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext, _cache);

            if (UsuarioAtivo != null)
            {
                ViewBag.usuarioCasa = UsuarioAtivo.Casa;
                ViewBag.usuarioNivel = UsuarioAtivo.Nivel;
            }
            else
            {
                throw new ArgumentNullException(nameof(UsuarioAtivo));
            }

            var listaPlanejados = await _context.Prospeccao
                .Where(p => p.Status.All(followup => followup.Status == StatusProspeccao.Planejada) && p.Usuario.Id == UsuarioAtivo.Id)
                .Select(p => new SelectListItem { Text = p.Empresa.Nome, Value = p.Id.ToString() })
                .ToListAsync();

            SelectList planejadas = new SelectList(listaPlanejados, "Value", "Text");

            ViewData["selectPlanejadas"] = planejadas;

            return View(new Prospeccao(new FollowUp()));
        }
    }
}