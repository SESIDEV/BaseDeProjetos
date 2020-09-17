using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    public class ProjetosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjetosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult PopularBase()
        {
            string[] _base;
            using (StreamReader file = new StreamReader("~/Base.csv"))
            {
                _base = file.ReadToEnd().Split("\n");
            }


            return View(nameof(Index));
        }

        // GET: Projetos
        public async Task<IActionResult> Index(string casa)
        {
            if (casa is null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Casa")))
                {
                    casa = HttpContext.Session.GetString("_Casa");
                }
                else { return View(await _context.Projeto.ToListAsync()); }
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
            Casa enum_casa = (Casa)Enum.Parse(typeof(Casa), casa);
            List<Projeto> lista = await _context.Projeto.Where(p => p.Casa.Equals(enum_casa)).ToListAsync();
            return View(lista);
        }

        // GET: Projetos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Projeto projeto = await _context.Projeto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projeto == null)
            {
                return NotFound();
            }

            return View(projeto);
        }

        // GET: Projetos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projetos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeProjeto,Casa, AreaPesquisa,DataInicio,DataEncerramento,Estado,FonteFomento,Inovacao,status,DuracaoProjetoEmMeses,ValorTotalProjeto,ValorAporteRecursos")] Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projeto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            return View(projeto);
        }

        // GET: Projetos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Projeto projeto = await _context.Projeto.FindAsync(id);
            if (projeto == null)
            {
                return NotFound();
            }
            return View(projeto);
        }

        // POST: Projetos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Casa,NomeProjeto,AreaPesquisa,DataInicio,DataEncerramento,Estado,FonteFomento,Inovacao,status,DuracaoProjetoEmMeses,ValorTotalProjeto,ValorAporteRecursos")] Projeto projeto)
        {
            if (id != projeto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projeto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjetoExists(projeto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            return View(projeto);
        }

        // GET: Projetos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Projeto projeto = await _context.Projeto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projeto == null)
            {
                return NotFound();
            }

            return View(projeto);
        }

        // POST: Projetos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Projeto projeto = await _context.Projeto.FindAsync(id);
            _context.Projeto.Remove(projeto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjetoExists(string id)
        {
            return _context.Projeto.Any(e => e.Id == id);
        }

        private void DetachLocal(Func<Usuario, bool> predicate)
        {
            Usuario local = _context.Set<Usuario>().Local.Where(predicate).FirstOrDefault();
            if (!(local is null))
            {
                _context.Entry(local).State = EntityState.Detached;
            }
        }

        [HttpPost]
        public IActionResult CarregarProjetos(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            if (files.Count < 1)
            {
                return Content("Não foram submetidos dados");
            }

            foreach (IFormFile formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (StreamReader file = new StreamReader(formFile.OpenReadStream())){
                        CriarProjetos(file);
                    }
                }
            }

            return View("Index","Home");
        }

        private void CriarProjetos(StreamReader file)
        {
            List<string> lines = file.ReadToEnd().Split(Environment.NewLine).Skip(1).ToList();

            foreach (string line in lines)
            {
                string[] dados = line.Split("|");
                double valorTotal = 0;
                double valorAporte = 0;
                _ = Double.TryParse(Regex.Replace(dados[10], @"[^\d]", ""), out valorTotal);
                _ = double.TryParse(Regex.Replace(dados[11], @"[^\d]", ""), out valorAporte);

                Projeto projeto = new Projeto
                {
                    Id = $"Proj_{DateTime.Now.Ticks}",
                    Casa = Enum.Parse<Casa>(dados[0], true),
                    NomeProjeto = dados[3],
                    Empresa = new Empresa { Nome = dados[2] },
                    AreaPesquisa = Enum.IsDefined(typeof(LinhaPesquisa), dados[1]) ? Enum.Parse<LinhaPesquisa>(dados[1]) : LinhaPesquisa.Indefinida,
                    Estado = Enum.IsDefined(typeof(Estado), dados[1].Replace(" ", "")) ? Enum.Parse<Estado>(dados[1]) : Estado.Estrangeiro,
                    ValorAporteRecursos = valorAporte > 0 ? valorAporte : valorTotal,
                    ValorTotalProjeto = valorTotal,
                    Inovacao = Enum.IsDefined(typeof(TipoInovacao), dados[8]) ? Enum.Parse<TipoInovacao>(dados[8]) : TipoInovacao.Processo,
                    status = Enum.IsDefined(typeof(StatusProjeto), dados[15]) ? Enum.Parse<StatusProjeto>(dados[15]) : StatusProjeto.EmExecucao,
                    FonteFomento = Enum.IsDefined(typeof(TipoContratacao), dados[7]) ? Enum.Parse<TipoContratacao>(dados[7]) : TipoContratacao.OutrosEditais,
                    DuracaoProjetoEmMeses = Convert.ToInt32(dados[17])
                };

                _context.Add(projeto);
                _context.SaveChanges();
            }
        }
    }
}
