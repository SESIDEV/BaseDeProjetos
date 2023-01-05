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
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

            return View();
        }
        public string dados(){
            
            return Helpers.Helpers.PuxarDadosUsuarios(_context);

        }

        public string usuarioAtivo(){
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            return usuario.UserName;
        }
    }
}