using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BaseDeProjetos.Controllers
{
    public class SGIController: Controller
    {
        public Usuario usuarioAtivo { get; set; }

        public void ViewbagizarUsuario(ApplicationDbContext _context)
        {
            this.usuarioAtivo = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
            ViewBag.usuarioCasa = usuarioAtivo.Casa;
            ViewBag.usuarioNivel = usuarioAtivo.Nivel;

        }
    }
}