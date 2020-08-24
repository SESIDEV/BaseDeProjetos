using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BaseDeProjetos.Controllers
{
    public class ProjetosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjetosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult PopularBase()
        {
            string[] _base;
            using(var file = new StreamReader("~/Base.csv"))
            {
                _base = file.ReadToEnd().Split("\n");
            }


            return View(nameof(Index));
        }

        // GET: Projetos
        public async Task<IActionResult> Index(string casa)
        {
            if (casa is null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Casa")))
                {
                    casa = HttpContext.Session.GetString("_Casa");
                }
                else { return View(await _context.Projeto.ToListAsync());}
            }
            else if (Enum.IsDefined(typeof(Casa), casa))
            {
                HttpContext.Session.SetString("_Casa", casa);
            }
            else
            {
                return NotFound();
            }

            ViewData["Area"] = casa;
            var enum_casa = (Casa)Enum.Parse(typeof(Casa), casa);
            var lista = await _context.Projeto.Where(p => p.Casa.Equals(enum_casa)).ToListAsync();
            return View(lista);
        }

        // GET: Projetos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projeto = await _context.Projeto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projeto == null)
            {
                return NotFound();
            }

            return View(projeto);
        }

        // GET: Projetos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projetos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeProjeto,Casa, AreaPesquisa,DataInicio,DataEncerramento,Estado,FonteFomento,Inovacao,status,DuracaoProjetoEmMeses,ValorTotalProjeto,ValorAporteRecursos")] Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projeto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projeto);
        }

        // GET: Projetos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projeto = await _context.Projeto.FindAsync(id);
            if (projeto == null)
            {
                return NotFound();
            }
            return View(projeto);
        }

        // POST: Projetos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,NomeProjeto,AreaPesquisa,DataInicio,DataEncerramento,Estado,FonteFomento,Inovacao,status,DuracaoProjetoEmMeses,ValorTotalProjeto,ValorAporteRecursos")] Projeto projeto)
        {
            if (id != projeto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projeto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjetoExists(projeto.Id))
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
            return View(projeto);
        }

        // GET: Projetos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projeto = await _context.Projeto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projeto == null)
            {
                return NotFound();
            }

            return View(projeto);
        }

        // POST: Projetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var projeto = await _context.Projeto.FindAsync(id);
            _context.Projeto.Remove(projeto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjetoExists(string id)
        {
            return _context.Projeto.Any(e => e.Id == id);
        }

        private void DetachLocal(Func<Usuario, bool> predicate)
        {
            var local = _context.Set<Usuario>().Local.Where(predicate).FirstOrDefault();
            if (!(local is null))
            {
                _context.Entry(local).State = EntityState.Detached;
            }
        }
    }
}
