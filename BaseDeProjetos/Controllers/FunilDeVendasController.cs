﻿using System;
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
    public class FunilDeVendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FunilDeVendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FunilDeVendas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Prospeccao.ToListAsync());
        }

        // GET: FunilDeVendas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prospeccao = await _context.Prospeccao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prospeccao == null)
            {
                return NotFound();
            }

            return View(prospeccao);
        }

        // GET: FunilDeVendas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FunilDeVendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoContratacao,LinhaPequisa")] Prospeccao prospeccao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prospeccao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prospeccao);
        }

        // GET: FunilDeVendas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prospeccao = await _context.Prospeccao.FindAsync(id);
            if (prospeccao == null)
            {
                return NotFound();
            }
            return View(prospeccao);
        }

        // POST: FunilDeVendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,TipoContratacao,LinhaPequisa")] Prospeccao prospeccao)
        {
            if (id != prospeccao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prospeccao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProspeccaoExists(prospeccao.Id))
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
            return View(prospeccao);
        }

        // GET: FunilDeVendas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prospeccao = await _context.Prospeccao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prospeccao == null)
            {
                return NotFound();
            }

            return View(prospeccao);
        }

        // POST: FunilDeVendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var prospeccao = await _context.Prospeccao.FindAsync(id);
            _context.Prospeccao.Remove(prospeccao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProspeccaoExists(string id)
        {
            return _context.Prospeccao.Any(e => e.Id == id);
        }
    }
}
