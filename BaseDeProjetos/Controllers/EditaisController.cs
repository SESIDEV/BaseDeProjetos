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

namespace BaseDeProjetos.Controllers
{
    public class EditaisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EditaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Editais
        public async Task<IActionResult> Index()
        {
			Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
            List<Empresa> empresas = _context.Empresa.ToList();
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
            ViewBag.usuarioCasa = usuario.Casa;
			ViewBag.usuarioNivel = usuario.Nivel;
			return View(await _context.Editais.ToListAsync());
        }

        // GET: Editais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
			Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

			ViewBag.usuarioCasa = usuario.Casa;
			ViewBag.usuarioNivel = usuario.Nivel;

			if (id == null)
            {
                return NotFound();
            }

            var editais = await _context.Editais
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editais == null)
            {
                return NotFound();
            }

            return View(editais);
        }

        // GET: Editais/Create
        public IActionResult Create()
        {
			Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

			ViewBag.usuarioCasa = usuario.Casa;
			ViewBag.usuarioNivel = usuario.Nivel;

			return View();
        }

        // POST: Editais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Descricao,Local,StatusEdital,AgenciaFomento,PrazoSubmissao,ValorEdital,Proponente,LinkEdital,ProjetoProposto,DataResultado,StatusSubmissao")] Editais editais)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editais);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editais);
        }

        // GET: Editais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
			Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

			ViewBag.usuarioCasa = usuario.Casa;
			ViewBag.usuarioNivel = usuario.Nivel;

			if (id == null)
            {
                return NotFound();
            }

            var editais = await _context.Editais.FindAsync(id);
            if (editais == null)
            {
                return NotFound();
            }
            return View(editais);
        }

        // POST: Editais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Descricao,Local,StatusEdital,AgenciaFomento,PrazoSubmissao,ValorEdital,Proponente,LinkEdital,ProjetoProposto,DataResultado,StatusSubmissao")] Editais editais)
        {
            if (id != editais.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editais);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditaisExists(editais.Id))
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
            return View(editais);
        }

        // GET: Editais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
			Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

			ViewBag.usuarioCasa = usuario.Casa;
			ViewBag.usuarioNivel = usuario.Nivel;

			if (id == null)
            {
                return NotFound();
            }

            var editais = await _context.Editais
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editais == null)
            {
                return NotFound();
            }

            return View(editais);
        }

        // POST: Editais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var editais = await _context.Editais.FindAsync(id);
            _context.Editais.Remove(editais);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EditaisExists(int id)
        {
            return _context.Editais.Any(e => e.Id == id);
        }
    }
}
