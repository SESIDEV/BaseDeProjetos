using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class IndicadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IndicadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IndicadoresFinanceiros
        public async Task<IActionResult> Index(string casa)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
                if (usuario.Nivel == Nivel.PMO || usuario.Nivel == Nivel.Dev)
                {
                    ViewBag.usuarioCasa = usuario.Casa;
                    ViewBag.usuarioNivel = usuario.Nivel;

                    List<IndicadoresFinanceiros> listaIndicadoresFinanceiros = await _context.IndicadoresFinanceiros.ToListAsync();
                    if (string.IsNullOrEmpty(casa))
                    {
                        casa = usuario.Casa.ToString();

                    }
                    List<IndicadoresFinanceiros> lista = DefinirCasaParaVisualizar(casa);
                    lista = VincularCasaAosIndicadoresFinanceiros(usuario, listaIndicadoresFinanceiros);
                    return View(lista.ToList());
                }
                else
                {
                    return View("Forbidden");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        public static List<IndicadoresFinanceiros> VincularCasaAosIndicadoresFinanceiros(Usuario usuario, List<IndicadoresFinanceiros> listaIndicadoresFinanceiros)
        {

            if (usuario.Casa == Instituto.Super || usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO)
            {
                return listaIndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList();

            }
            else
            {
                return listaIndicadoresFinanceiros.Where(lista => lista.Casa == usuario.Casa).ToList();
            }

        }

        private List<IndicadoresFinanceiros> DefinirCasaParaVisualizar(string? casa)
        {
            Instituto enum_casa;

            if (Enum.IsDefined(typeof(Instituto), casa))
            {
                HttpContext.Session.SetString("_Casa", casa);
                enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
            }
            else
            {
                enum_casa = Instituto.Super;
            }


            List<IndicadoresFinanceiros> listaIndicadores = new List<IndicadoresFinanceiros>();

            List<IndicadoresFinanceiros> lista = enum_casa == Instituto.Super ?
            _context.IndicadoresFinanceiros.ToList() :
            _context.IndicadoresFinanceiros.Where(p => p.Casa.Equals(enum_casa)).ToList();

            listaIndicadores.AddRange(lista);

            return listaIndicadores.ToList();
        }
        // GET: IndicadoresFinanceiros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
                if (usuario.Nivel == Nivel.PMO || usuario.Nivel == Nivel.Dev)
                {
                    ViewBag.usuarioCasa = usuario.Casa;
                    ViewBag.usuarioNivel = usuario.Nivel;

                    if (id == null)
                    {
                        return NotFound();
                    }

                    IndicadoresFinanceiros indicadoresFinanceiros = await _context.IndicadoresFinanceiros
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (indicadoresFinanceiros == null)
                    {
                        return NotFound();
                    }

                    return View(indicadoresFinanceiros);
                }
                else
                {
                    return View("Forbidden");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        // GET: IndicadoresFinanceiros/Create
        public IActionResult Create()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
                if (usuario.Nivel == Nivel.PMO || usuario.Nivel == Nivel.Dev)
                {
                    ViewBag.usuarioCasa = usuario.Casa;
                    ViewBag.usuarioNivel = usuario.Nivel;

                    return View();
                }
                else
                {
                    return View("Forbidden");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: IndicadoresFinanceiros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Receita,Despesa,Investimento,QualiSeguranca, Casa")] IndicadoresFinanceiros indicadoresFinanceiros)
        {
            if (ModelState.IsValid)
            {
                _context.Add(indicadoresFinanceiros);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(indicadoresFinanceiros);
        }

        // GET: IndicadoresFinanceiros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
                if (usuario.Nivel == Nivel.PMO || usuario.Nivel == Nivel.Dev)
                {
                    ViewBag.usuarioCasa = usuario.Casa;
                    ViewBag.usuarioNivel = usuario.Nivel;

                    if (id == null)
                    {
                        return NotFound();
                    }

                    IndicadoresFinanceiros indicadoresFinanceiros = await _context.IndicadoresFinanceiros.FindAsync(id);
                    if (indicadoresFinanceiros == null)
                    {
                        return NotFound();
                    }
                    return View(indicadoresFinanceiros);
                }
                else
                {
                    return View("Forbidden");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: IndicadoresFinanceiros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Receita,Despesa,Investimento,QualiSeguranca")] IndicadoresFinanceiros indicadoresFinanceiros)
        {
            if (id != indicadoresFinanceiros.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(indicadoresFinanceiros);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndicadoresFinanceirosExists(indicadoresFinanceiros.Id))
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
            return View(indicadoresFinanceiros);
        }

        // GET: IndicadoresFinanceiros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
                if (usuario.Nivel == Nivel.PMO || usuario.Nivel == Nivel.Dev)
                {
                    ViewBag.usuarioCasa = usuario.Casa;
                    ViewBag.usuarioNivel = usuario.Nivel;

                    if (id == null)
                    {
                        return NotFound();
                    }

                    IndicadoresFinanceiros indicadoresFinanceiros = await _context.IndicadoresFinanceiros
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (indicadoresFinanceiros == null)
                    {
                        return NotFound();
                    }

                    return View(indicadoresFinanceiros);
                }
                else
                {
                    return View("Forbidden");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: IndicadoresFinanceiros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            IndicadoresFinanceiros indicadoresFinanceiros = await _context.IndicadoresFinanceiros.FindAsync(id);
            _context.IndicadoresFinanceiros.Remove(indicadoresFinanceiros);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IndicadoresFinanceirosExists(int id)
        {
            return _context.IndicadoresFinanceiros.Any(e => e.Id == id);
        }
    }
}