﻿using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BaseDeProjetos.Controllers
{
    public class HomeController : SGIController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtém a quantidade de prospecções dado um determinado Status
        /// </summary>
        /// <param name="status">Status a se buscar</param>
        /// <returns>Quantidade de prospecções</returns>
        private int GetQuantidadeProspStatus(StatusProspeccao status)
        {
            return _context.Prospeccao.Where(p => p.Status.OrderByDescending(f => f.Data).FirstOrDefault().Status == status).Count();
        }


        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        [Route("Home/Index/{id?}")]
        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewbagizarUsuario(_context);

                ObterDadosReceita(UsuarioAtivo);

                var prospeccoes = _context.Prospeccao
                    .Where(p => p.Casa == UsuarioAtivo.Casa)
                    .ToList();

                var prospeccoesAtivas = prospeccoes
                    .Where(p => VerificarProspeccaoAtiva(p) == true)
                    .ToList();

                // Volume de negocios é o somatório de todos os valores de prospecções
                ViewData["Volume_Negocios"] = prospeccoesAtivas.Sum(p => p.ValorEstimado);
                
                // Para fins de debug
                if (UsuarioAtivo.Casa == Instituto.Super)
                {
                    ViewData["Volume_Negocios"] = _context.Prospeccao
                        .Where(p => p.Casa == Instituto.ISIQV).ToList()
                        .Where(p => VerificarProspeccaoAtiva(p) == true)
                        .Sum(p => p.ValorEstimado);
                }

                ViewData["n_prosp_ativas"] = prospeccoesAtivas.Count;
                ViewData["n_prosp_total"] = prospeccoes.Count;

                ViewData["n_prosp_emproposta"] = GetQuantidadeProspStatus(StatusProspeccao.ComProposta);
                ViewData["n_prosp_planejadas"] = GetQuantidadeProspStatus(StatusProspeccao.Planejada);

                int n_prosp_convertidas = GetQuantidadeProspStatus(StatusProspeccao.Convertida);
                int n_prosp_naoconvertidas = GetQuantidadeProspStatus(StatusProspeccao.NaoConvertida);
                int n_prosp_suspensa = GetQuantidadeProspStatus(StatusProspeccao.Suspensa);

                ViewData["n_prosp_concluidas"] = n_prosp_convertidas + n_prosp_naoconvertidas + n_prosp_suspensa;
                                
                ViewData["n_proj"] = _context.Projeto.Where(p => p.Casa == UsuarioAtivo.Casa).Where(p => p.Status == StatusProjeto.EmExecucao).ToList().Count;
                ViewData["n_empresas"] = _context.Prospeccao.Where(p => p.Status.Any(s => s.Status != StatusProspeccao.Planejada)).Select(e => e.Empresa).Distinct().Count();
                ViewData["satisfacao"] = 0.8750; // TODO: Implementar lógica?
                ViewData["Valor_Prosp_Proposta"] = _context.Prospeccao.Where(p => p.Casa == UsuarioAtivo.Casa).Where(p => p.Status.Any(s => s.Status == StatusProspeccao.ComProposta)).Sum(p => p.ValorProposta);
                ViewBag.Usuarios = _context.Users.AsEnumerable().Where(p => p.Casa == UsuarioAtivo.Casa).Where(u => u.Casa == UsuarioAtivo.Casa).Where(u => u.EmailConfirmed == true).Where(u => u.Nivel != Nivel.Dev && u.Nivel != Nivel.Externos).ToList().Count();
                ViewBag.Estados = _context.Prospeccao.Where(p => p.Casa == UsuarioAtivo.Casa).Select(p => p.Empresa.Estado).ToList();
                ViewBag.LinhaDePesquisa = _context.Prospeccao.Where(p => p.Casa == UsuarioAtivo.Casa && p.Status.Any(f => f.Status != StatusProspeccao.Planejada)).Select(p => p.LinhaPequisa).ToList();

            }
            return View();

        }

        /// <summary>
        /// Obtém os dados da receita de cada instituto, atribuindo-os ao ViewData
        /// </summary>
        private void ObterDadosReceita(Usuario usuarioAtivo)
        {
            //Implementar quando base tiver atualizada
            ViewData["receita_isiqv"] = ReceitaCasa(Instituto.ISIQV);
            ViewData["receita_isiii"] = ReceitaCasa(Instituto.ISIII);
            ViewData["receita_cisho"] = ReceitaCasa(Instituto.CISHO);

            try
            {

                if (usuarioAtivo.Casa == Instituto.Super || usuarioAtivo.Casa == Instituto.ISIQV || usuarioAtivo.Casa == Instituto.CISHO)
                {
                    /*ViewData["receita_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList().LastOrDefault().Receita;
                    ViewData["despesa_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList().LastOrDefault().Despesa;
                    ViewData["invest_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList().LastOrDefault().Investimento;
                    ViewData["Data"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList().LastOrDefault().Data;
                    ViewData["quali"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == Instituto.ISIQV).ToList().LastOrDefault().QualiSeguranca;
                    */
                    ViewData["receita_total"] = 0;
                    ViewData["despesa_total"] = 1;
                    ViewData["invest_total"] = 0;
                    ViewData["quali"] = 0;
                    ViewData["Data"] = DateTime.Today;


                }
                else
                {
                    ViewData["receita_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == usuarioAtivo.Casa).ToList().LastOrDefault().Receita;
                    ViewData["despesa_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == usuarioAtivo.Casa).ToList().LastOrDefault().Despesa;
                    ViewData["invest_total"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == usuarioAtivo.Casa).ToList().LastOrDefault().Investimento;
                    ViewData["Data"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == usuarioAtivo.Casa).ToList().LastOrDefault().Data;
                    ViewData["quali"] = _context.IndicadoresFinanceiros.Where(lista => lista.Casa == usuarioAtivo.Casa).ToList().LastOrDefault().QualiSeguranca;
                }
            }
            catch (Exception)
            {
                ViewData["receita_total"] = 0;
                ViewData["despesa_total"] = 1;
                ViewData["invest_total"] = 0;
                ViewData["quali"] = 0;
                ViewData["Data"] = DateTime.Today;
            }
        }

        /// <summary>
        /// Verifica se uma prospecção está com status considerado ativo
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool VerificarProspeccaoAtiva(Prospeccao p)
        {
            if (p.Status.Count <= 0)
            {
                return false;
            }

            return p.Status.OrderBy(k => k.Data).LastOrDefault().Status <= StatusProspeccao.ComProposta;
        }

        /// <summary>
        /// Retorna o valor da receita para um determinado Instituto
        /// </summary>
        /// <param name="casa">Instituto a se obter o valor</param>
        /// <returns></returns>
        private decimal ReceitaCasa(Instituto casa)
        {
            IQueryable<decimal> valores = _context.Projeto.
                Where(p => p.Casa == casa && p.Status == StatusProjeto.EmExecucao).
                Select(p => CalcularReceita(p));

            decimal sum = 0;
            foreach (decimal valor in valores)
            {
                sum += valor;
            }

            return sum;
        }

        /// <summary>
        /// Retorna a receita dado um projeto
        /// </summary>
        /// <param name="p">Projeto a se obter a receita</param>
        /// <returns></returns>
        private static decimal CalcularReceita(Projeto p)
        {
            decimal valor_aportado = 0M;
            if (p.DataEncerramento < DateTime.Today)
            {
                //O projeto está em execução, mas não recebe mais dinheiro
                return valor_aportado;
            }

            if (p.DataEncerramento.Year == DateTime.Today.Year)
            {
                if (p.DuracaoProjetoEmMeses >= 12)
                {
                    //Caso relativamente simples, o projeto é longo e acaba neste ano, recebendo até o seu término (Jan/19 a Mar/21)
                    return (decimal)((p.ValorAporteRecursos / p.DuracaoProjetoEmMeses) * p.DataEncerramento.Month);
                }
                else
                {
                    //É um projeto curto, que provavelmente começou e vai acabar neste ano (Abril a Outubro)
                    if (p.DataInicio.Year == DateTime.Today.Year)
                    {
                        return (decimal)p.ValorAporteRecursos;
                    }
                    else
                    {
                        //É um projeto curto que já vinha em andamento e que vai terminar neste ano (Ex.: De setembro a março)
                        return (decimal)((p.ValorAporteRecursos / p.DuracaoProjetoEmMeses) * p.DataEncerramento.Month);
                    }
                }
            }
            else
            {
                //Caso mais simples, o projeto tem mais de 12 meses e não começou nem termina este ano (jan/20 a Dez/24)
                if (p.DuracaoProjetoEmMeses >= 12 && p.DataInicio.Year != DateTime.Today.Year)
                {
                    return (decimal)((p.ValorAporteRecursos / p.DuracaoProjetoEmMeses) * 12);
                }

                //O projeto tem menos de 12 meses, mas só acaba no ano seguinte (Set/20 a Mar/21)
                if (p.DataEncerramento.Year == (DateTime.Today.Year + 1))
                {
                    return (decimal)((p.ValorAporteRecursos / p.DuracaoProjetoEmMeses) * (12 - p.DataInicio.Month));
                }
            }

            return valor_aportado;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}