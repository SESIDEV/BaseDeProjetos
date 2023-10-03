using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    public class StatusCurvasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusCurvasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StatusCurvas
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel; // Já vi não funcionar, porquê? :: hhenriques1999
                ViewData["NivelUsuario"] = usuario.Nivel;

                return View(await _context.StatusCurva.ToListAsync());
            }
            else
            {
                return View("Forbidden");
            }
        }

        // GET: StatusCurvas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusCurva = await _context.StatusCurva
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusCurva == null)
            {
                return NotFound();
            }

            return View(statusCurva);
        }

        // GET: StatusCurvas/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ListaDeProjetos = await _context.Projeto.ToListAsync();
            return View();
        }

        // POST: StatusCurvas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjetoId,Data,Fisico,Financeiro")] StatusCurva statusCurva)
        {
            if (ModelState.IsValid)
            {
                Projeto projetoExiste = await _context.Projeto.FindAsync(statusCurva.ProjetoId);

                if (projetoExiste != null)
                {
                    statusCurva.Projeto = projetoExiste;
                    _context.Add(statusCurva);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    throw new Exception("Não foi encontrado um projeto com este ID");
                }
            }
            return View(statusCurva);
        }

        // GET: StatusCurvas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusCurva = await _context.StatusCurva.FindAsync(id);
            if (statusCurva == null)
            {
                return NotFound();
            }
            return View(statusCurva);
        }

        // POST: StatusCurvas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjetoId,Data,Fisico,Financeiro")] StatusCurva statusCurva)
        {
            if (id != statusCurva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusCurva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusCurvaExists(statusCurva.Id))
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
            return View(statusCurva);
        }

        // GET: StatusCurvas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusCurva = await _context.StatusCurva
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusCurva == null)
            {
                return NotFound();
            }

            return View(statusCurva);
        }

        // POST: StatusCurvas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statusCurva = await _context.StatusCurva.FindAsync(id);
            _context.StatusCurva.Remove(statusCurva);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusCurvaExists(int id)
        {
            return _context.StatusCurva.Any(e => e.Id == id);
        }
    }
}