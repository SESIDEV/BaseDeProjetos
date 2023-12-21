using BaseDeProjetos.Data;
using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Controllers
{
    [Authorize]
    public class IndicadoresController : SGIController
    {
        private readonly ApplicationDbContext _context;

        public IndicadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IndicadoresFinanceiros
        public async Task<IActionResult> Index(string casa)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                List<IndicadoresFinanceiros> listaIndicadoresFinanceiros = await _context.IndicadoresFinanceiros.ToListAsync();
                if (string.IsNullOrEmpty(casa))
                {
                    casa = UsuarioAtivo.Casa.ToString();
                }
                List<IndicadoresFinanceiros> lista = DefinirCasaParaVisualizar(casa);
                lista = VincularCasaAosIndicadoresFinanceiros(UsuarioAtivo, listaIndicadoresFinanceiros);
                return View(lista.ToList());
            }
            else
            {
                return View("Forbidden");
            }
        }

        [Route("Indicadores/IndicadoresDashBoard/{ano?}")]
        public ActionResult IndicadoresDashBoard(int? ano)
        {
            IndicadorHelper indicadoresProspeccoesTotal = new IndicadorHelper(_context.Prospeccao.Where(prospeccao =>
        prospeccao.Status.OrderBy(followup =>
                    followup.Data).LastOrDefault().Status != StatusProspeccao.Planejada).ToList());

            IndicadorHelper indicadoresProspeccoesComProposta = new IndicadorHelper(_context.Prospeccao.Where(prospeccao =>
            prospeccao.Status.OrderBy(followup => followup.Data).LastOrDefault().Status == StatusProspeccao.ComProposta || prospeccao.Status.OrderBy(followup => followup.Data).LastOrDefault().Status == StatusProspeccao.Convertida || prospeccao.Status.OrderBy(followup => followup.Data).LastOrDefault().Status == StatusProspeccao.NaoConvertida).ToList());

            List<Prospeccao> listaParaTaxaDeConversao = _context.Prospeccao.ToList();

            IndicadorHelper indicadorTaxaDeConversao = new IndicadorHelper(listaParaTaxaDeConversao);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Usuario usuario = _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);

                ViewBag.QuantidadeDeProspeccoesPorCasa = indicadoresProspeccoesTotal.QuantidadeDeProspeccoes(p => p?.Casa.GetDisplayName(), ano);
                ViewBag.ValorSomaProspeccoesPorCasa = indicadoresProspeccoesTotal.ValorSomaProspeccoes(p => p?.Casa.GetDisplayName(), ano);

                ViewBag.QuantidadeDeProspeccoesPorEmpresa = indicadoresProspeccoesTotal.QuantidadeDeProspeccoes(p => p?.Empresa.Nome, ano);
                ViewBag.ValorSomaDeProspeccoesPorEmpresa = indicadoresProspeccoesTotal.ValorSomaProspeccoes(p => p?.Empresa.Nome, ano);

                ViewBag.QuantidadeDeProspeccoesPorPesquisador = indicadoresProspeccoesTotal.QuantidadeDeProspeccoes(p => p?.Usuario, ano);
                ViewBag.ValorSomaProspeccoesPorPesquisador = indicadoresProspeccoesTotal.ValorSomaProspeccoes(p => p?.Usuario, ano);

                ViewBag.QuantidadeDeProspeccoesPorTipoContratacao = indicadoresProspeccoesTotal.QuantidadeDeProspeccoes(p => p?.TipoContratacao.GetDisplayName(), ano);
                ViewBag.ValorSomaProspeccoesPorTipoContratacao = indicadoresProspeccoesTotal.ValorSomaProspeccoes(p => p?.TipoContratacao.GetDisplayName(), ano);

                ViewBag.QuantidadeDeProspeccoesPorLinhaDePesquisa = indicadoresProspeccoesTotal.QuantidadeDeProspeccoes(p => p?.LinhaPequisa.GetDisplayName(), ano);
                ViewBag.ValorSomaProspeccoesPorLinhaDePesquisa = indicadoresProspeccoesTotal.ValorSomaProspeccoes(p => p?.LinhaPequisa.GetDisplayName(), ano);

                ViewBag.ValorSomaProspeccoesPorPesquisadorComProposta = indicadoresProspeccoesComProposta.ValorSomaProspeccoes(p => p?.Usuario, ano);

                ViewBag.TaxaDeConversaoDosPesquisadores = indicadorTaxaDeConversao.CalcularTaxaDeConversao(p => p?.Usuario, ano);

                ViewBag.TaxaDeConversaoDasCasas = indicadorTaxaDeConversao.CalcularTaxaDeConversao(p => p?.Casa.GetDisplayName(), ano);

                ViewBag.TaxaDeConversaoDasEmpresas = indicadorTaxaDeConversao.CalcularTaxaDeConversao(p => p?.Empresa.Nome, ano);

                ViewBag.TaxaDeConversaoDosTiposDeContratacao = indicadorTaxaDeConversao.CalcularTaxaDeConversao(p => p?.TipoContratacao.GetDisplayName(), ano);

                ViewBag.TaxaDeConversaoDasLinhasDePesquisa = indicadorTaxaDeConversao.CalcularTaxaDeConversao(p => p?.LinhaPequisa.GetDisplayName(), ano);

                ViewBag.usuarioCasa = usuario.Casa;
                ViewBag.usuarioNivel = usuario.Nivel;
                return View("IndicadoresDashBoard");
            }
            else
            {
                return View("Forbideden");
            }
        }

        /// <summary>
        /// Filtra os indicadores financeiros de acordo com a casa do usuário passado por parâmetro
        /// </summary>
        /// <param name="usuario">?Usuário ativo?</param>
        /// <param name="listaIndicadoresFinanceiros">Indicadores financeiros</param>
        /// <returns></returns>
        public static List<IndicadoresFinanceiros> VincularCasaAosIndicadoresFinanceiros(Usuario usuario, List<IndicadoresFinanceiros> listaIndicadoresFinanceiros)
        {
            if (usuario.Casa == Instituto.Super || usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO)
            {
                return listaIndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList();
            }
            else
            {
                return listaIndicadoresFinanceiros.Where(lista => lista.Casa == usuario.Casa).ToList();
            }
        }

        /// <summary>
        /// Define a casa a qual os indicadores financeiros devem ser filtrados
        /// </summary>
        /// <param name="casa">Nome do instituto</param>
        /// <returns></returns>
        private List<IndicadoresFinanceiros> DefinirCasaParaVisualizar(string casa)
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

            List<IndicadoresFinanceiros> listaIndicadores = new List<IndicadoresFinanceiros>();

            List<IndicadoresFinanceiros> lista = enum_casa == Instituto.Super ?
            _context.IndicadoresFinanceiros.ToList() :
            _context.IndicadoresFinanceiros.Where(p => p.Casa.Equals(enum_casa)).ToList();

            listaIndicadores.AddRange(lista);

            return listaIndicadores.ToList();
        }

        // GET: IndicadoresFinanceiros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);
                if (UsuarioAtivo.Nivel == Nivel.PMO || UsuarioAtivo.Nivel == Nivel.Dev)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    IndicadoresFinanceiros indicadoresFinanceiros = await _context.IndicadoresFinanceiros
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (indicadoresFinanceiros == null)
                    {
                        return NotFound();
                    }

                    return View(indicadoresFinanceiros);
                }
                else
                {
                    return View("Forbidden");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        // GET: IndicadoresFinanceiros/Create
        public IActionResult Create()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);
                if (UsuarioAtivo.Nivel == Nivel.PMO || UsuarioAtivo.Nivel == Nivel.Dev)
                {
                    return View();
                }
                else
                {
                    return View("Forbidden");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: IndicadoresFinanceiros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Receita,Despesa,Investimento,QualiSeguranca, Casa")] IndicadoresFinanceiros indicadoresFinanceiros)
        {
            if (ModelState.IsValid)
            {
                _context.Add(indicadoresFinanceiros);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(indicadoresFinanceiros);
        }

        // GET: IndicadoresFinanceiros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);
                if (UsuarioAtivo.Nivel == Nivel.PMO || UsuarioAtivo.Nivel == Nivel.Dev)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    IndicadoresFinanceiros indicadoresFinanceiros = await _context.IndicadoresFinanceiros.FindAsync(id);
                    if (indicadoresFinanceiros == null)
                    {
                        return NotFound();
                    }
                    return View(indicadoresFinanceiros);
                }
                else
                {
                    return View("Forbidden");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: IndicadoresFinanceiros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Receita,Despesa,Investimento,QualiSeguranca")] IndicadoresFinanceiros indicadoresFinanceiros)
        {
            if (id != indicadoresFinanceiros.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(indicadoresFinanceiros);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IndicadoresFinanceirosExists(indicadoresFinanceiros.Id))
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
            return View(indicadoresFinanceiros);
        }

        // GET: IndicadoresFinanceiros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);
                if (UsuarioAtivo.Nivel == Nivel.PMO || UsuarioAtivo.Nivel == Nivel.Dev)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    IndicadoresFinanceiros indicadoresFinanceiros = await _context.IndicadoresFinanceiros
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (indicadoresFinanceiros == null)
                    {
                        return NotFound();
                    }

                    return View(indicadoresFinanceiros);
                }
                else
                {
                    return View("Forbidden");
                }
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: IndicadoresFinanceiros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            IndicadoresFinanceiros indicadoresFinanceiros = await _context.IndicadoresFinanceiros.FindAsync(id);
            _context.IndicadoresFinanceiros.Remove(indicadoresFinanceiros);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Verifica se um indicador financeiro existe no Banco de Dados
        /// </summary>
        /// <param name="id">ID do Indicador Financeiro</param>
        /// <returns></returns>
        private bool IndicadoresFinanceirosExists(int id)
        {
            return _context.IndicadoresFinanceiros.Any(e => e.Id == id);
        }
    }
}