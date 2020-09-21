using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaseDeProjetos.Data;
using SmartTesting.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BaseDeProjetos.Controllers
{
    public class EntregasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EntregasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Entregas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Entrega.Include(e => e.projeto);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Entregas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrega = await _context.Entrega
                .Include(e => e.projeto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrega == null)
            {
                return NotFound();
            }

            return View(entrega);
        }

        // GET: Entregas/Create
        public IActionResult Create()
        {
            ViewData["projetoId"] = new SelectList(_context.Projeto, "Id", "NomeProjeto");
            return View();
        }

        // POST: Entregas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,projetoId,NomeEntrega,DescricaoEntrega,DataInicio,DataFim,Concluida")] Entrega entrega)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entrega);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["projetoId"] = new SelectList(_context.Projeto, "Id", "Id", entrega.projetoId);
            return View(entrega);
        }

        // GET: Entregas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrega = await _context.Entrega.FindAsync(id);
            if (entrega == null)
            {
                return NotFound();
            }
            ViewData["projetoId"] = new SelectList(_context.Projeto, "Id", "Id", entrega.projetoId);
            return View(entrega);
        }

        // POST: Entregas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,projetoId,NomeEntrega,DescricaoEntrega,DataInicio,DataFim,Concluida")] Entrega entrega)
        {
            if (id != entrega.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entrega);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntregaExists(entrega.Id))
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
            ViewData["projetoId"] = new SelectList(_context.Projeto, "Id", "Id", entrega.projetoId);
            return View(entrega);
        }

        // GET: Entregas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entrega = await _context.Entrega
                .Include(e => e.projeto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrega == null)
            {
                return NotFound();
            }

            return View(entrega);
        }

        // POST: Entregas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entrega = await _context.Entrega.FindAsync(id);
            _context.Entrega.Remove(entrega);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntregaExists(int id)
        {
            return _context.Entrega.Any(e => e.Id == id);
        }
        [HttpPost]
        public IActionResult CarregarEntregas(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            if (files.Count < 1)
            {
                return Content("Não foram submetidos dados");
            }

            foreach (IFormFile formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (StreamReader file = new StreamReader(formFile.OpenReadStream()))
                    {
                        CriarEntregas(file);
                    }
                }
            }

            return View("Index", "Home");
        }

        private void CriarEntregas(StreamReader file)
        {
            throw new NotImplementedException();
        }
    }
}
