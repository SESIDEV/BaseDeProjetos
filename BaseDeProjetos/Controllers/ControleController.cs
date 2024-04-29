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
    public class ControleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ControleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Controles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Controle.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Detalhes()
        {
            var controle = await _context.Controle.FirstOrDefaultAsync(c => c.Codigo == Request.Form["codigo"].ToString());

            if (controle == null)
            {
                return NotFound();
            }

            return View("Details", controle);

        }

        // GET: Controles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controle = await _context.Controle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (controle == null)
            {
                return NotFound();
            }

            return View(controle);
        }

        // GET: Controles/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Usuarios"] = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
            return View();
        }

        // POST: Controles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Item,Projeto,Anotações,Data")] Controle controle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(controle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(controle);
        }

        // GET: Controles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controle = await _context.Controle.FindAsync(id);
            if (controle == null)
            {
                return NotFound();
            }
            return View(controle);
        }

        // POST: Controles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Item,Projeto,Anotações,Data")] Controle controle)
        {
            if (id != controle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(controle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControleExists(controle.Id))
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
            return View(controle);
        }

        // GET: Controles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var controle = await _context.Controle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (controle == null)
            {
                return NotFound();
            }

            return View(controle);
        }

        // POST: Controles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var controle = await _context.Controle.FindAsync(id);
            _context.Controle.Remove(controle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ControleExists(int id)
        {
            return _context.Controle.Any(e => e.Id == id);
        }
    }
}
