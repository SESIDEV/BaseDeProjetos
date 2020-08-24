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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Data.Common;

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
        public async Task<IActionResult> Index(string casa)
        {
            if (casa is null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Casa")))
                {
                    casa = HttpContext.Session.GetString("_Casa");
                }
                else { return View(await _context.Prospeccao.ToListAsync()); }
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
            var lista = await _context.Prospeccao.Where(p => p.Casa.Equals(enum_casa)).ToListAsync();
            return View(lista);
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
            ViewData["Empresas"] = new SelectList(_context.Empresa.ToList(), "Id", "Nome");
            return View();
        }

        [HttpGet]
        public IActionResult Atualizar(string id)
        {
            ViewData["origem"] = id;
            ViewData["prosp"] = _context.Prospeccao.FirstOrDefault(p => p.Id == id);
            return View("CriarFollowUp");
        }
        [HttpPost]
        public async Task<IActionResult> Atualizar([Bind("OrigemID, Data, Status, Anotacoes")] FollowUp followup)
        {
            if (ModelState.IsValid)
            {
                followup.Origem = _context.Prospeccao.FirstOrDefault(p => p.Id == followup.OrigemID);
                _context.Add(followup);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
        }

        // POST: FunilDeVendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoContratacao,LinhaPequisa, Status, Empresa, Contato, Casa")] Prospeccao prospeccao)
        {
            if (ModelState.IsValid)
            {
                if (_context.Empresa.Where(e => e.Id == prospeccao.Empresa.Id).Count() > 0)
                {
                    prospeccao.Empresa = _context.Empresa.First(e => e.Id == prospeccao.Empresa.Id);
                }
                else
                {
                    prospeccao.Empresa = new Empresa { Estado = prospeccao.Empresa.Estado, CNPJ = prospeccao.Empresa.CNPJ, Nome = prospeccao.Empresa.Nome, Segmento = prospeccao.Empresa.Segmento };
                }
                prospeccao.Contato.empresa = prospeccao.Empresa;
                var userId = HttpContext.User.Identity.Name;
                var user = await _context.Users.FirstAsync(u => u.UserName == userId);
                prospeccao.Usuario = user;

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
