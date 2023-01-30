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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml.Drawing;

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

        public List<Usuario> VerificarEmailAtivo(string statusEmailUsuario)
        {
            var listaUsuarios = _context.Users.ToList();

            switch (statusEmailUsuario)
            {
                case "1":
                    listaUsuarios = _context.Users.Where(u => u.EmailConfirmed).ToList();
                    break;
                case "0":
                    listaUsuarios = _context.Users.Where(u => !u.EmailConfirmed).ToList();
                    break;
                default:
                    listaUsuarios = _context.Users.ToList();
                    break;
            }

            return listaUsuarios;

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
        [Route("GerenciarUsuarios")]
        [Route("GerenciarUsuarios/Index")]
        [Route("GerenciarUsuarios/Index/{id?}")]
        // GET: GerenciarUsuarios
        public IActionResult Index(string? statusEmailUsuario)
        {
            ViewData["CurrentFilter"] = statusEmailUsuario;

            var lista = _context.Users.AsNoTracking().ToList();

            Usuario usuario = UsuarioExiste(HttpContext.User.Identity.Name);

            if (usuario != null && VerificarNivelUsuario(usuario))
            {

                lista = VerificarEmailAtivo(statusEmailUsuario);

            }

            return View(lista);

        }

        // GET: GerenciarUsuarios/Details/5
        public async Task<IActionResult> Details(string? id)
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
            ViewData["SenhaDefault"] = "AQAAAAEAACcQAAAAEEJOLHMoQRfLBTu8K2wwcFq91QZqkhyVQyP1TpvtsZ5/6jd5CP6jpEEL0bcpUjKvpg==";

            if (ModelState.IsValid)
            {
                usuario.PasswordHash = "AQAAAAEAACcQAAAAEEJOLHMoQRfLBTu8K2wwcFq91QZqkhyVQyP1TpvtsZ5/6jd5CP6jpEEL0bcpUjKvpg==";

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: GerenciarUsuarios/Edit/5
        [HttpGet]
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,Casa,Nivel,PasswordHash")] Usuario usuario)
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
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
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
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(string id)
        {
            return _context.Users.AsNoTracking().Any(e => e.Id == id.ToString());
        }
    }
}
