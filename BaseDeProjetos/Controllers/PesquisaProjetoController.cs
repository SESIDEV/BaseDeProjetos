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
    public class PesquisaProjetoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PesquisaProjetoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PesquisaProjeto
        public async Task<IActionResult> Index()
        {
            return View(await _context.PesquisaProjeto.ToListAsync());
        }

        // GET: PesquisaProjeto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pesquisaProjeto = await _context.PesquisaProjeto
                .FirstOrDefaultAsync(m => m.IdPesquisa == id);
            if (pesquisaProjeto == null)
            {
                return NotFound();
            }

            return View(pesquisaProjeto);
        }

        // GET: PesquisaProjeto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PesquisaProjeto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPesquisa,ProjetoId,ResultadoFinal")] PesquisaProjeto pesquisaProjeto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pesquisaProjeto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pesquisaProjeto);
        }

        // GET: PesquisaProjeto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pesquisaProjeto = await _context.PesquisaProjeto.FindAsync(id);
            if (pesquisaProjeto == null)
            {
                return NotFound();
            }
            return View(pesquisaProjeto);
        }

        // POST: PesquisaProjeto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPesquisa,ProjetoId,ResultadoFinal")] PesquisaProjeto pesquisaProjeto)
        {
            if (id != pesquisaProjeto.IdPesquisa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pesquisaProjeto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PesquisaProjetoExists(pesquisaProjeto.IdPesquisa))
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
            return View(pesquisaProjeto);
        }

        // GET: PesquisaProjeto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pesquisaProjeto = await _context.PesquisaProjeto
                .FirstOrDefaultAsync(m => m.IdPesquisa == id);
            if (pesquisaProjeto == null)
            {
                return NotFound();
            }

            return View(pesquisaProjeto);
        }

        // POST: PesquisaProjeto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pesquisaProjeto = await _context.PesquisaProjeto.FindAsync(id);
            _context.PesquisaProjeto.Remove(pesquisaProjeto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PesquisaProjetoExists(int id)
        {
            return _context.PesquisaProjeto.Any(e => e.IdPesquisa == id);
        }
    }
}
