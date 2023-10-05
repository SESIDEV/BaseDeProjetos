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
    public class CurvaFisicoFinanceiraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CurvaFisicoFinanceiraController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CurvaFisicoFinanceira
        public async Task<IActionResult> Index()
        {
            return View(await _context.StatusCurva.ToListAsync());
        }

        // GET: CurvaFisicoFinanceira/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curvaFisicoFinanceira = await _context.StatusCurva
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curvaFisicoFinanceira == null)
            {
                return NotFound();
            }

            return View(curvaFisicoFinanceira);
        }

        // GET: CurvaFisicoFinanceira/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CurvaFisicoFinanceira/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,PercentualFisico,PercentualFinanceiro")] CurvaFisicoFinanceira curvaFisicoFinanceira)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curvaFisicoFinanceira);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(curvaFisicoFinanceira);
        }

        // GET: CurvaFisicoFinanceira/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curvaFisicoFinanceira = await _context.StatusCurva.FindAsync(id);
            if (curvaFisicoFinanceira == null)
            {
                return NotFound();
            }
            return View(curvaFisicoFinanceira);
        }

        // POST: CurvaFisicoFinanceira/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,PercentualFisico,PercentualFinanceiro")] CurvaFisicoFinanceira curvaFisicoFinanceira)
        {
            if (id != curvaFisicoFinanceira.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curvaFisicoFinanceira);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurvaFisicoFinanceiraExists(curvaFisicoFinanceira.Id))
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
            return View(curvaFisicoFinanceira);
        }

        // GET: CurvaFisicoFinanceira/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curvaFisicoFinanceira = await _context.StatusCurva
                .FirstOrDefaultAsync(m => m.Id == id);
            if (curvaFisicoFinanceira == null)
            {
                return NotFound();
            }

            return View(curvaFisicoFinanceira);
        }

        // POST: CurvaFisicoFinanceira/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curvaFisicoFinanceira = await _context.StatusCurva.FindAsync(id);
            _context.StatusCurva.Remove(curvaFisicoFinanceira);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurvaFisicoFinanceiraExists(int id)
        {
            return _context.StatusCurva.Any(e => e.Id == id);
        }
    }
}
