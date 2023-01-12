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
using System.Text.Json;

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

            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

            if (string.IsNullOrEmpty(casa))
            {
                casa = usuario.Casa.ToString();
            }

            List<Empresa> empresas = _context.Empresa.ToList();
            List<Prospeccao> lista;
            lista = FunilHelpers.DefinirCasaParaVisualizar(casa, usuario, _context, HttpContext, ViewData);
            lista = FunilHelpers.VincularCasaProspeccao(usuario, lista);
            lista = FunilHelpers.PeriodizarProspecções(ano, lista); // ANO DA PROSPEC
            lista = FunilHelpers.OrdenarProspecções(sortOrder, lista); //SORT ORDEM ALFABETICA
            lista = FunilHelpers.FiltrarProspecções(searchString, lista); //APENAS NA BUSCA
            FunilHelpers.SetarFiltrosNaView(HttpContext, ViewData, sortOrder, searchString);
            FunilHelpers.CategorizarProspecçõesNaView(lista, usuario, HttpContext, ViewBag);
            ViewData["ListaProspeccoes"] = lista.ToList();
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
            ViewData["Equipe"] = new SelectList(_context.Users.ToList(), "Id", "UserName");

            return View();
        }
        // GET: FunilDeVendas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

            List<Empresa> empresas = _context.Empresa.ToList();
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
            ViewData["Equipe"] = new SelectList(_context.Users.ToList(), "Id", "UserName");

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
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

            List<Empresa> empresas = _context.Empresa.ToList();
            ViewData["Empresas"] = new SelectList(empresas, "Id", "EmpresaUnique");
            return View();
        }

        public IActionResult Planejar(int id, string userId)
        {

            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

            userId = HttpContext.User.Identity.Name;
            Instituto usuarioCasa = _context.Users.FirstOrDefault(u => u.UserName == userId).Casa;

            Prospeccao prosp = new Prospeccao
            {
                Id = $"proj_{DateTime.Now.Ticks}",
                Empresa = _context.Empresa.FirstOrDefault(E => E.Id == id),
                Usuario = _context.Users.FirstOrDefault(u => u.UserName == userId),
                Casa = usuarioCasa,
                LinhaPequisa = LinhaPesquisa.Indefinida,
                CaminhoPasta = ""
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

        public IActionResult ProspectarEdital(int id, string userId)
        {

            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

            userId = HttpContext.User.Identity.Name;
            Instituto usuarioCasa = _context.Users.FirstOrDefault(u => u.UserName == userId).Casa;

            Prospeccao prosp = new Prospeccao
            {
                Id = $"proj_{DateTime.Now.Ticks}",
                Empresa = _context.Empresa.FirstOrDefault(E => E.Id == id),
                Usuario = _context.Users.FirstOrDefault(u => u.UserName == userId),
                Casa = usuarioCasa,
                LinhaPequisa = LinhaPesquisa.Indefinida,
                CaminhoPasta = ""
            };
            prosp.Status = new List<FollowUp>
            {
                new FollowUp
                {

                    OrigemID = prosp.Id,
                    Data = DateTime.Today,
                    Anotacoes = $"Prospecção iniciada a partir do edital",
                    Status = StatusProspeccao.ContatoInicial

                }
            };

            _context.Add(prosp);
            _context.SaveChanges();
            return RedirectToAction("Index", "FunilDeVendas");

        }

        [HttpGet]
        public IActionResult Atualizar(string id)
        {
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

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

            return RedirectToAction(nameof(Index), new { id = id });
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
        public async Task<IActionResult> Create([Bind("Id, TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Status, MembrosEquipe, Empresa, Contato, Casa, CaminhoPasta")] Prospeccao prospeccao)
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

            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
			ViewBag.usuarioCasa = usuario.Casa;
			ViewBag.usuarioNivel = usuario.Nivel;

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
        public async Task<IActionResult> Edit(string id, [Bind("Id, TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Empresa, Contato, Casa, Usuario, MembrosEquipe, ValorProposta, ValorEstimado, Status, CaminhoPasta")] Prospeccao prospeccao)
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
                    _context.SaveChanges(); // Essa linha já foi async, talvez seja possível que isso permita ressurgências...
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
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

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
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);

            ViewBag.usuarioCasa = usuario.Casa;
            ViewBag.usuarioNivel = usuario.Nivel;

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
            Usuario usuario = FunilHelpers.ObterUsuarioAtivo(_context, HttpContext);
            Prospeccao prospeccao = _context.Prospeccao.FirstOrDefault(p => p.Id == followup.OrigemID);

            if (verificarCondicoesRemocao(prospeccao, usuario, followup.Origem.Usuario) || usuario.Casa == Instituto.Super)
            {
                _context.FollowUp.Remove(followup);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = followup.OrigemID });
            }
            else
            {
                // TODO: Colocar isso no frontend em vez de jogar uma exceção na cara do usuário
                throw new InvalidOperationException("Não é possível remover todas os followups de uma prospecção");
            }
        }

        private bool verificarCondicoesRemocao(Prospeccao prospeccao, Usuario dono, Usuario ativo)
        {

            return prospeccao.Status.Count() > 1 && dono == ativo;


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

        public IActionResult RetornarModal(string idProsp, string tipo)
        {
            CriarSelectListsDaView();
            if (tipo != null || tipo != "") {
                return ViewComponent($"Modal{tipo}Prosp", new { id = idProsp }); 
            } 
            else
            {
                return ViewComponent("null"); // View n existe é mais pra ""debug""
            }

        }

        public IActionResult RetornarModalEditFollowup(string idProsp, string idFollowup, string tipo)
        {
            CriarSelectListsDaView();
            if (tipo != null || tipo != "")
            {
                return ViewComponent($"Modal{tipo}Prosp", new { id = idProsp, id2 = idFollowup });
            }
            else 
            {
                return ViewComponent("null"); // View debug
            }
        }

        public string PuxarDadosProspeccoes(string casa = "ISIQV", int ano = 2022, bool planejadas = false){
            
            Instituto casa_ = (Instituto)Enum.Parse(typeof(Instituto), casa);

            List<Prospeccao> lista_prosp = _context.Prospeccao.Where(p => p.Casa == casa_).ToList();

            List<Dictionary<string, object>> listaFull = new List<Dictionary<string, object>>();

            foreach (var p in lista_prosp)
            {
                if (p.Status.OrderBy(k=>k.Data).LastOrDefault().Data.Year == ano){
                    if (planejadas){
                        if (p.Status.OrderBy(k=>k.Data).LastOrDefault().Status == StatusProspeccao.Planejada){
                        } else {
                            continue;
                        }
                    }
                    
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict["idProsp"] = p.Id;
                    dict["Status"] = p.Status.OrderBy(k=>k.Data).LastOrDefault().Status.GetDisplayName();
                    dict["Data"] = p.Status.OrderBy(k=>k.Data).LastOrDefault().Data;
                    dict["Empresa"] = p.Empresa.Nome;
                    dict["Segmento"] = p.Empresa.Segmento.GetDisplayName();
                    listaFull.Add(dict);
                } else {
                    continue;
                }
            }

            return JsonSerializer.Serialize(listaFull);

        }

        public string PuxarDadosUsuarios(){
            
            return Helpers.Helpers.PuxarDadosUsuarios(_context);

        }

    }

}