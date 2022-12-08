using BaseDeProjetos.Data;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BaseDeProjetos.Helpers
{
    public static class FunilHelpers
    {
        public static HtmlString VerificarTemperatura(int qtdDias)
        {
            if (qtdDias < 7)
            {
                return new HtmlString($"<span class='badge badge-funil badge-quente text-dark'>Quente ({qtdDias} Dias)</span>");
            }
            else if (qtdDias >= 7 && qtdDias <= 15)
            {
                return new HtmlString($"<span class='badge badge-funil badge-morno text-dark'>Morno ({qtdDias} Dias)</span>");
            }
            else if (qtdDias >= 16 && qtdDias <= 30)
            {
                return new HtmlString($"<span class='badge badge-funil badge-esfriando text-dark'>Esfriando ({qtdDias} Dias)</span>");
            }
            else
            {
                return new HtmlString($"<span class='badge badge-funil badge-frio text-dark'>Frio ({qtdDias} Dias)</span>");
            }
        }
        public static bool ProspeccaoExists(string id, ApplicationDbContext _context)
        {
            return _context.Prospeccao.Any(e => e.Id == id);
        }
        public static List<Producao> VincularCasaProducao(Usuario usuario, List<Producao> listaProd)
        {
             if (usuario.Nivel == Nivel.Dev) {
                return listaProd.Where(p => 
                p.Casa == Instituto.ISIQV || 
                p.Casa == Instituto.CISHO ||
                p.Casa == Instituto.ISIII ||
                p.Casa == Instituto.ISISVP).ToList();
            }
            else {
                if (usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO)
            {
                return listaProd.Where(p => p.Casa == Instituto.ISIQV || p.Casa == Instituto.CISHO).ToList();

            }
            else
            {
                return listaProd.Where(p => p.Casa == usuario.Casa).ToList();
            }
            }
        }
        public static List<Prospeccao> VincularCasaProspeccao(Usuario usuario, List<Prospeccao> listaProsp)
        {
            if (usuario.Nivel == Nivel.Dev) {
                return listaProsp.Where(p => 
                p.Casa == Instituto.ISIQV || 
                p.Casa == Instituto.CISHO ||
                p.Casa == Instituto.ISIII ||
                p.Casa == Instituto.ISISVP).ToList();
            }

            else {
                if (usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO)
            {
                return listaProsp.Where(p => p.Casa == Instituto.ISIQV || p.Casa == Instituto.CISHO).ToList();

            }
            else
            {
                return listaProsp.Where(p => p.Casa == usuario.Casa).ToList();
            }
            }
        }

        public static void SetarFiltrosNaView(HttpContext HttpContext, ViewDataDictionary ViewData, string sortOrder = "", string searchString = "")
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

        public static void CategorizarProspecçõesNaView(List<Prospeccao> lista, Usuario usuario, HttpContext HttpContext, dynamic ViewBag)
        {

            var concluidos = lista.Where(p => p.Status.Any(f => f.Status == StatusProspeccao.Convertida ||
                                                              f.Status == StatusProspeccao.Suspensa ||
                                                              f.Status == StatusProspeccao.NaoConvertida)).ToList();

            List<Prospeccao> errados = lista.Where(p => p.Status.OrderBy(k => k.Data).FirstOrDefault().Status != StatusProspeccao.ContatoInicial
            && p.Status.OrderBy(k => k.Data).FirstOrDefault().Status != StatusProspeccao.Planejada).ToList();

            List<Prospeccao> emProposta = lista.Where(p => p.Status.OrderBy(k => k.Data).LastOrDefault().Status == StatusProspeccao.ComProposta).ToList();

            List<Prospeccao> ativos = lista.Where(p => p.Status.OrderBy(k => k.Data).All(pa => pa.Status == StatusProspeccao.ContatoInicial || pa.Status == StatusProspeccao.Discussao_EsbocoProjeto)).ToList();

            List<Prospeccao> planejados = usuario.Nivel == Nivel.Dev ?
            lista.Where(p => p.Status.All(f => f.Status == StatusProspeccao.Planejada)).ToList() :
            lista.Where(p => p.Status.All(f => f.Status == StatusProspeccao.Planejada)).Where(u => u.Usuario.ToString() == HttpContext.User.Identity.Name).ToList();

            ViewBag.Erradas = errados;
            ViewBag.Concluidas = concluidos;
            ViewBag.Ativas = ativos;
            ViewBag.EmProposta = emProposta;
            ViewBag.Planejadas = planejados;
        }

        public static List<Prospeccao> DefinirCasaParaVisualizar(string? casa, Usuario usuario, ApplicationDbContext _context, HttpContext HttpContext, ViewDataDictionary ViewData)
        {
            Instituto enum_casa;

            List<Prospeccao> prospeccoes = new List<Prospeccao>();

            if (usuario.Nivel == Nivel.Dev) {
                List<Prospeccao> lista = _context.Prospeccao.ToList();
                prospeccoes.AddRange(lista);
            }

            else {
                
                if (Enum.IsDefined(typeof(Instituto), casa)) {
                    HttpContext.Session.SetString("_Casa", casa);
                    enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
                    List<Prospeccao> lista = _context.Prospeccao.Where(p => p.Casa.Equals(enum_casa)).ToList();

                    prospeccoes.AddRange(lista);

                    ViewData["Area"] = casa;
                }
            }
            
            return prospeccoes.ToList();
        }

        public static List<Producao> DefinirCasaParaVisualizarEmProducao(string? casa, Usuario usuario, ApplicationDbContext _context, HttpContext HttpContext, ViewDataDictionary ViewData)
        {
            Instituto enum_casa;

            List<Producao> producoes = new List<Producao>();

            if (usuario.Nivel == Nivel.Dev) {
                List<Producao> lista = _context.Producao.ToList();
                producoes.AddRange(lista);
            }

            else {
                
                if (Enum.IsDefined(typeof(Instituto), casa)) {
                    HttpContext.Session.SetString("_Casa", casa);
                    enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
                    List<Producao> lista = _context.Producao.Where(p => p.Casa.Equals(enum_casa)).ToList();

                    producoes.AddRange(lista);

                    ViewData["Area"] = casa;
                }
            }
            
            return producoes.ToList();
        }

        public static Usuario ObterUsuarioAtivo(ApplicationDbContext _context, HttpContext HttpContext)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == HttpContext.User.Identity.Name);
        }

        public static List<Prospeccao> PeriodizarProspecções(string ano, List<Prospeccao> lista)
        {
            if (ano.Equals("Todos") || string.IsNullOrEmpty(ano))
            {
                return lista;
            }

            return lista.Where(s => s.Status.Any(k => k.Data.Year == Convert.ToInt32(ano))).ToList();
        }

        public static List<Producao> PeriodizarProduções(string ano, List<Producao> lista)
        {
            if (ano.Equals("Todos") || string.IsNullOrEmpty(ano))
            {
                return lista;
            }

            return lista.Where(k => k.Data.Year == Convert.ToInt32(ano)).ToList();
        }

        public static List<Prospeccao> OrdenarProspecções(string sortOrder, List<Prospeccao> lista)
        {
            var prosps = lista.AsQueryable<Prospeccao>();
            prosps = sortOrder switch
            {
                "name_desc" => prosps.OrderByDescending(s => s.Empresa.Nome),
                "TipoContratacao" => prosps.OrderBy(s => s.TipoContratacao),
                "tipo_desc" => prosps.OrderByDescending(s => s.TipoContratacao),
                _ => prosps.OrderBy(s => s.Status.OrderBy(k => k.Data).Last().Data),
            };
            return prosps.ToList();
        }
        public static List<Prospeccao> FiltrarProspecções(string searchString, List<Prospeccao> lista)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                lista = lista.Where(s => s.Empresa.Nome.ToLower().Contains(searchString) || s.Usuario.UserName.ToLower().Contains(searchString)).ToList();
            }

            return lista;
        }
        public static List<Producao> FiltrarProduções(string searchString, List<Producao> lista)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                if (lista[0].Empresa != null){

                    lista = lista.Where(s => 
                        s.Autores.ToLower().Contains(searchString) ||
                        s.Titulo.ToLower().Contains(searchString) ||
                        s.Empresa.Nome.ToLower().Contains(searchString) ||
                        s.Projeto.NomeProjeto.ToLower().Contains(searchString)
                        ).ToList();
                } else {
                    lista = lista.Where(s => 
                        s.Autores.ToLower().Contains(searchString) ||
                        s.Titulo.ToLower().Contains(searchString)
                        ).ToList();
                }               
            }

            return lista;
        }
        public static bool VerificarContratacaoDireta(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.ContratacaoDireta;
        }
        public static bool VerificarContratacaoEditalInovacao(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.EditalInovacao;
        }
        public static bool VerificarContratacaoAgenciaFomento(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.AgenciaFomento;
        }
        public static bool VerificarContratacaoEmbrapii(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.Embrapii;
        }
        public static bool VerificarContratacaoIndefinida(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.Indefinida;
        }
        public static bool VerificarContratacaoParceiro(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.Parceiro;
        }
        public static bool VerificarContratacaoANP(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.ANP;
        }
        public static bool VerificarStatusContatoInicial(Prospeccao p)
        {
            if (p.Status.Count == 0)
            {
                return false;
            }
            return p.Status.Any(s => s.Status == StatusProspeccao.ContatoInicial);
        }
        public static bool VerificarStatusEmDiscussao(Prospeccao p)
        {
            if (p.Status.Count == 0)
            {
                return false;
            }
            return p.Status.Any(k => k.Status < StatusProspeccao.ComProposta && k.Status > StatusProspeccao.ContatoInicial);
        }
        public static bool VerificarStatusComProposta(Prospeccao p)
        {
            if (p.Status.Count == 0)
            {
                return false;
            }
            return p.Status.Any(s => s.Status == StatusProspeccao.ComProposta);
        }
    }

}