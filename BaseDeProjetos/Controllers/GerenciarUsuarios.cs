using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using BaseDeProjetos.Helpers;
using System.Collections;
using Microsoft.AspNetCore.Http;

namespace BaseDeProjetos.Controllers
{
    public class GerenciarUsuarios : Controller
    {
        private readonly ApplicationDbContext _context;

        public GerenciarUsuarios(ApplicationDbContext context)
        {
            _context = context;
        }

        public Usuario UsuarioExiste(string nomeUsuario)
        {
            string usuarioLogado = nomeUsuario;

            var usuarioExiste = _context.Users.Where(u => u.UserName == usuarioLogado).FirstOrDefault();

            var usuario = _context.Users.Find(usuarioExiste.Id);

            return usuario;
        }

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

        // GET: GerenciarUsuarios
        public IActionResult Index()
        {
            var listaUsuarios = new List<Usuario>();
            Usuario usuario = UsuarioExiste(HttpContext.User.Identity.Name);
            if (usuario != null && VerificarNivelUsuario(usuario))
            {
                listaUsuarios = _context.Users.Where(u => u.Casa == usuario.Casa).ToList();

            }

            return View(listaUsuarios);

        }

        // GET: GerenciarUsuarios/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Users
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
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: GerenciarUsuarios/Edit/5
        public async Task<IActionResult> Edit(string? id)
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
        public async Task<IActionResult> Edit(string id, [Bind("id,UserName,Email,EmailConfirmado,PasswordHash,Casa,Nivel")] Usuario usuario)
        {
            if (id.ToString() != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: GerenciarUsuarios/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Users
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
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(string id)
        {
            return _context.Users.Any(e => e.Id == id.ToString());
        }
    }
}
