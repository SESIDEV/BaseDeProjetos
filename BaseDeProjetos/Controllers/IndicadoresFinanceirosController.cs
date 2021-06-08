using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    public class IndicadoresFinanceirosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IndicadoresFinanceirosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IndicadoresFinanceiros
        public async Task<IActionResult> Index()
        {
            return View(await _context.IndicadoresFinanceiros.ToListAsync());
        }

        // GET: IndicadoresFinanceiros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: IndicadoresFinanceiros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IndicadoresFinanceiros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Receita,Despeita,Investimento")] IndicadoresFinanceiros indicadoresFinanceiros)
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

        // POST: IndicadoresFinanceiros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Receita,Despeita,Investimento")] IndicadoresFinanceiros indicadoresFinanceiros)
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
