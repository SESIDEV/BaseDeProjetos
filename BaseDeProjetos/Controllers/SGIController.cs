using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BaseDeProjetos.Controllers
{
    public class SGIController: Controller
    {
        public Usuario UsuarioAtivo { get; set; }

        public void ViewbagizarUsuario(ApplicationDbContext _context)
        {
            UsuarioAtivo = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
            ViewBag.usuarioCasa = UsuarioAtivo.Casa;
            ViewBag.usuarioNivel = UsuarioAtivo.Nivel;

        }
    }
}