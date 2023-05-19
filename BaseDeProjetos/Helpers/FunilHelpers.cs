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
                return new HtmlString($"<span class='badge bg-quente'>Quente: ({qtdDias} Dias)</span>");
            }
            else if (qtdDias >= 7 && qtdDias <= 15)
            {
                return new HtmlString($"<span class='badge bg-morno'>Morno: ({qtdDias} Dias)</span>");
            }
            else if (qtdDias >= 16 && qtdDias <= 30)
            {
                return new HtmlString($"<span class='badge bg-esfriando text-dark'>Esfriando: ({qtdDias} Dias)</span>");
            }
            else if (qtdDias > 30 && qtdDias <= 365)
            {
                return new HtmlString($"<span class='badge bg-frio text-dark'>Frio: ({qtdDias} Dias)</span>");
            }
            else
            {
                return new HtmlString($"<span class='badge bg-frio text-dark'>Congelado: ({qtdDias} Dias)</span>");
            }
        }
        public static bool ProspeccaoExists(string id, ApplicationDbContext _context)
        {
            return _context.Prospeccao.Any(prospeccao => prospeccao.Id == id);
        }
        public static List<Producao> VincularCasaProducao(Usuario usuario, List<Producao> listaProd)
        {
            if (usuario.Nivel == Nivel.Dev)
            {
                return listaProd.Where(producao =>
                producao.Casa == Instituto.ISIQV ||
                producao.Casa == Instituto.CISHO ||
                producao.Casa == Instituto.ISIII ||
                producao.Casa == Instituto.ISISVP).ToList();
            }
            else
            {
                if (usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO)
                {
                    return listaProd.Where(producao => producao.Casa == Instituto.ISIQV || producao.Casa == Instituto.CISHO).ToList();

                }
                else
                {
                    return listaProd.Where(producao => producao.Casa == usuario.Casa).ToList();
                }
            }
        }
        public static List<Prospeccao> VincularCasaProspeccao(Usuario usuario, List<Prospeccao> listaProsp)
        {
            if (usuario.Nivel == Nivel.Dev)
            {
                return listaProsp.Where(prospeccao =>
                prospeccao.Casa == Instituto.ISIQV ||
                prospeccao.Casa == Instituto.CISHO ||
                prospeccao.Casa == Instituto.ISIII ||
                prospeccao.Casa == Instituto.ISISVP).ToList();
            }

            else
            {
                if (usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO)
                {
                    return listaProsp.Where(prospeccao => prospeccao.Casa == Instituto.ISIQV || prospeccao.Casa == Instituto.CISHO).ToList();

                }
                else
                {
                    return listaProsp.Where(prospeccao => prospeccao.Casa == usuario.Casa).ToList();
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

        public static void CategorizarProspecçõesNaView(List<Prospeccao> lista, Usuario usuario, string aba, HttpContext HttpContext, dynamic ViewBag)
        {

            if (aba.ToLowerInvariant() == "ativas") 
            {
                List<Prospeccao> ativas = lista.Where(prospeccao => prospeccao.Status.OrderBy(followup => followup.Data).LastOrDefault().Status < StatusProspeccao.ComProposta).ToList();
                ViewBag.Ativas = ativas;
            }
            else if (aba.ToLowerInvariant() == "comproposta")
            {
                List<Prospeccao> emProposta = lista.Where(prospeccao => prospeccao.Status.OrderBy(followup => followup.Data).LastOrDefault().Status == StatusProspeccao.ComProposta).ToList();
                ViewBag.EmProposta = emProposta;
            } 
            else if (aba.ToLowerInvariant() == "concluidas")
            {
                List<Prospeccao> concluidas = lista.Where(prospeccao => prospeccao.Status.Any(followup => followup.Status == StatusProspeccao.Convertida ||
                                                              followup.Status == StatusProspeccao.Suspensa ||
                                                              followup.Status == StatusProspeccao.NaoConvertida)).ToList();
                ViewBag.Concluidas = concluidas;
            } 
            else if (aba.ToLowerInvariant() == "planejadas")
            {
                // Jesus cristo 😬
                List<Prospeccao> planejadas = usuario.Nivel == Nivel.Dev ?
                            lista.Where(prospeccao => prospeccao.Status.All(followup => followup.Status == StatusProspeccao.Planejada)).ToList() :
                            lista.Where(prospeccao => prospeccao.Status.All(followup => followup.Status == StatusProspeccao.Planejada)).Where(prosp => prosp.Usuario.UserName.ToString() == HttpContext.User.Identity.Name).ToList();
                ViewBag.Planejadas = planejadas;
            } 
            else if (aba.ToLowerInvariant() == "erradas")
            {
                List<Prospeccao> erradas = lista.Where(prospeccao => prospeccao.Status.OrderBy(followup => followup.Data).FirstOrDefault().Status != StatusProspeccao.ContatoInicial
                            && prospeccao.Status.OrderBy(followup => followup.Data).FirstOrDefault().Status != StatusProspeccao.Planejada).ToList();
                ViewBag.Erradas = erradas;
            } 
            else
            {
                return;
            }            
        }

        public static List<Prospeccao> DefinirCasaParaVisualizar(string? casa, Usuario usuario, ApplicationDbContext _context, HttpContext HttpContext, ViewDataDictionary ViewData)
        {
            Instituto enum_casa;

            List<Prospeccao> prospeccoes = new List<Prospeccao>();

            if (usuario.Nivel == Nivel.Dev)
            {
                List<Prospeccao> lista = _context.Prospeccao.ToList();
                prospeccoes.AddRange(lista);
            }

            else
            {

                if (Enum.IsDefined(typeof(Instituto), casa))
                {
                    HttpContext.Session.SetString("_Casa", casa);
                    enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
                    List<Prospeccao> lista = _context.Prospeccao.Where(prospeccao => prospeccao.Casa.Equals(enum_casa)).ToList();

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

            if (usuario.Nivel == Nivel.Dev)
            {
                List<Producao> lista = _context.Producao.ToList();
                producoes.AddRange(lista);
            }

            else
            {

                if (Enum.IsDefined(typeof(Instituto), casa))
                {
                    HttpContext.Session.SetString("_Casa", casa);
                    enum_casa = (Instituto)Enum.Parse(typeof(Instituto), HttpContext.Session.GetString("_Casa"));
                    List<Producao> lista = _context.Producao.Where(producao => producao.Casa.Equals(enum_casa)).ToList();

                    producoes.AddRange(lista);

                    ViewData["Area"] = casa;
                }
            }

            return producoes.ToList();
        }

        public static Usuario ObterUsuarioAtivo(ApplicationDbContext _context, HttpContext HttpContext)
        {
            return _context.Users.ToList().FirstOrDefault(usuario => usuario.UserName == HttpContext.User.Identity.Name);
        }

        public static List<Prospeccao> PeriodizarProspecções(string ano, List<Prospeccao> lista)
        {
            if (ano.Equals("Todos") || string.IsNullOrEmpty(ano))
            {
                return lista;
            }

            return lista.Where(prospeccao => prospeccao.Status.Any(followup => followup.Data.Year == Convert.ToInt32(ano))).ToList();
        }

        public static List<Producao> PeriodizarProduções(string ano, List<Producao> lista)
        {
            if (ano.Equals("Todos") || string.IsNullOrEmpty(ano))
            {
                return lista;
            }

            return lista.Where(producao => producao.Data.Year == Convert.ToInt32(ano)).ToList();
        }

        public static TimeSpan RetornarValorDiferencaTempo(List<Prospeccao> prospeccoes)
        {
            TimeSpan intervaloDeTempo;
            if (prospeccoes.Count() > 0)
            {
                intervaloDeTempo = prospeccoes.Aggregate(new TimeSpan(0),
                (inicial, prospeccao) => prospeccao.Status.FirstOrDefault(s => s.Status == StatusProspeccao.ComProposta).Data - prospeccao.Status.First().Data, diff => diff);

                intervaloDeTempo = new TimeSpan(intervaloDeTempo.Ticks / prospeccoes.Count());
            }
            else
            {
                intervaloDeTempo = new TimeSpan(0);
            }

            return intervaloDeTempo;
        }

        public static List<Prospeccao> OrdenarProspecções(string sortOrder, List<Prospeccao> lista)
        {
            var prospeccoes = lista.AsQueryable<Prospeccao>();
            prospeccoes = sortOrder switch
            {
                "name_desc" => prospeccoes.OrderByDescending(s => s.Empresa.Nome),
                "TipoContratacao" => prospeccoes.OrderBy(s => s.TipoContratacao),
                "tipo_desc" => prospeccoes.OrderByDescending(s => s.TipoContratacao),
                _ => prospeccoes.OrderBy(s => s.Status.OrderBy(k => k.Data).Last().Data),
            };
            return prospeccoes.ToList();
        }
        public static List<Prospeccao> FiltrarProspecções(string searchString, List<Prospeccao> prospeccoes)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                prospeccoes = prospeccoes.Where(p =>
                ChecarSubstring(searchString, p.Empresa.Nome) ||
                ChecarSubstring(searchString, p.Usuario.UserName) ||
                ChecarSubstring(searchString, p.NomeProspeccao) ||
                ChecarSubstring(searchString, p.MembrosEquipe)

                ).ToList();
            }

            return prospeccoes;
        }

        private static bool ChecarSubstring(string searchString, string campo)
        {
            if (campo != null)
            {
                return campo.ToLower().Contains(searchString);
            }
            else
            {
                return false;
            }
        }

        public static List<Producao> FiltrarProduções(string searchString, List<Producao> producoes)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                if (producoes[0].Empresa != null)
                {

                    producoes = producoes.Where(producao =>
                        producao.Autores.ToLower().Contains(searchString) ||
                        producao.Titulo.ToLower().Contains(searchString) ||
                        producao.Empresa.Nome.ToLower().Contains(searchString) ||
                        producao.Projeto.NomeProjeto.ToLower().Contains(searchString)
                        ).ToList();
                }
                else
                {
                    producoes = producoes.Where(producao =>
                        producao.Autores.ToLower().Contains(searchString) ||
                        producao.Titulo.ToLower().Contains(searchString)
                        ).ToList();
                }
            }

            return producoes;
        }
        public static bool VerificarContratacao(Prospeccao prospeccao, TipoContratacao tipoContratacao)
        {
            switch (tipoContratacao)
            {
                case TipoContratacao.ContratacaoDireta:
                    return prospeccao.TipoContratacao == tipoContratacao;
                case TipoContratacao.EditalInovacao:
                    return prospeccao.TipoContratacao == tipoContratacao;
                case TipoContratacao.AgenciaFomento:
                    return prospeccao.TipoContratacao == tipoContratacao;
                case TipoContratacao.Embrapii:
                    return prospeccao.TipoContratacao == tipoContratacao;
                case TipoContratacao.Indefinida:
                    return prospeccao.TipoContratacao == tipoContratacao;
                case TipoContratacao.Parceiro:
                    return prospeccao.TipoContratacao == tipoContratacao;
                case TipoContratacao.ANP:
                    return prospeccao.TipoContratacao == tipoContratacao;
                default:
                    return false;
            }
        }

        public static bool VerificarStatus(Prospeccao prospeccao, StatusProspeccao status)
        {
            if (prospeccao.Status.Count == 0)
            {
                return false;
            }

            switch (status)
            {
                case StatusProspeccao.ContatoInicial:
                    return prospeccao.Status.Any(s => s.Status == status);
                case StatusProspeccao.Discussao_EsbocoProjeto: // Status seria < 5 e > 0 1-4 INCLUSO, se precisar inclua um case acima sem execução de nenhuma instrução ou break
                    return prospeccao.Status.Any(followup => followup.Status < StatusProspeccao.ComProposta && followup.Status > StatusProspeccao.ContatoInicial);
                case StatusProspeccao.ComProposta:
                    return prospeccao.Status.Any(followup => followup.Status == status);
                default:
                    return false;
            }
        }
    }
}