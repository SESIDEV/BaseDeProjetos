using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BaseDeProjetos.Controllers
{
    public class SGIController : Controller
    {
        public Usuario UsuarioAtivo { get; set; }

        public void ViewbagizarUsuario(ApplicationDbContext _context)
        {
            UsuarioAtivo = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            if (UsuarioAtivo != null)
            {
                ViewBag.usuarioCasa = UsuarioAtivo.Casa;
                ViewBag.usuarioNivel = UsuarioAtivo.Nivel;
            }
            else
            {
                throw new ArgumentNullException(nameof(UsuarioAtivo));
            }
        }
    }
}