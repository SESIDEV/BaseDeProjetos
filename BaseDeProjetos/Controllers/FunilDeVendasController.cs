using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using MailSenderApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using SmartTesting.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    public class FunilDeVendasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _mailer;

        public FunilDeVendasController(ApplicationDbContext context, IEmailSender mailer)
        {
            _context = context;
            _mailer = mailer;
        }

        // GET: FunilDeVendas
        public async Task<IActionResult> Index(string casa, string sortOrder, string searchString)
        {

            //Filtros e ordenadores
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "TipoContratacao" ? "tipo_desc" : "TipoContratacao";
            ViewData["CurrentFilter"] = searchString;

            Casa enum_casa;
            enum_casa = await DefinirCasa(casa);

            var lista = enum_casa == Casa.Super ?
                _context.Prospeccao:
                _context.Prospeccao.Where(p => p.Casa.Equals(enum_casa));

            lista = OrdenarProspecções(sortOrder, lista);

            if (!String.IsNullOrEmpty(searchString))
            {
                lista = lista.Where(s => s.Empresa.Nome.Contains(searchString)
                                       || s.Usuario.UserName.Contains(searchString));
            }

            return View(lista.ToList<Prospeccao>());
        }

        private async Task<Casa> DefinirCasa(string casa)
        {
            switch (casa)
            {
                case null:
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Casa")))
                    {
                        casa = HttpContext.Session.GetString("_Casa");
                    }
                    else { return Casa.Super; }

                    break;
                default:
                    if (Enum.IsDefined(typeof(Casa), casa))
                    {
                        HttpContext.Session.SetString("_Casa", casa);
                    }
                    else
                    {
                        return Casa.Super;
                    }
                    break;
            }

            ViewData["Area"] = casa;
            return (Casa)Enum.Parse(typeof(Casa), casa);
        }

        private static IQueryable<Prospeccao> OrdenarProspecções(string sortOrder, IQueryable<Prospeccao> lista)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    lista = lista.OrderByDescending(s => s.Empresa.Nome);
                    break;
                case "TipoContratacao":
                    lista = lista.OrderBy(s => s.TipoContratacao);
                    break;
                case "tipo_desc":
                    lista = lista.OrderByDescending(s => s.TipoContratacao);
                    break;
                default:
                    lista = lista.OrderBy(s => s.Empresa.Nome);
                    break;
            }

            return lista;
        }


        // GET: FunilDeVendas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Prospeccao prospeccao = await _context.Prospeccao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prospeccao == null)
            {
                return NotFound();
            }

            return View(prospeccao);
        }

        // GET: FunilDeVendas/Create
        public IActionResult Create()
        {
            ViewData["Empresas"] = new SelectList(_context.Empresa.ToList(), "Id", "Nome");
            return View();
        }

        [HttpGet]
        public IActionResult Atualizar(string id)
        {
            ViewData["origem"] = id;
            ViewData["prosp"] = _context.Prospeccao.FirstOrDefault(p => p.Id == id);
            return View("CriarFollowUp");
        }

        [HttpPost]
        public async Task<IActionResult> Atualizar([Bind("OrigemID, Data, Status, Anotacoes")] FollowUp followup)
        {
            if (ModelState.IsValid)
            {
                followup.Origem = _context.Prospeccao.FirstOrDefault(p => p.Id == followup.OrigemID);
                _context.Add(followup);

                NotificarProspecção(followup);

                AtualizarStatus(followup);

                if (followup.Status == StatusProspeccao.Convertida)
                {
                    CriarProjetoConvertido(followup);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
        }

        private static void AtualizarStatus(FollowUp followup)
        {
            foreach (FollowUp status in followup.Origem.Status)
            {
                status.isTratado = true;
            }
        }

        private void CriarProjetoConvertido(FollowUp followup)
        {

            Usuario user = _context.Prospeccao.Find(followup.OrigemID).Usuario;
            Projeto novo_projeto = new Projeto
            {
                Casa = followup.Origem.Casa,
                AreaPesquisa = followup.Origem.LinhaPequisa,
                Empresa = followup.Origem.Empresa,
                status = StatusProjeto.EmExecucao,
                Id = $"proj_{DateTime.Now.Ticks}",
                Equipe = {user}
            };

            _context.Add(novo_projeto);
        }

        private void NotificarProspecção(FollowUp followup)
        {
            Notificacao notificacao;

            switch (followup.Status)
            {
                case StatusProspeccao.ComProposta:

                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma proposta comercial foi enviada!",
                        TextoBase = $"Olá, A prospecção com {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo {followup.Origem.Usuario} teve uma proposta enviada. " +
                         $"<hr><br><br><hr>" +
                         $"Mais detalhes: {followup.Anotacoes}"
                    };
                    break;

                case StatusProspeccao.Convertida:
                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma proposta comercial foi convertida!",
                        TextoBase = $"Olá, A proposta com {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo {followup.Origem.Usuario} foi convertida." +
                         $"<hr><br><br><hr>" +
                         $"Mais detalhes: {followup.Anotacoes}"
                    };
                    break;

                case StatusProspeccao.NaoConvertida:
                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma proposta comercial não foi convertida",
                        TextoBase = $"Olá, A proposta com {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo {followup.Origem.Usuario} não foi convertida." +
                         $"<hr><br><br><hr>" +
                         $"Mais detalhes: {followup.Anotacoes}"
                    };
                    break;

                case StatusProspeccao.ContatoInicial:
                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma nova prospecção foi inicializada!",
                        TextoBase = $"Olá, A prospecção com {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo {followup.Origem.Usuario} foi iniciada." +
                         $"<hr><br><br><hr>" +
                         $"Mais detalhes: {followup.Anotacoes}"
                    };
                    break;

                default:
                    return;
            }
                    ((EmailSender)_mailer).SendEmailAsync(notificacao);
        }

        // POST: FunilDeVendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoContratacao,LinhaPequisa, Status, Empresa, Contato, Casa")] Prospeccao prospeccao)
        {
            if (ModelState.IsValid)
            {
                if (_context.Empresa.Where(e => e.Id == prospeccao.Empresa.Id).Count() > 0)
                {
                    prospeccao.Empresa = _context.Empresa.First(e => e.Id == prospeccao.Empresa.Id);
                }
                else
                {
                    prospeccao.Empresa = new Empresa { Estado = prospeccao.Empresa.Estado, CNPJ = prospeccao.Empresa.CNPJ, Nome = prospeccao.Empresa.Nome, Segmento = prospeccao.Empresa.Segmento };
                }
                prospeccao.Contato.empresa = prospeccao.Empresa;
                string userId = HttpContext.User.Identity.Name;
                Usuario user = await _context.Users.FirstAsync(u => u.UserName == userId);
                prospeccao.Usuario = user;
                prospeccao.Status[0].Origem = prospeccao;
                NotificarProspecção(prospeccao.Status[0]);

                _context.Add(prospeccao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
        }

        // GET: FunilDeVendas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Empresas"] = new SelectList(_context.Empresa.ToList(), "Id", "Nome");
            if (id == null)
            {
                return NotFound();
            }

            Prospeccao prospeccao = await _context.Prospeccao.FindAsync(id);
            if (prospeccao == null)
            {
                return NotFound();
            }
            return View(prospeccao);
        }

        // POST: FunilDeVendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,TipoContratacao,LinhaPequisa, Empresa, Contato, Casa")] Prospeccao prospeccao)
        {
            if (id != prospeccao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.Empresa.Where(e => e.Id == prospeccao.Empresa.Id).Count() > 0)
                    {
                        prospeccao.Empresa = _context.Empresa.First(e => e.Id == prospeccao.Empresa.Id);
                    }
                    else
                    {
                        prospeccao.Empresa = new Empresa { Estado = prospeccao.Empresa.Estado, CNPJ = prospeccao.Empresa.CNPJ, Nome = prospeccao.Empresa.Nome, Segmento = prospeccao.Empresa.Segmento };
                    }

                    _context.Update(prospeccao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProspeccaoExists(prospeccao.Id))
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
            return View(prospeccao);
        }

        public async Task<IActionResult> EditarFollowUp(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FollowUp followup = await _context.FollowUp.FindAsync(id);
            ViewData["origem"] = followup.OrigemID;
            ViewData["prosp"] = followup.Origem;

            if (followup == null)
            {
                return NotFound();
            }
            return View(followup);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarFollowUp(int id, [Bind("Id", "OrigemID", "Status", "Anotacoes", "Data", "Vencimento")] FollowUp followup)
        {
            if (id != followup.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Update(followup);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { casa = HttpContext.Session.GetString("_Casa") });
        }

        // GET: FunilDeVendas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Prospeccao prospeccao = await _context.Prospeccao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prospeccao == null)
            {
                return NotFound();
            }

            return View(prospeccao);
        }

        public async Task<IActionResult> RemoverFollowUp(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            FollowUp followup = await _context.FollowUp
                .FirstOrDefaultAsync(m => m.Id == id);

            if (followup == null)
            {
                return NotFound();
            }

            var prospeccao = _context.Prospeccao.FirstOrDefault(p => p.Id == followup.OrigemID);
            if (prospeccao.Status.Count() > 1)
            {
                _context.FollowUp.Remove(followup);
                return RedirectToAction("Index", new { casa = HttpContext.Session.GetString("_Casa") });
            }
            else
            {
                throw new InvalidOperationException("Não é possível remover todas os followups de uma prospecção");
            }

        }

        // POST: FunilDeVendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Prospeccao prospeccao = await _context.Prospeccao.FindAsync(id);

            //Remover os filhos
            var follow_ups = _context.FollowUp.Where(p => p.OrigemID == id).ToList();
            foreach (var followup in follow_ups)
            {
                _context.Remove(followup);
            }
            _context.Prospeccao.Remove(prospeccao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
        }

        private bool ProspeccaoExists(string id)
        {
            return _context.Prospeccao.Any(e => e.Id == id);
        }
    }
}
