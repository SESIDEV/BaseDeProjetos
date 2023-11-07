using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    public class GerenciarUsuarios : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly DbCache _cache;

        public GerenciarUsuarios(ApplicationDbContext context, DbCache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Verifica se um usuário (de acordo com uma string) existe no Banco de Dados
        /// </summary>
        /// <param name="nomeUsuario">Nome do Usuário</param>
        /// <returns></returns>
        public async Task<Usuario> UsuarioExiste(string nomeUsuario)
        {
            var usuarios = await _cache.GetCachedAsync("AllUsuarios", () => _context.Users.ToListAsync());
            return usuarios.FirstOrDefault(u => u.UserName == nomeUsuario);
        }

        /// <summary>
        /// Verifica e retorna quais usuários possuem email ativo
        /// </summary>
        /// <param name="statusEmailUsuario">Status que se deseja obter (1 confirmado, 0 não confirmado)</param>
        /// <returns></returns>
        public async Task<List<Usuario>> VerificarStatusEmailUsuario(string statusEmailUsuario)
        {
            var usuarios = await _cache.GetCachedAsync("AllUsuarios", () => _context.Users.ToListAsync());


            usuarios = statusEmailUsuario switch
            {
                "1" => _context.Users.Where(u => u.EmailConfirmed).ToList(),
                "0" => _context.Users.Where(u => !u.EmailConfirmed).ToList(),
                _ => _context.Users.ToList(),
            };
            return usuarios;
        }

        /// <summary>
        /// Verifica o nível de um usuário
        /// </summary>
        /// <param name="usuario">Usuário a ser passado como parâmetro</param>
        /// <returns></returns>
        public bool VerificarNivelUsuario(Usuario usuario)
        {
            if (usuario.Nivel == Nivel.PMO || usuario.Nivel == Nivel.Dev || usuario.Nivel == Nivel.Supervisor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Route("GerenciarUsuarios")]
        [Route("GerenciarUsuarios/Index")]
        [Route("GerenciarUsuarios/Index/{id?}")]
        public async Task<IActionResult> Index(string statusEmailUsuario)
        {
            ViewData["CurrentFilter"] = statusEmailUsuario;

            var usuarios = await _cache.GetCachedAsync("AllUsuarios", () => _context.Users.ToListAsync());

            Usuario usuario = await UsuarioExiste(HttpContext.User.Identity.Name);

            if (usuario != null && VerificarNivelUsuario(usuario))
            {
                usuarios = await VerificarStatusEmailUsuario(statusEmailUsuario);
            }

            return View(usuarios);
        }

        // GET: GerenciarUsuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: GerenciarUsuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GerenciarUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,UserName,Email,EmailConfirmado,PasswordHash,Casa,Nivel")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.PasswordHash = "AQAAAAEAACcQAAAAEEJOLHMoQRfLBTu8K2wwcFq91QZqkhyVQyP1TpvtsZ5/6jd5CP6jpEEL0bcpUjKvpg==";

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                await CacheHelper.CleanupUsuariosCache(_cache);
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: GerenciarUsuarios/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: GerenciarUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email, EmailConfirmed,Casa,Nivel,PasswordHash")] Usuario usuario)
        {
            Usuario usuarioEditado = usuario;

            if (id.ToString() != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioExiste = await _context.Users.FindAsync(id);
                    _context.Users.Remove(usuarioExiste);
                    _context.Add(usuarioEditado);
                    await _context.SaveChangesAsync();
                    await CacheHelper.CleanupUsuariosCache(_cache);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(usuarioEditado);
        }

        // GET: GerenciarUsuarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: GerenciarUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usuario = await _context.Users.FindAsync(id);
            _context.Users.Remove(usuario);
            await _context.SaveChangesAsync();
            await CacheHelper.CleanupUsuariosCache(_cache);
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(string id)
        {
            return _context.Users.AsNoTracking().Any(e => e.Id == id.ToString());
        }
    }
}