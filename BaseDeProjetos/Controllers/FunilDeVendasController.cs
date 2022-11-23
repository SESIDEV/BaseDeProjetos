using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using BaseDeProjetos.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        [Route("FunilDeVendas/Index/{casa?}/{ano?}")]
        public IActionResult Index(string casa, string sortOrder = "", string searchString = "", string ano = "")
        {

            Usuario usuario = ObterUsuarioAtivo();

            if (string.IsNullOrEmpty(casa))
            {
                casa = usuario.Casa.ToString();

            }
            List<Prospeccao> lista = DefinirCasaParaVisualizar(casa);
            lista = FunilHelpers.VincularCasaProspeccao(usuario, lista);
            lista = FunilHelpers.PeriodizarProspecções(ano, lista);
            lista = FunilHelpers.OrdenarProspecções(sortOrder, lista);
            lista = FunilHelpers.FiltrarProspecções(searchString, lista);
            SetarFiltrosNaView(sortOrder, searchString);
            CategorizarProspecçõesNaView(lista);
            return View(lista.ToList());
        }


        private void CategorizarProspecçõesNaView(List<Prospeccao> lista)
        {
            var concluidos = lista.Where(p => p.Status.Any(f => f.Status == StatusProspeccao.Convertida ||
                                                              f.Status == StatusProspeccao.Suspensa ||
                                                              f.Status == StatusProspeccao.NaoConvertida)).ToList();

            List<Prospeccao> errados = lista.Where(p => p.Status.OrderBy(k => k.Data).FirstOrDefault().Status != StatusProspeccao.ContatoInicial
            && p.Status.OrderBy(k => k.Data).FirstOrDefault().Status != StatusProspeccao.Planejada).ToList();

            List<Prospeccao> emProposta = lista.Where(p => p.Status.OrderBy(k => k.Data).LastOrDefault().Status == StatusProspeccao.ComProposta).ToList();

            List<Prospeccao> ativos = lista.Where(p => p.Status.OrderBy(k => k.Data).LastOrDefault().Status < StatusProspeccao.ComProposta).ToList();

            List<Prospeccao> planejados = lista.Where(p => p.Status.All(f => f.Status == StatusProspeccao.Planejada)).ToList();

            ViewBag.Erradas = errados;
            ViewBag.Concluidas = concluidos;
            ViewBag.Ativas = ativos;
            ViewBag.EmProposta = emProposta;
            ViewBag.Planejadas = planejados;
        }

        private void SetarFiltrosNaView(string sortOrder = "", string searchString = "")
        {
            //Filtros e ordenadores
            if (string.IsNullOrEmpty(searchString) && HttpContext.Session.Keys.Contains("_CurrentFilter"))
            {
                ViewData["CurrentFilter"] = HttpContext.Session.GetString("_CurrentFilter");
            }
            else
            {
                ViewData["CurrentFilter"] = searchString;
                HttpContext.Session.SetString("_CurrentFilter", searchString);
            }

            if (string.IsNullOrEmpty(sortOrder) && HttpContext.Session.Keys.Contains("_CurrentFilter"))
            {
                ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewData["DateSortParm"] = sortOrder == "TipoContratacao" ? "tipo_desc" : "TipoContratacao";
            }
            else
            {
                ViewData["CurrentFilter"] = sortOrder;
                HttpContext.Session.SetString("____", sortOrder);
            }
        }

        private List<Prospeccao> DefinirCasaParaVisualizar(string? casa)
        {
            Instituto enum_casa;

            if (Enum.IsDefined(typeof(Instituto), casa))
            {
                HttpContext.Session.SetString("_Casa", casa);
                enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
            }
            else
            {
                enum_casa = Instituto.Super;
            }


            List<Prospeccao> prospeccoes = new List<Prospeccao>();

            List<Prospeccao> lista = enum_casa == Instituto.Super ?
            _context.Prospeccao.ToList() :
            _context.Prospeccao.Where(p => p.Casa.Equals(enum_casa)).ToList();

            prospeccoes.AddRange(lista);



            ViewData["Area"] = casa;

            return prospeccoes.ToList();
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
        public IActionResult Create(int id)
        {
            List<Empresa> empresas = _context.Empresa.ToList();
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
            return View();
        }

        public IActionResult Planejar(int id, string userId)
        {

            userId = HttpContext.User.Identity.Name;
            Instituto usuarioCasa = _context.Users.FirstOrDefault(u => u.UserName == userId).Casa;

            Prospeccao prosp = new Prospeccao
            {
                Id = $"proj_{DateTime.Now.Ticks}",
                Empresa = _context.Empresa.FirstOrDefault(E => E.Id == id),
                Usuario = _context.Users.FirstOrDefault(u => u.UserName == userId),
                Casa = usuarioCasa,
                LinhaPequisa = LinhaPesquisa.Indefinida
            };
            prosp.Status = new List<FollowUp>
            {
                new FollowUp
                {

                    OrigemID = prosp.Id,
                    Data = DateTime.Today,
                    Anotacoes = $"Incluído no plano de prospecção de {User.Identity.Name}",
                    Status = StatusProspeccao.Planejada

                }
            };

            _context.Add(prosp);
            _context.SaveChanges();
            return RedirectToAction("Index", "Empresas");

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
                Prospeccao prospeccao_origem = _context.Prospeccao.FirstOrDefault(p => p.Id == followup.OrigemID);
                followup.Origem = prospeccao_origem;

                CriarFollowUp(followup);

                bool enviou = MailHelper.NotificarProspecção(followup, _mailer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = id });
        }

        private void CriarFollowUp(FollowUp followup)
        {
            _context.Add(followup);
            _context.SaveChanges();
        }

        // POST: FunilDeVendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Status, Empresa, Contato, Casa")] Prospeccao prospeccao)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    prospeccao = ValidarEmpresa(prospeccao);
                }
                catch (Exception e)
                {
                    return CapturarErro(e);
                }
                prospeccao.Contato.empresa = prospeccao.Empresa;
                await VincularUsuario(prospeccao);

                prospeccao.Status[0].Origem = prospeccao;

                bool enviou = MailHelper.NotificarProspecção(prospeccao.Status[0], _mailer);

                _context.Add(prospeccao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
        }


        private async Task VincularUsuario(Prospeccao prospeccao)
        {
            string userId = HttpContext.User.Identity.Name;
            Usuario user = await _context.Users.FirstAsync(u => u.UserName == userId);
            prospeccao.Usuario = user;
        }

        public Prospeccao ValidarEmpresa(Prospeccao prospeccao)
        {
            if (prospeccao.Empresa.Nome != null && prospeccao.Empresa.CNPJ != null && prospeccao.Empresa.Id == -1)
            {
                Empresa atual = new Empresa { Estado = prospeccao.Empresa.Estado, CNPJ = prospeccao.Empresa.CNPJ, Nome = prospeccao.Empresa.Nome, Segmento = prospeccao.Empresa.Segmento };
                if (atual.Nome != " " && atual.CNPJ != " ")
                {
                    prospeccao.Empresa = atual;

                }
                else
                {
                    throw new Exception("Ocorreu um erro no registro da empresa. \n As informações da empresa não foram submetidas ao banco. \n Contacte um administrador do sistema");
                }
            }
            else
            {
                var existe_empresa = _context.Empresa.FirstOrDefault(e => e.Id == prospeccao.Empresa.Id);

                if (existe_empresa == null)
                {
                    throw new Exception("Ocorreu um erro no registro da empresa. \n A empresa selecionada não foi encontrada. \n Contacte um administrador do sistema");
                }
                else
                {
                    prospeccao.Empresa = existe_empresa;
                }
            }

            return prospeccao;
        }

        // GET: FunilDeVendas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            CriarSelectListsDaView();

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

        private void CriarSelectListsDaView()
        {
            ViewData["Empresas"] = new SelectList(_context.Empresa.ToList(), "Id", "EmpresaUnique");
            ViewData["Equipe"] = new SelectList(_context.Users.ToList(), "Id", "UserName");
        }

        // POST: FunilDeVendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id," + "TipoContratacao, " + "NomeProspeccao, " + "PotenciaisParceiros, " + "LinhaPequisa, Empresa, Contato, Casa, Usuario, ValorProposta, ValorEstimado, Status")] Prospeccao prospeccao)
        {
            if (id != prospeccao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid) 
            {
                try
                {
                    prospeccao = EditarDadosDaProspecção(id, prospeccao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunilHelpers.ProspeccaoExists(prospeccao.Id, _context))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Outro erro de banco, lançar para depuração
                    }
                }
                return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
            }
            return View(prospeccao);
        }
        
        private Prospeccao EditarDadosDaProspecção(string id, Prospeccao prospeccao)
        {
            Empresa Empresa_antigo = _context.Empresa.FirstOrDefault(e => e.Id == prospeccao.Empresa.Id);
            Usuario lider = _context.Users.First(p => p.Id == prospeccao.Usuario.Id);
            prospeccao.Usuario = lider;

            if (prospeccao.Empresa.Id != Empresa_antigo.Id) // Empresa existente
            {
                Empresa empresa = _context.Empresa.FirstOrDefault(e => e.Id == prospeccao.Empresa.Id);
                if (empresa != null)
                {
                    prospeccao.Empresa = empresa;
                }
            }
            prospeccao.Empresa = Empresa_antigo;
            _context.Update(prospeccao);
            return prospeccao;
        }

        public async Task<IActionResult> EditarFollowUp(int? id) // RETONAR VIEW
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
        public async Task<IActionResult> EditarFollowUp(int id, [Bind("Id", "OrigemID", "Status", "Anotacoes", "Data", "Vencimento")] FollowUp followup, double valorProposta)
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

            return RedirectToAction("Details", new { id = followup.OrigemID });
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

            return await RemoverFollowupAutenticado(followup);
        }
        private async Task<IActionResult> RemoverFollowupAutenticado(FollowUp followup)
        {
            //Verifica se o usuário está apto para remover o followup
            Usuario usuario = ObterUsuarioAtivo();
            Prospeccao prospeccao = _context.Prospeccao.FirstOrDefault(p => p.Id == followup.OrigemID);

            if (verificarCondicoesRemocao(prospeccao, usuario, followup.Origem.Usuario) || usuario.Casa == Instituto.Super)
            {
                _context.FollowUp.Remove(followup);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = followup.OrigemID });
            }
            else
            {
                throw new InvalidOperationException("Não é possível remover todas os followups de uma prospecção");
            }
        }

        private bool verificarCondicoesRemocao(Prospeccao prospeccao, Usuario dono, Usuario ativo) {

            return prospeccao.Status.Count() > 1 && dono == ativo;
            

        }

        private Usuario ObterUsuarioAtivo()
        {
            return _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
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
        private IActionResult CapturarErro(Exception e)
        {
            ErrorViewModel erro = new ErrorViewModel
            {
                Mensagem = e.Message
            };
            return View("Error", erro);
        }

    }

}