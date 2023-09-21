using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using System.Net.Http;

namespace BaseDeProjetos.Controllers
{
    public class PesquisaProjetosController : SGIController
    {
        private readonly ApplicationDbContext _context;

        public PesquisaProjetosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PesquisaProjetos
        public async Task<IActionResult> Index()
        {
            ViewbagizarUsuario(_context);
            return View(await _context.PesquisaProjeto.ToListAsync());
        }

        // GET: PesquisaProjetos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pesquisaProjeto = await _context.PesquisaProjeto
                .FirstOrDefaultAsync(m => m.IdPesquisa == id);
            if (pesquisaProjeto == null)
            {
                return NotFound();
            }

            return View(pesquisaProjeto);
        }

        // GET: PesquisaProjetos/Create
        public IActionResult Create()
        {
            List<Projeto> projetos = _context.Projeto.ToList();
            ViewData["Projetos"] = new SelectList(projetos, "Id", "NomeProjeto");
            return View();
        }

        // POST: PesquisaProjetos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPesquisa,ProjetoId")] PesquisaProjeto pesquisaProjeto)
        {
            if (ModelState.IsValid)
            {
                PesquisaProjeto pesquisaNova = new PesquisaProjeto(InicioFrio:true)
                {
                    IdPesquisa = pesquisaProjeto.IdPesquisa,
                    ProjetoId = pesquisaProjeto.ProjetoId
                };

                _context.Add(pesquisaNova);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: PesquisaProjetos/Responder/5
        public IActionResult Responder(int? id)
        {
            PesquisaProjeto pesquisa = _context.PesquisaProjeto.FirstOrDefault(p => p.IdPesquisa == id);
            Projeto projetoPesquisa = _context.Projeto.FirstOrDefault(p => p.Id == pesquisa.ProjetoId);

            ViewBag.Projeto = projetoPesquisa;
            ViewBag.Questionario = pesquisa;

            return View();
                
        }

        [HttpPost]
        public async Task<IActionResult> Responder()
        {
            List<int> respostas = new List<int>();
            int pesquisaId = Int32.Parse(HttpContext.Request.Form["PesquisaId"]);
            PesquisaProjeto pesquisa = _context.PesquisaProjeto.First(p => p.IdPesquisa == pesquisaId);

            foreach(int code in pesquisa.ObterHashCodesQuestoes())
            {
                int resposta = -1;
                Int32.TryParse(HttpContext.Request.Form[$"likert-{code}"], out resposta);
                respostas.Add(resposta); 
                
            }
            int retornaria = -1;
            Int32.TryParse(HttpContext.Request.Form["option"],out retornaria);
            respostas.Add(retornaria);

            var texto = HttpContext.Request.Form["commentsTextArea"];

            pesquisa.RespostasIndividuais = respostas;
            pesquisa.Comentarios = texto;

            _context.PesquisaProjeto.Update(pesquisa);
            await _context.SaveChangesAsync();

            return View("Kokoro");
        }

        // GET: PesquisaProjetos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pesquisaProjeto = await _context.PesquisaProjeto.FindAsync(id);
            if (pesquisaProjeto == null)
            {
                return NotFound();
            }
            return View(pesquisaProjeto);
        }

        // POST: PesquisaProjetos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPesquisa,ProjetoId,ResultadoFinal,RepresentacaoTextualQuestionario")] PesquisaProjeto pesquisaProjeto)
        {
            if (id != pesquisaProjeto.IdPesquisa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pesquisaProjeto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PesquisaProjetoExists(pesquisaProjeto.IdPesquisa))
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
            return View(pesquisaProjeto);
        }

        // GET: PesquisaProjetos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pesquisaProjeto = await _context.PesquisaProjeto
                .FirstOrDefaultAsync(m => m.IdPesquisa == id);
            if (pesquisaProjeto == null)
            {
                return NotFound();
            }

            return View(pesquisaProjeto);
        }

        // POST: PesquisaProjetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pesquisaProjeto = await _context.PesquisaProjeto.FindAsync(id);
            _context.PesquisaProjeto.Remove(pesquisaProjeto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PesquisaProjetoExists(int id)
        {
            return _context.PesquisaProjeto.Any(e => e.IdPesquisa == id);
        }
    }
}
