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

        /*public static List<Usuario> DefinirCasaParaVisualizarListaDeUsuarios(string? casa, Usuario usuario, ApplicationDbContext _context, HttpContext HttpContext, ViewDataDictionary ViewData)
        {
            Instituto enum_casa;

            List<Usuario> usuarios = new List<Usuario>();

            if (usuario.Nivel == Nivel.Dev)
            {
                List<Usuario> lista = _context.Users.ToList();
                usuarios.AddRange(lista);
            }

            else
            {

                if (Enum.IsDefined(typeof(Instituto), casa))
                {
                    HttpContext.Session.SetString("_Casa", casa);
                    enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
                    List<Usuario> lista = _context.Users.Where(usuario => usuario.Casa.Equals(enum_casa)).ToList();

                    usuarios.AddRange(lista);

                    ViewData["Area"] = casa;
                }
            }

            return usuarios.ToList();
        }*/

        // GET: GerenciarUsuarios
        public async Task<IActionResult> Index()
        {
            /*if (httpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                if (string.IsNullOrEmpty(casa))
                {
                    casa = usuario.Casa.ToString();
                }

                List<Usuario> lista;

                return View(await _context.Users.ToListAsync());
            }

            else
            {
                return View("Forbidden");
            }
            */
            return View(await _context.Users.ToListAsync());
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
