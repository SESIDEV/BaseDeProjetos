using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BaseDeProjetos.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Empresas
        public IActionResult Index(string searchString = "")
        {
            ViewBag.Contatos = _context.Pessoa.ToList();
            //Filtros e ordenadores
            if (string.IsNullOrEmpty(searchString) && HttpContext.Session.Keys.Contains("_CurrentFilter"))
            {
                ViewData["CurrentFilter"] = HttpContext.Session.GetString("_CurrentFilter");
            }
            else
            {
                ViewData["CurrentFilter"] = searchString;
                HttpContext.Session.SetString("_CurrentFilter", searchString);
            }

            var empresas = FiltrarEmpresas(searchString, _context.Empresa.OrderBy(e=> e.Nome).ToList());

            return View(empresas);
        }
        private static List<Empresa> FiltrarEmpresas(string searchString, List<Empresa> lista)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                lista = lista.Where(j => j.Segmento != null).
                              Where(s => s.Nome.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                                      || s.Segmento.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            return lista;
        }


        // GET: Empresas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Empresa empresa = await _context.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // GET: Empresas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empresas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Logo,Nome,CNPJ,Segmento,Estado")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                empresa.CNPJ = Regex.Replace(empresa.CNPJ, "[^0-9]", "");
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            return View(empresa);
        }

        // GET: Empresas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Empresa empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Nome,CNPJ,Segmento,Estado")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    empresa.CNPJ = Regex.Replace(empresa.CNPJ, "[^0-9]", "");
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
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
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Empresa empresa = await _context.Empresa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Empresa empresa = await _context.Empresa.FindAsync(id);
            IQueryable<Pessoa> pessoas_a_remover = _context.Pessoa.Where(p => p.empresa.Id == empresa.Id);
            _context.Pessoa.RemoveRange(pessoas_a_remover);
            _context.Empresa.Remove(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpresaExists(int id)
        {
            return _context.Empresa.Any(e => e.Id == id);
        }

        /*public async Task<IActionResult> RetornarKPIsEmbrapii()
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> RetornarKPIsFraunhofer()
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> IncluirCronograma()
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> AtualizarCronograma()
        {
            throw new NotImplementedException();
        }*/
    }
}