using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using MailSenderApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartTesting.Controllers;
using System;
using System.Collections.Generic;
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
        public IActionResult Index(string casa, string sortOrder, string searchString, string ano)
        {

            //Filtros e ordenadores
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "TipoContratacao" ? "tipo_desc" : "TipoContratacao";

            IQueryable<Prospeccao> lista = DefinirCasa(casa);
            lista = OrdenarProspecções(sortOrder, lista);
            lista = FiltrarProspecções(searchString, lista);
            lista = PeriodizarProspecções(ano, lista);

            return View(lista.ToList<Prospeccao>());
        }

        private IQueryable<Prospeccao> PeriodizarProspecções(string ano, IQueryable<Prospeccao> lista)
        {

            if (string.IsNullOrEmpty(ano))
            {
                ano = DateTime.Now.Year.ToString();
            }

            if (ano.Equals("Todos"))
            {
                return lista;
            }

            return lista.Where(s => s.Status.Any(k => k.Data.Year == Convert.ToInt32(ano)));
        }

        private static IQueryable<Prospeccao> FiltrarProspecções(string searchString, IQueryable<Prospeccao> lista)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                lista = lista.Where(s => s.Empresa.Nome.Contains(searchString)
                                       || s.Usuario.UserName.Contains(searchString));
            }

            return lista;
        }

        private IQueryable<Prospeccao> DefinirCasa(string? casa)
        {

            Casa enum_casa;

            if (string.IsNullOrEmpty(casa))
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Casa")))
                {
                    enum_casa = (Casa)Enum.Parse(typeof(Casa), HttpContext.Session.GetString("_Casa"));
                }
                else { enum_casa = Casa.Super; }
            }
            else
            {
                if (Enum.IsDefined(typeof(Casa), casa))
                {
                    HttpContext.Session.SetString("_Casa", casa);
                    enum_casa = (Casa)Enum.Parse(typeof(Casa), HttpContext.Session.GetString("_Casa"));
                }
                else
                {
                    enum_casa = Casa.Super;
                }
            }


            ViewData["Area"] = casa;

            IQueryable<Prospeccao> lista = enum_casa == Casa.Super ?
                _context.Prospeccao :
                _context.Prospeccao.Where(p => p.Casa.Equals(enum_casa));

            return lista;
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
            var empresas = _context.Empresa.ToList();
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
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
        public async Task<IActionResult> Atualizar(string id, [Bind("OrigemID, Data, Status, Anotacoes, MotivoNaoConversao")] FollowUp followup)
        {
            if (ModelState.IsValid)
            {
                followup.Origem = _context.Prospeccao.FirstOrDefault(p => p.Id == followup.OrigemID);
                _context.Add(followup);

                NotificarProspecção(followup);

                if (followup.Status == StatusProspeccao.Convertida)
                {
                    CriarProjetoConvertido(followup);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
        }

        private async Task AtualizarStatusAsync(FollowUp followup)
        {
            foreach (FollowUp status in followup.Origem.Status)
            {
                status.isTratado = true;

            }
            _context.Update(followup);
            await _context.SaveChangesAsync();
        }

        private void CriarProjetoConvertido(FollowUp followup)
        {

            //Evitar que duplique projetos. TODO: Consertar isso antes da virada do ano
            if (followup.Data.Year != 2020)
            {
                Usuario user = _context.Prospeccao.Find(followup.OrigemID).Usuario;
                Projeto novo_projeto = new Projeto()
                {
                    Casa = followup.Origem.Casa,
                    AreaPesquisa = followup.Origem.LinhaPequisa,
                    Empresa = followup.Origem.Empresa,
                    status = StatusProjeto.EmExecucao,
                    Id = $"proj_{DateTime.Now.Ticks}",
                    Equipe = new List<Usuario>() { user }
                };
                _context.Add(novo_projeto);
                _context.SaveChanges();
            }

        }

        private void NotificarProspecção(FollowUp followup)
        {
            try
            {
                Notificacao notificacao = GerarNotificacao(followup);
                ((EmailSender)_mailer).SendEmailAsync(notificacao);
            }
            catch (ArgumentException)
            {
                //É um status não notificável
                return;
            }
        }

        private static Notificacao GerarNotificacao(FollowUp followup)
        {
            Notificacao notificacao;
            switch (followup.Status)
            {
                case StatusProspeccao.ComProposta:

                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma proposta comercial foi enviada!",
                        TextoBase = EmailTemplate.MontarTemplate($"<h1>Olá,</h1> <br> A prospecção com  a empresa {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo usuário {followup.Origem.Usuario} teve uma proposta enviada. " +
                         $"<hr>" +
                         $"Mais detalhes do contato: {followup.Anotacoes} <hr>")
                    };
                    break;

                case StatusProspeccao.Convertida:
                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma proposta comercial foi convertida!",
                        TextoBase = EmailTemplate.MontarTemplate($"<h1>Olá,</h1>, <br>A proposta com  a empresa {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo usuário {followup.Origem.Usuario} foi convertida." +
                         $"<hr>" +
                         $"Mais detalhes da conversão: {followup.Anotacoes}<hr>")
                    };
                    break;

                case StatusProspeccao.NaoConvertida:
                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma proposta comercial não foi convertida",
                        TextoBase = EmailTemplate.MontarTemplate($"<h1>Olá,</h1><br> A proposta com  a empresa {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo usuário {followup.Origem.Usuario} não foi convertida." +
                         $"<hr>" +
                         $"Mais detalhes: {followup.Anotacoes}<hr>")
                    };
                    break;

                case StatusProspeccao.ContatoInicial:
                    notificacao = new Notificacao
                    {
                        Titulo = "SGI - Uma nova prospecção foi inicializada!",
                        TextoBase = EmailTemplate.MontarTemplate($"<h1>Olá,</h1><br> A prospecção com a empresa {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo usuário {followup.Origem.Usuario} foi iniciada." +
                         $"<hr>" +
                         $"Mais detalhes: {followup.Anotacoes}<hr>")
                    };
                    break;
                default:
                    throw new ArgumentException("Status não notificável");
            }
            return notificacao;
        }

        // POST: FunilDeVendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Status, Empresa, Contato, Casa")] Prospeccao prospeccao)
        {
            if (ModelState.IsValid)
            {
                ValidarEmpresa(prospeccao);
                await VincularUSuario(prospeccao);

                prospeccao.Status[0].Origem = prospeccao;

                NotificarProspecção(prospeccao.Status[0]);

                _context.Add(prospeccao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
        }

        private async Task VincularUSuario(Prospeccao prospeccao)
        {
            string userId = HttpContext.User.Identity.Name;
            Usuario user = await _context.Users.FirstAsync(u => u.UserName == userId);
            prospeccao.Usuario = user;
        }

        private void ValidarEmpresa(Prospeccao prospeccao)
        {
            if (_context.Empresa.Where(e => e.EmpresaUnique == prospeccao.Empresa.EmpresaUnique).Count() > 0)
            {
                prospeccao.Empresa = _context.Empresa.First(e => e.EmpresaUnique == prospeccao.Empresa.EmpresaUnique);
            }
            else
            {
                prospeccao.Empresa = new Empresa { Estado = prospeccao.Empresa.Estado, CNPJ = prospeccao.Empresa.CNPJ, Nome = prospeccao.Empresa.Nome, Segmento = prospeccao.Empresa.Segmento };
            }

            prospeccao.Contato.empresa = prospeccao.Empresa;
        }

        // GET: FunilDeVendas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Empresas"] = new SelectList(_context.Empresa.ToList(), "Id", "EmpresaUnique");
            ViewData["Equipe"] = new SelectList(_context.Users.ToList(), "Id", "UserName");

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
        public async Task<IActionResult> Edit(string id, [Bind("Id,TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Empresa, Contato, Casa, Usuario")] Prospeccao prospeccao)
        {
            if (id != prospeccao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var Empresa_antigo = _context.Prospeccao.Include("Empresa").AsNoTracking().First(p => String.Equals(p.Id , id)).Empresa;
                    if (prospeccao.Empresa.Id != Empresa_antigo.Id) // Nova empresa existente
                    {
                        var empresa = _context.Empresa.First(e => e.Id == prospeccao.Empresa.Id);
                        prospeccao.Empresa = empresa;
                    }
                    else
                    {
                        if(prospeccao.Empresa.Nome != Empresa_antigo.Nome
                            || prospeccao.Empresa.CNPJ != Empresa_antigo.CNPJ) //Nova empresa inexistente
                        {
                            prospeccao.Empresa = new Empresa
                            {
                                Estado = prospeccao.Empresa.Estado,
                                CNPJ = prospeccao.Empresa.CNPJ,
                                Nome = prospeccao.Empresa.Nome,
                                Segmento = prospeccao.Empresa.Segmento
                            };
                        }
                    }

                    Usuario lider = _context.Users.First(p => p.Id == prospeccao.Usuario.Id);
                    prospeccao.Usuario = lider;
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

            return RedirectToAction("Details", new { id = followup.OrigemID }); ;
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

            //TODO: Somente líder ou Super pode remover
            Prospeccao prospeccao = _context.Prospeccao.FirstOrDefault(p => p.Id == followup.OrigemID);
            if (prospeccao.Status.Count() > 1)
            {
                _context.FollowUp.Remove(followup);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = followup.OrigemID }); ;
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
            List<FollowUp> follow_ups = _context.FollowUp.Where(p => p.OrigemID == id).ToList();
            foreach (FollowUp followup in follow_ups)
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


