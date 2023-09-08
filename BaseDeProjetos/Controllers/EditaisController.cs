using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class EditaisController : SGIController
    {
        private readonly ApplicationDbContext _context;
        public EditaisController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Editais
        public async Task<IActionResult> Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);
                List<Empresa> empresas = _context.Empresa.ToList();
                ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
                return View(await _context.Editais.ToListAsync());
            }
            else
            {
                return View("Forbidden");
            }
        }
        // GET: Editais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                if (id == null)
                {
                    return NotFound();
                }

                var editais = await _context.Editais
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (editais == null)
                {
                    return NotFound();
                }

                return View(editais);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // GET: Editais/Create        
        public IActionResult Create()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                return View();
            }
            else
            {
                return View("Forbidden");
            }
        }
        // POST: Editais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Descricao,Local,StatusEdital,AgenciaFomento,PrazoSubmissao,ValorEdital,Proponente,LinkEdital,ProjetoProposto,DataResultado,StatusSubmissao")] Editais editais)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editais);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editais);
        }

        // GET: Editais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                if (id == null)
                {
                    return NotFound();
                }

                var editais = await _context.Editais.FindAsync(id);
                if (editais == null)
                {
                    return NotFound();
                }
                return View(editais);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Editais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Descricao,Local,StatusEdital,AgenciaFomento,PrazoSubmissao,ValorEdital,Proponente,LinkEdital,ProjetoProposto,DataResultado,StatusSubmissao")] Editais editais)
        {
            if (id != editais.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editais);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditaisExists(editais.Id))
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
            return View(editais);
        }

        // GET: Editais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                if (id == null)
                {
                    return NotFound();
                }

                var editais = await _context.Editais
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (editais == null)
                {
                    return NotFound();
                }

                return View(editais);
            }
            else
            {
                return View("Forbidden");
            }
        }

        /// <summary>
        /// Retorna um modal de acordo com os parâmetros passados
        /// </summary>
        /// <param name="idEdital">ID do Edital</param>
        /// <param name="tipo">Tipo de Modal a ser retornado (Edit, Create, etc...)</param>
        /// <returns>ViewComponent com os conteúdos do Modal</returns>
        public IActionResult RetornarModal(string idEdital, string tipo)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (tipo != null)
                {
                    return ViewComponent($"Modal{tipo}Edital", new { id = idEdital });
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

        // POST: Editais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var editais = await _context.Editais.FindAsync(id);
            _context.Editais.Remove(editais);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica se um edital existe no Banco de Dados
        /// </summary>
        /// <param name="id">ID do Edital</param>
        /// <returns></returns>
        private bool EditaisExists(int id)
        {
            return _context.Editais.Any(e => e.Id == id);
        }

        /// <summary>
        /// Cria uma prospeccção à partir de um edital
        /// </summary>
        /// <param name="id">ID do Edital</param>
        /// <returns></returns>
        public IActionResult ProspectarEdital(int id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);
                Editais Edital = _context.Editais.FirstOrDefault(e => e.Id == id);
                string prospeccaoId = $"prosp_{DateTime.Now.Ticks}";

                Submissao submissao = new Submissao
                {
                    ComEmpresa = false,
                    Edital = Edital,
                    EditalId = Edital.Id.ToString(),
                    Prospeccao = new Prospeccao
                    {
                        Id = prospeccaoId,
                        Usuario = usuarioAtivo,
                        Status = new List<FollowUp> {
                        new FollowUp
                        {
                            OrigemID = prospeccaoId,
                            Data = DateTime.Today,
                            Anotacoes = $"Prospecção iniciada a partir do edital {Edital.Name}",
                            Status = StatusProspeccao.ContatoInicial
                        }
                    },
                        Casa = usuarioAtivo.Casa
                    },
                    ProspeccaoId = prospeccaoId,
                    Proponente = "",
                    ProjetoProposto = "",
                    ResponsavelSubmissao = usuarioAtivo.UserName,
                    StatusSubmissao = StatusSubmissaoEdital.emAnalise
                };
                _context.Add(submissao);
                _context.SaveChanges();
                return RedirectToAction("Index", "FunilDeVendas");
            }
            else
            {
                return View("Forbidden");
            }
        }
    }
}
