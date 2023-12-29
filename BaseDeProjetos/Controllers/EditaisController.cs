using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        private readonly ILogger<EditaisController> _logger;

        private readonly UserManager<Usuario> _userManager;

        [HttpGet]
        public async Task<IActionResult> CreateUsers()
        {
            ViewbagizarUsuario(_context);

            if (HttpContext.User.Identity.IsAuthenticated && UsuarioAtivo.Nivel == Nivel.Dev) // Apenas devs
            {
                await AddMultipleUsers(); // The function from the previous answer
                return View(); // Redirect to a confirmation page or wherever you want after creating users.
            }
            else
            {
                return View("Forbidden");
            }
        }

        public async Task AddMultipleUsers()
        {
            var users = new List<Usuario>
            {
                // Cadastro bolsistas
                new Usuario { UserName = "Beatriz Guimarães", Email = "bsguimaraes@pesquisador.firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Débora Oliveira", Email = "drdoliveira@pesquisador.firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Bruna Borges", Email = "bbasilva@pesquisador.firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Andreza Mendonça", Email = "admendonca@pesquisador.firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Thuanny Moraes", Email = "tmoraes@pesquisador.firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Geovanna Batista", Email = "gbflorentino@pesquisador.firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Carlos Magno", Email = "carlosms@pesquisador.firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},

                // Cadastro estagiários
                new Usuario { UserName = "Gabriel Mattos", Email = "gserra@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0, EmailConfirmed = false },
                new Usuario { UserName = "Caio Vitor", Email = "ccadete@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0, EmailConfirmed = false },
                new Usuario { UserName = "Caio de Souza", Email = "cdmoreira@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Mariana de Paula", Email = "mdbatista@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Lucas Rocha", Email = "lucrocha@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Maria Carolina", Email = "mcmarques@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Juliana Peres", Email = "jpborges@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Pedro Tagliabui", Email = "ppduarte@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},

                // Cadastro de usuários "dummy" (idealmente deveremos encontrar uma forma mais dinâmica de fazer isso no futuro, mas pela "correria", vai desse jeito.
                new Usuario { UserName = "Estagiário 1", Email = "estag1@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Estagiário 2", Email = "estag2@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Estagiário 3", Email = "estag3@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Estagiário 4", Email = "estag4@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Estagiário 5", Email = "estag5@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 3, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Bolsista 1", Email = "bols1@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Bolsista 2", Email = "bols2@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Bolsista 3", Email = "bols3@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Bolsista 4", Email = "bols4@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
                new Usuario { UserName = "Bolsista 5", Email = "bols5@firjan.com.br", Casa = Instituto.ISIQV, CargoId = 2, Matricula = 0, Nivel = 0, Vinculo = 0 , EmailConfirmed = false},
            };

            foreach (var user in users)
            {
                var result = await _userManager.CreateAsync(user, "@ISIqv1234"); // Replace 'DefaultPassword' with your desired password.
                if (!result.Succeeded)
                {
                    // Handle error - for instance, the user might already exist, or there might be password validation errors.
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Um erro ocorreu ao criar o usuário: {error.Code} : {error.Description}");
                    }
                }
            }
        }

        public EditaisController(ApplicationDbContext context, UserManager<Usuario> userManager, ILogger<EditaisController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
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
                        Usuario = UsuarioAtivo,
                        Status = new List<FollowUp> {
                        new FollowUp
                        {
                            OrigemID = prospeccaoId,
                            Data = DateTime.Today,
                            Anotacoes = $"Prospecção iniciada a partir do edital {Edital.Name}",
                            Status = StatusProspeccao.ContatoInicial
                        }
                    },
                        Casa = UsuarioAtivo.Casa
                    },
                    ProspeccaoId = prospeccaoId,
                    Proponente = "",
                    ProjetoProposto = "",
                    ResponsavelSubmissao = UsuarioAtivo.UserName,
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