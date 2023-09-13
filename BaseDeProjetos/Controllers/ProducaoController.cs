using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class ProducaoController : SGIController
    {
        private readonly ApplicationDbContext _context;

        public ProducaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Producao
        public async Task<IActionResult> Index(string casa, string searchString = "", string ano = "")
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                List<Producao> producoes;

                this.ViewbagizarUsuario(_context);

                if (string.IsNullOrEmpty(casa))
                {
                    casa = UsuarioAtivo.Casa.ToString();
                }

                producoes = await FunilHelpers.DefinirCasaParaVisualizarEmProducao(casa, UsuarioAtivo, _context, HttpContext, ViewData);
                producoes = FunilHelpers.VincularCasaProducao(UsuarioAtivo, producoes);
                producoes = FunilHelpers.PeriodizarProduções(ano, producoes);
                producoes = FunilHelpers.FiltrarProduções(searchString, producoes);
                producoes = producoes.OrderBy(p => p.Data).ToList();

                List<Empresa> empresas = await _context.Empresa.ToListAsync();
                List<Projeto> projetos = await _context.Projeto.ToListAsync();

                ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
                ViewData["Projetos"] = new SelectList(projetos, "Id", "NomeProjeto");

                return View(producoes);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // GET: Producao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                if (id == null)
                {
                    return NotFound();
                }

                var producao = await _context.Producao
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (producao == null)
                {
                    return NotFound();
                }

                return View(producao);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // GET: Producao/Create
        public IActionResult Create()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                List<Empresa> empresas = _context.Empresa.ToList();
                List<Projeto> projetos = _context.Projeto.ToList();
                ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
                ViewData["Projetos"] = new SelectList(projetos, "Id", "NomeProjeto");
                return View();
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Producao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Grupo,Casa,Titulo,Descricao,Autores,StatusPub,Data,Local,DOI,Imagem,Projeto,Empresa,Responsavel,NumPatente")] Producao producao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producao);
        }

        // GET: Producao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                if (id == null)
                {
                    return NotFound();
                }

                var producao = await _context.Producao.FindAsync(id);
                if (producao == null)
                {
                    return NotFound();
                }
                List<Empresa> empresas = _context.Empresa.ToList();
                List<Projeto> projetos = _context.Projeto.ToList();
                ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
                ViewData["Projetos"] = new SelectList(projetos, "Id", "NomeProjeto");
                return View(producao);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Producao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Grupo,Casa,Titulo,Descricao,Autores,StatusPub,Data,Local,DOI,Imagem,Projeto,Empresa,Responsavel,NumPatente")] Producao producao)
        {
            if (id != producao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducaoExists(producao.Id))
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
            return View(producao);
        }

        // GET: Producao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;

                if (id == null)
                {
                    return NotFound();
                }

                var producao = await _context.Producao
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (producao == null)
                {
                    return NotFound();
                }

                return View(producao);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Producao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producao = await _context.Producao.FindAsync(id);
            _context.Producao.Remove(producao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProducaoExists(int id)
        {
            return _context.Producao.Any(e => e.Id == id);
        }
    }
}
