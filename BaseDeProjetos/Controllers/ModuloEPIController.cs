using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;

namespace BaseDeProjetos.Controllers
{
    public class ModuloEPIController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModuloEPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ModuloEPI
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RegistroEPI.Include(r => r.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ModuloEPI/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroEPI = await _context.RegistroEPI
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registroEPI == null)
            {
                return NotFound();
            }

            return View(registroEPI);
        }

        // GET: ModuloEPI/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ModuloEPI/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DataEntrega,UnidadeOperacional,UsuarioId,Justificativa")] RegistroEPI registroEPI)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registroEPI);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", registroEPI.UsuarioId);
            return View(registroEPI);
        }

        // GET: ModuloEPI/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroEPI = await _context.RegistroEPI.FindAsync(id);
            if (registroEPI == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", registroEPI.UsuarioId);
            return View(registroEPI);
        }

        // POST: ModuloEPI/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataEntrega,UnidadeOperacional,UsuarioId,Justificativa")] RegistroEPI registroEPI)
        {
            if (id != registroEPI.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registroEPI);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroEPIExists(registroEPI.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Id", registroEPI.UsuarioId);
            return View(registroEPI);
        }

        // GET: ModuloEPI/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroEPI = await _context.RegistroEPI
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registroEPI == null)
            {
                return NotFound();
            }

            return View(registroEPI);
        }

        // POST: ModuloEPI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registroEPI = await _context.RegistroEPI.FindAsync(id);
            _context.RegistroEPI.Remove(registroEPI);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistroEPIExists(int id)
        {
            return _context.RegistroEPI.Any(e => e.Id == id);
        }
    }
}
