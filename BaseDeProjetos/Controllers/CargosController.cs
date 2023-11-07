using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
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
        private readonly DbCache _dbCache;

        public CargosController(ApplicationDbContext context, DbCache dbCache)
        {
            _context = context;
            _dbCache = dbCache;
        }

        // GET: Cargos
        public async Task<IActionResult> Index()
        {
            string cacheKey = "AllCargos";

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                if (UsuarioAtivo.Nivel != Nivel.Usuario && UsuarioAtivo.Nivel != Nivel.Externos)
                {
                    var cargos = await _dbCache.GetCachedAsync(cacheKey, () => _context.Cargo.ToListAsync());
                    return View(cargos);
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
                await CacheHelper.CleanupCargosCache(_dbCache);
                await _dbCache.SetCachedAsync($"Cargos:{cargo.Id}", cargo);
                return RedirectToAction(nameof(Index));
            }
            return View(cargo);
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
                    await CacheHelper.CleanupCargosCache(_dbCache);
                    await _dbCache.SetCachedAsync($"Cargos:{id}", cargo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CargoExists(cargo.Id))
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

        // POST: Cargos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargo = await _dbCache.GetCachedAsync("Cargos:{id}", () => _context.Cargo.FindAsync(id).AsTask());
            _context.Cargo.Remove(cargo);
            await _context.SaveChangesAsync();
            await CacheHelper.CleanupCargosCache(_dbCache);
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

            private async Task<bool> CargoExists(int id)
            {
                return await _dbCache.GetCachedAsync($"Cargos:{id}:exists", async () => await Task.FromResult(_context.Cargo.Any(e => e.Id == id)));
            }
    }
}