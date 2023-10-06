using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    public class CargosController : SGIController
    {
        private readonly ApplicationDbContext _context;

        public CargosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cargos
        public async Task<IActionResult> Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                if (UsuarioAtivo.Nivel != Nivel.Usuario && UsuarioAtivo.Nivel != Nivel.Externos)
                {
                    return View(await _context.Cargo.ToListAsync());
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

        // GET: Cargos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated && UsuarioAtivo.Nivel != Nivel.Usuario && UsuarioAtivo.Nivel != Nivel.Externos)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cargo = await _context.Cargo
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (cargo == null)
                {
                    return NotFound();
                }

                return View(cargo);
            }
            else
            {
                return View("Forbiden");
            }
        }

        // GET: Cargos/Create
        public IActionResult Create()
        {
            if (HttpContext.User.Identity.IsAuthenticated && UsuarioAtivo.Nivel != Nivel.Usuario && UsuarioAtivo.Nivel != Nivel.Externos)
            {
                return View();
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Cargos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Salario,HorasSemanais,Tributos")] Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cargo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cargo);
        }

        // GET: Cargos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated && UsuarioAtivo.Nivel != Nivel.Usuario && UsuarioAtivo.Nivel != Nivel.Externos)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cargo = await _context.Cargo.FindAsync(id);
                if (cargo == null)
                {
                    return NotFound();
                }
                return View(cargo);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Cargos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Salario,HorasSemanais,Tributos")] Cargo cargo)
        {
            if (id != cargo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cargo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoExists(cargo.Id))
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
            return View(cargo);
        }

        // GET: Cargos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated && UsuarioAtivo.Nivel != Nivel.Usuario && UsuarioAtivo.Nivel != Nivel.Externos)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var cargo = await _context.Cargo
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (cargo == null)
                {
                    return NotFound();
                }

                return View(cargo);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Cargos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargo = await _context.Cargo.FindAsync(id);
            _context.Cargo.Remove(cargo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult RetornarModal(string idCargo, string tipo)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (tipo != null)
                {
                    return ViewComponent($"Modal{tipo}Cargo", new { id = idCargo });
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        private bool CargoExists(int id)
        {
            return _context.Cargo.Any(e => e.Id == id);
        }
    }
}