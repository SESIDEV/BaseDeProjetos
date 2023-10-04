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
    public class CodigoAmostraProjetoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CodigoAmostraProjetoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CodigoAmostraProjeto
        public async Task<IActionResult> Index()
        {
            return View(await _context.CodigoAmostraProjeto.ToListAsync());
        }

        // GET: CodigoAmostraProjeto/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codigoAmostraProjeto = await _context.CodigoAmostraProjeto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codigoAmostraProjeto == null)
            {
                return NotFound();
            }

            return View(codigoAmostraProjeto);
        }

        // GET: CodigoAmostraProjeto/Create
        public IActionResult Create()
        {
            List<Projeto> projetos = _context.Projeto.ToList();
            ViewData["Projetos"] = new SelectList(projetos, "Id", "NomeProjeto");
            return View();
        }

        // POST: CodigoAmostraProjeto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Projeto,Codigo")] CodigoAmostraProjeto codigoAmostraProjeto)
        {

            if (_context.CodigoAmostraProjeto.Any(c => c.Codigo == codigoAmostraProjeto.Codigo))
            {
                throw new InvalidOperationException("Já existe um código com este valor.");
            }

            if (ModelState.IsValid)
            {
                Projeto proj = _context.Projeto.First(p => p.Id == codigoAmostraProjeto.Projeto.Id);
                codigoAmostraProjeto.Projeto = proj;
                _context.Add(codigoAmostraProjeto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(codigoAmostraProjeto);
        }

        // GET: CodigoAmostraProjeto/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var codigoAmostraProjeto = await _context.CodigoAmostraProjeto.FindAsync(id);
            if (codigoAmostraProjeto == null)
            {
                return NotFound();
            }
            List<Projeto> projetos = _context.Projeto.ToList();
            ViewData["Projetos"] = new SelectList(projetos, "Id", "NomeProjeto");
            return View(codigoAmostraProjeto);
        }

        // POST: CodigoAmostraProjeto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Projeto,Codigo")] CodigoAmostraProjeto codigoAmostraProjeto)
        {
            if (id != codigoAmostraProjeto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Projeto proj = _context.Projeto.First(p => p.Id == codigoAmostraProjeto.Projeto.Id);
                    codigoAmostraProjeto.Projeto = proj;
                    _context.Update(codigoAmostraProjeto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CodigoAmostraProjetoExists(codigoAmostraProjeto.Id))
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
            return View(codigoAmostraProjeto);
        }

        // GET: CodigoAmostraProjeto/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var codigoAmostraProjeto = await _context.CodigoAmostraProjeto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (codigoAmostraProjeto == null)
            {
                return NotFound();
            }

            return View(codigoAmostraProjeto);
        }

        // POST: CodigoAmostraProjeto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var codigoAmostraProjeto = await _context.CodigoAmostraProjeto.FindAsync(id);
            _context.CodigoAmostraProjeto.Remove(codigoAmostraProjeto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CodigoAmostraProjetoExists(int id)
        {
            return _context.CodigoAmostraProjeto.Any(e => e.Id == id);
        }
    }
}
