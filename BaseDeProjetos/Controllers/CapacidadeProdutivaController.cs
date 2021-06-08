using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    public class CapacidadeProdutivaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CapacidadeProdutivaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CapacidadeProdutiva
        public async Task<IActionResult> Index()
        {
            return View(await _context.AtividadesProdutivas.ToListAsync());
        }

        // GET: CapacidadeProdutiva/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AtividadesProdutivas atividadesProdutivas = await _context.AtividadesProdutivas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (atividadesProdutivas == null)
            {
                return NotFound();
            }

            return View(atividadesProdutivas);
        }

        // GET: CapacidadeProdutiva/Create
        public IActionResult Create()
        {

            ViewData["Projetos"] = _context.Projeto.Where(p=>!string.IsNullOrEmpty(p.NomeProjeto)).ToList();
            return View();
        }

        // POST: CapacidadeProdutiva/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Atividade,AreaAtividade,FonteFomento,ProjetoId,DescricaoAtividade,CargaHoraria,Data")] AtividadesProdutivas atividadesProdutivas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(atividadesProdutivas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(atividadesProdutivas);
        }

        // GET: CapacidadeProdutiva/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AtividadesProdutivas atividadesProdutivas = await _context.AtividadesProdutivas.FindAsync(id);
            if (atividadesProdutivas == null)
            {
                return NotFound();
            }
            return View(atividadesProdutivas);
        }

        // POST: CapacidadeProdutiva/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Atividade,AreaAtividade,FonteFomento,ProjetoId,DescricaoAtividade,CargaHoraria,Data")] AtividadesProdutivas atividadesProdutivas)
        {
            if (id != atividadesProdutivas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(atividadesProdutivas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AtividadesProdutivasExists(atividadesProdutivas.Id))
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
            return View(atividadesProdutivas);
        }

        // GET: CapacidadeProdutiva/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AtividadesProdutivas atividadesProdutivas = await _context.AtividadesProdutivas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (atividadesProdutivas == null)
            {
                return NotFound();
            }

            return View(atividadesProdutivas);
        }

        // POST: CapacidadeProdutiva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            AtividadesProdutivas atividadesProdutivas = await _context.AtividadesProdutivas.FindAsync(id);
            _context.AtividadesProdutivas.Remove(atividadesProdutivas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AtividadesProdutivasExists(int id)
        {
            return _context.AtividadesProdutivas.Any(e => e.Id == id);
        }
    }
}
