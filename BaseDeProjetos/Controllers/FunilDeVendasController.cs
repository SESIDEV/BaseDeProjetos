﻿using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
//using MailSenderApp.Services;
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
    public IActionResult Index(string casa, string sortOrder = "", string searchString = "", string ano = "")
    {
      Usuario usuario = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
      casa = usuario.Casa.ToString();
      List<Prospeccao> lista = DefinirCasa(casa);
      lista = VincularCasaProspeccao(usuario, lista);
      lista = PeriodizarProspecções(ano, lista);
      lista = OrdenarProspecções(sortOrder, lista);
      lista = FiltrarProspecções(searchString, lista);
      SetarFiltros(sortOrder, searchString);
      CategorizarProspecções(lista);
      return View(lista.ToList());
    }

    public static List<Prospeccao> VincularCasaProspeccao(Usuario usuario, List<Prospeccao> listaProsp)
    {

      if (usuario.Casa == Instituto.Super || usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO)
      {
        return listaProsp.Where(p => p.Casa == Instituto.ISIQV || p.Casa == Instituto.CISHO).ToList();

      }
      else
      {
        return listaProsp.Where(p => p.Casa == usuario.Casa).ToList();
      }

    }

    private void CategorizarProspecções(List<Prospeccao> lista)
    {
      var concluidos = lista.Where(p => p.Status.Any(f => f.Status == StatusProspeccao.Convertida ||
                                                        f.Status == StatusProspeccao.Suspensa ||
                                                        f.Status == StatusProspeccao.NaoConvertida)).ToList();

      List<Prospeccao> errados = lista.Where(p => p.Status.FirstOrDefault().Status != StatusProspeccao.ContatoInicial).ToList();

      List<Prospeccao> emProposta = lista.Where(p => p.Status.LastOrDefault().Status == StatusProspeccao.ComProposta).ToList();

      List<Prospeccao> ativos = lista.Where(p => p.Status.LastOrDefault().Status < StatusProspeccao.ComProposta).ToList();

      List<Prospeccao> planejados = lista.Where(p => p.Status.All(f => f.Status == StatusProspeccao.Planejada)).ToList();

      ViewBag.Erradas = errados;
      ViewBag.Concluidas = concluidos;
      ViewBag.Ativas = ativos;
      ViewBag.EmProposta = emProposta;
      ViewBag.Planejadas = planejados;
    }

    private void SetarFiltros(string sortOrder = "", string searchString = "")
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

    private List<Prospeccao> PeriodizarProspecções(string ano, List<Prospeccao> lista)
    {
      if (ano.Equals("Todos") || string.IsNullOrEmpty(ano))
      {
        return lista;
      }

      return lista.Where(s => s.Status.Any(k => k.Data.Year == Convert.ToInt32(ano))).ToList();
    }

    private static List<Prospeccao> FiltrarProspecções(string searchString, List<Prospeccao> lista)
    {

      if (!string.IsNullOrEmpty(searchString))
      {
        searchString = searchString.ToLower();

        lista = lista.Where(s => s.Empresa.Nome.ToLower().Contains(searchString) || s.Usuario.UserName.ToLower().Contains(searchString)).ToList();
      }

      return lista;
    }

    private List<Prospeccao> DefinirCasa(string? casa)
    {
      Instituto enum_casa;

      List<Prospeccao> prospeccoes = new List<Prospeccao>();

      if (string.IsNullOrEmpty(casa))
      {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Casa")))
        {
          enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
        }
        else
        {
          return _context.Prospeccao.ToList();
        }
      }
      else
      {
        List<string> casas = ProcessarCasa(casa);
        if (casas.Count() == 0)
        {

          DefinirCasa(null);

        }
        foreach (string c in casas)
        {

          if (Enum.IsDefined(typeof(Instituto), c))
          {
            HttpContext.Session.SetString("_Casa", c);
            enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
          }
          else
          {
            enum_casa = Instituto.Super;
          }

          List<Prospeccao> lista = enum_casa == Instituto.Super ?
          _context.Prospeccao.ToList() :
          _context.Prospeccao.Where(p => p.Casa.Equals(enum_casa)).ToList();

          prospeccoes.AddRange(lista);
        }
      }

      ViewData["Area"] = casa;

      return prospeccoes.ToList();
    }

    private List<string> ProcessarCasa(string? casa)
    {

      return casa.Split("-").ToList();

    }

    private static List<Prospeccao> OrdenarProspecções(string sortOrder, List<Prospeccao> lista)
    {
      var prosps = lista.AsQueryable<Prospeccao>();
      prosps = sortOrder switch
      {
        "name_desc" => prosps.OrderByDescending(s => s.Empresa.Nome),
        "TipoContratacao" => prosps.OrderBy(s => s.TipoContratacao),
        "tipo_desc" => prosps.OrderByDescending(s => s.TipoContratacao),
        _ => prosps.OrderBy(s => s.Empresa.Nome),
      };
      return prosps.ToList();
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

      Prospeccao prosp = new Prospeccao();
      prosp.Id = $"proj_{DateTime.Now.Ticks}";
      prosp.Empresa = _context.Empresa.FirstOrDefault(E => E.Id == id);
      prosp.Usuario = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
      prosp.Casa = prosp.Usuario.Casa;
      prosp.Status = new List<FollowUp>();
      prosp.Status.Add(new FollowUp
      {

        OrigemID = prosp.Id,
        Data = DateTime.Today,
        Anotacoes = "Incluído no plano de prospecção de" + User.Identity.Name,
        Status = StatusProspeccao.Planejada

      });

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
        AcertarCasosEspeciais(followup);

        //  NotificarProspecção(followup);
        await _context.SaveChangesAsync();
      }

      return RedirectToAction(nameof(Index), new { casa = HttpContext.Session.GetString("_Casa") });
    }

    private void AcertarCasosEspeciais(FollowUp followup)
    {

      if (followup.Status == StatusProspeccao.Convertida)
      {
        CriarProjetoConvertido(followup);
      }
    }

    private void CriarFollowUp(FollowUp followup)
    {
      _context.Add(followup);
      _context.SaveChanges();
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
          Status = StatusProjeto.EmExecucao,
          Id = $"proj_{DateTime.Now.Ticks}",
          Equipe = new List<Usuario>() { user }
        };
        _context.Add(novo_projeto);
        _context.SaveChanges();
      }
    }
    /*
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

            case StatusProspeccao.Discussao_EsbocoProjeto:
              notificacao = new Notificacao
              {
                Titulo = "SGI - Uma prospecção está avançando!",
                TextoBase = EmailTemplate.MontarTemplate($"<h1>Olá,</h1><br> A prospecção com a empresa {followup.Origem.Empresa.Nome} - {followup.Origem.Empresa.CNPJ} na linha {followup.Origem.LinhaPequisa}, iniciada pelo usuário {followup.Origem.Usuario} foi chegou à etapa do esboço de projeto. A proposta deverá ser enviada até o dia {DateTime.Today.AddDays(14)}." +
                   $"<hr>" +
                   $"Mais detalhes: {followup.Anotacoes}<hr>")
              };

              break;

            default:
              throw new ArgumentException("Status não notificável");
          }

          notificacao.Status = followup.Status;
          return notificacao;
        }
        */
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
          ValidarEmpresa(prospeccao);
        }
        catch (Exception e)
        {
          ErrorViewModel erro = new ErrorViewModel
          {
            Mensagem = e.Message
          };
          return View("Error", erro);
        }
        prospeccao.Contato.empresa = prospeccao.Empresa;
        await VincularUsuario(prospeccao);

        prospeccao.Status[0].Origem = prospeccao;

        //  NotificarProspecção(prospeccao.Status[0]);

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

    public void ValidarEmpresa(Prospeccao prospeccao)
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
        /*int existe_empresa = _context.Empresa.Where(e => e.Id == prospeccao.Empresa.Id).Count();*/

        if (existe_empresa != null)
        {
          prospeccao.Empresa = existe_empresa;
        }
        else
        {
          throw new Exception("Ocorreu um erro no registro da empresa. \n A empresa selecionada não foi encontrada. \n Contacte um administrador do sistema");
        }
      }
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
    public async Task<IActionResult> Edit(string id, [Bind("Id,TipoContratacao, NomeProspeccao, PotenciaisParceiros, LinhaPequisa, Empresa, Contato, Casa, Usuario, ValorProposta, ValorEstimado")] Prospeccao prospeccao)
    {
      if (id != prospeccao.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid) //APAGAR EXCESSO DE CÓDIGO
      {
        try
        {
          Empresa Empresa_antigo = _context.Prospeccao.Include("Empresa").AsNoTracking().First(p => string.Equals(p.Id, id)).Empresa;
          if (prospeccao.Empresa.Id != Empresa_antigo.Id) // Nova empresa existente
          {
            Empresa empresa = _context.Empresa.First(e => e.Id == prospeccao.Empresa.Id);
            prospeccao.Empresa = empresa;
          }
          else
          {
            prospeccao.Empresa = Empresa_antigo;
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