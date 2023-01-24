using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using BaseDeProjetos.Helpers;
using Microsoft.AspNetCore.Mvc;


namespace BaseDeProjetos.Controllers
{
    public class PessoasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PessoasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pessoas
        [Route("Pessoas")]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                return View();
            } 
            else
            {
                return View("Forbidden");
            }
        }
        public string dados()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return Helpers.Helpers.PuxarDadosUsuarios(_context);
            else
                return "403 Forbidden";

        }

        public string usuarioAtivo()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

                return usuario.UserName;
            } 
            else
            {
                return "403 Forbidden";
            }
        }
    }
}