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
    public class TroubleshootsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TroubleshootsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Troubleshoots
        public async Task<IActionResult> Index()
        {
            return View(await _context.Troubleshoot.ToListAsync());
        }

        // GET: Troubleshoots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var troubleshoot = await _context.Troubleshoot
                .FirstOrDefaultAsync(m => m.Id == id);
            if (troubleshoot == null)
            {
                return NotFound();
            }

            return View(troubleshoot);
        }

        // GET: Troubleshoots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Troubleshoots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tag,Titulo,Descricao,Data,Status")] Troubleshoot troubleshoot)
        {
            if (ModelState.IsValid)
            {
                troubleshoot.Data = DateTime.Now;
                _context.Add(troubleshoot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(troubleshoot);
        }

        // GET: Troubleshoots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var troubleshoot = await _context.Troubleshoot.FindAsync(id);
            if (troubleshoot == null)
            {
                return NotFound();
            }
            return View(troubleshoot);
        }

        // POST: Troubleshoots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tag,Titulo,Descricao,Data,Status")] Troubleshoot troubleshoot)
        {
            if (id != troubleshoot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(troubleshoot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TroubleshootExists(troubleshoot.Id))
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
            return View(troubleshoot);
        }

        // GET: Troubleshoots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var troubleshoot = await _context.Troubleshoot
                .FirstOrDefaultAsync(m => m.Id == id);
            if (troubleshoot == null)
            {
                return NotFound();
            }

            return View(troubleshoot);
        }

        // POST: Troubleshoots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var troubleshoot = await _context.Troubleshoot.FindAsync(id);
            _context.Troubleshoot.Remove(troubleshoot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TroubleshootExists(int id)
        {
            return _context.Troubleshoot.Any(e => e.Id == id);
        }
    }
}
