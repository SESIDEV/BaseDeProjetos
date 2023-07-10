using BaseDeProjetos.Data;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;

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

        public static List<Prospeccao> RetornarProspeccoesPorStatus(List<Prospeccao> lista, Usuario usuario, string aba, HttpContext HttpContext)
        {

            if (aba.ToLowerInvariant() == "ativas") 
            {
                List<Prospeccao> ativas = lista.Where(prospeccao => prospeccao.Status.OrderBy(followup =>
                    followup.Data).LastOrDefault().Status < StatusProspeccao.ComProposta).ToList();
                return ativas;
            }
            else if (aba.ToLowerInvariant() == "comproposta")
            {
                List<Prospeccao> emProposta = lista.Where(prospeccao => prospeccao.Status.OrderBy(followup =>
                    followup.Data).LastOrDefault().Status == StatusProspeccao.ComProposta).ToList();
                return emProposta;
            } 
            else if (aba.ToLowerInvariant() == "concluidas")
            {
                List<Prospeccao> concluidas = lista.Where(prospeccao => prospeccao.Status.Any(followup =>
                    followup.Status == StatusProspeccao.Convertida ||
                    followup.Status == StatusProspeccao.Suspensa ||
                    followup.Status == StatusProspeccao.NaoConvertida
                )).ToList();
                return concluidas;
            } 
            else if (aba.ToLowerInvariant() == "planejadas")
            {
                // Jesus cristo 😬
                List<Prospeccao> planejadas = usuario.Nivel == Nivel.Dev ?
                    lista.Where(prospeccao => prospeccao.Status.All(followup => followup.Status == StatusProspeccao.Planejada)).ToList() :
                    lista.Where(prospeccao => prospeccao.Status.All(followup => followup.Status == StatusProspeccao.Planejada)).Where(prosp => prosp.Usuario.UserName.ToString() == HttpContext.User.Identity.Name).ToList();
                return planejadas;
            } 
            else if (aba.ToLowerInvariant() == "erradas")
            {
                List<Prospeccao> erradas = lista.Where(prospeccao =>
                    prospeccao.Status.OrderBy(followup => followup.Data).FirstOrDefault().Status != StatusProspeccao.ContatoInicial &&
                    prospeccao.Status.OrderBy(followup => followup.Data).FirstOrDefault().Status != StatusProspeccao.Planejada
                ).ToList();
                return erradas;
            } 
            else
            {
                return null;
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

        public static void AddAgregadas(ApplicationDbContext _context, Prospeccao prospAntiga, Prospeccao prospeccao)
        {
            if(!prospeccao.Agregadas.IsNullOrEmpty()){
                var listaIdsAggAntiga = prospAntiga.Agregadas?.Split(";") ?? Array.Empty<string>(); //puxa os dados das agregadas da prosp antiga
                var listaIdsAggNova = prospeccao.Agregadas.Split(";"); //separa os Ids das agregadas da prosp nova

                foreach (string AggId in listaIdsAggNova) //itera pelas agregadas
                {
                    if(AggId != "" && !listaIdsAggAntiga.Contains(AggId)){
                        var prospAgg = _context.Prospeccao.Where(prosp => prosp.Id == AggId).First();

                        FollowUp statusAgg = new FollowUp{
                            Status = StatusProspeccao.Agregada,
                            Data = DateTime.Today,
                            Anotacoes = "Esta prospecção foi agregada à um grupo.",
                        }; //cria o status agregada

                        prospAgg.Status.Add(statusAgg); //adiciona o status ao final da lista de followups
                    }
                }
            }
        }

        public static void DelAgregadas(ApplicationDbContext _context, Prospeccao prospAntiga, Prospeccao prospeccao)
        {
            if (!prospAntiga.Agregadas.IsNullOrEmpty()){
                var listaAggAntiga = prospAntiga.Agregadas.Split(";");
                var listaAggNova = prospeccao.Agregadas.Split(";"); //separa os Ids
                var listaRemovidas = new List<string>(); // lista dos ids que não estão presentes na nova lista

                foreach (string AggId in listaAggAntiga) // itera pelas agregadas da lista antiga
                {
                    if(AggId != ""){
                        
                        if(!listaAggNova.Contains(AggId)){
                            listaRemovidas.Add(AggId);
                        }
                    }
                }

                if(!listaRemovidas.IsNullOrEmpty()){ // itera pelas removidas
                    foreach (string ids in listaRemovidas){
                        var antigaAgg = _context.Prospeccao.Where(prosp => prosp.Id == ids).First(); // seleciona a prosp antiga referente ao Id do loop (PODE DAR PROBLEMA SE A PROSP NAO EXISTIR MAIS)
                        var AggUltimoStatusData = antigaAgg.Status.Last().Data; // salva a data do último followup
                        var statusMaisRecentes = prospeccao.Status.Where(s => s.Data > AggUltimoStatusData).ToList(); // busca por status da prosp atual que sejam de datas posteriores ao último status da antiga agregada
                        
                        if(statusMaisRecentes.IsNullOrEmpty()){// se não houver diferença de status, add um novo status
                            FollowUp statusDeagg = new FollowUp{
                                Status = antigaAgg.Status.OrderByDescending(d => d.Data).ToList()[1].Status, // retorna ao status anterior ao agregado
                                Data = DateTime.Today,
                                Anotacoes = "Esta prospecção foi desagregada de um grupo."
                            };
                            
                            antigaAgg.Status.Add(statusDeagg); //adiciona o status ao final da lista de followups
                        } else {
                            List<FollowUp> listaCopia = new List<FollowUp>(statusMaisRecentes.Count);

                            statusMaisRecentes.ForEach(s => {
                                var copiaStatus = new FollowUp{
                                    OrigemID = ids, // essa é a própria id do loop, da prosp que está sendo removida
                                    Anotacoes = s.Anotacoes,
                                    AnoFiscal = s.AnoFiscal,
                                    Status = s.Status,
                                    Data = s.Data
                                };
                                listaCopia.Add(copiaStatus);
                            });
                            antigaAgg.Status.AddRange(listaCopia);
                        }
                        _context.Entry(antigaAgg).State = EntityState.Detached;
                    }
                }
            }
        }

        public static string MostrarAgregadas(List<Prospeccao> aggTodas, string agregadas)
        {
            if(!aggTodas.IsNullOrEmpty()){
                string empresas = "";
                List<string> agg = agregadas.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (var p in aggTodas)
                {
                    if(agg.Contains(p.Id)){
                        empresas += p.Empresa.NomeFantasia + ";";
                    }
                }

                return empresas.Replace(";", "<br>");
            } else {
                return "";
            }
        }

        public static void RepassarStatusAoCancelarAncora(ApplicationDbContext _context, Prospeccao prospeccao)
        {
            if(!prospeccao.Agregadas.IsNullOrEmpty()){
                var listaAggIds = prospeccao.Agregadas.Split(";"); //separa os Ids

                foreach (string AggId in listaAggIds) //itera pelas agregadas
                {
                    if(AggId != ""){
                        var prospAgg = _context.Prospeccao.Where(prosp => prosp.Id == AggId).First();
                        var AggUltimoStatusData = prospAgg.Status.Last().Data;
                        var statusMaisRecentes = prospeccao.Status.Where(s => s.Data > AggUltimoStatusData).ToList();
                        if(statusMaisRecentes.IsNullOrEmpty()){// se não houver diferença de status, add um novo status
                            FollowUp statusDeagg = new FollowUp{
                                Status = prospAgg.Status.OrderByDescending(d => d.Data).ToList()[1].Status, // retorna ao status anterior ao agregado
                                Data = DateTime.Today,
                                Anotacoes = "Esta prospecção foi desagregada de um grupo."
                            };
                            
                            prospAgg.Status.Add(statusDeagg); //adiciona o status ao final da lista de followups
                        } else {
                            List<FollowUp> listaCopia = new List<FollowUp>(statusMaisRecentes.Count);

                            statusMaisRecentes.ForEach(s => {
                                var copiaStatus = new FollowUp{
                                    OrigemID = AggId, // essa é a própria id do loop, da prosp que está sendo removida
                                    Anotacoes = s.Anotacoes,
                                    AnoFiscal = s.AnoFiscal,
                                    Status = s.Status,
                                    Data = s.Data
                                };
                                listaCopia.Add(copiaStatus);
                            });
                            prospAgg.Status.AddRange(listaCopia);
                        }
                    }
                }
            }
            prospeccao.Agregadas = "";
        }

        public static Usuario ObterUsuarioAtivo(ApplicationDbContext _context, HttpContext HttpContext)
        {
            return _context.Users.ToList().FirstOrDefault(usuario => usuario.UserName == HttpContext.User.Identity.Name);
        }

        /// <summary>
        /// Periodiza as prospecções de acordo com o ano no parâmetro
        /// </summary>
        /// <param name="ano">Ano que se deseja</param>
        /// <param name="lista">Lista de prospecções a serem periodizadas</param>
        /// <returns></returns>
        public static List<Prospeccao> PeriodizarProspecções(string ano, List<Prospeccao> lista)
        {
            if (ano.Equals("Todos") || string.IsNullOrEmpty(ano))
            {
                return lista;
            }

            return lista.Where(prospeccao => prospeccao.Status.Any(followup => followup.Data.Year == Convert.ToInt32(ano))).ToList();
        }

        /// <summary>
        /// Periodiza produções de acordo com o ano no parâmetro
        /// </summary>
        /// <param name="ano">Ano que se deseja</param>
        /// <param name="lista">Lista de produções a serem periodizadas</param>
        /// <returns></returns>
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
                    (inicial, prospeccao) =>
                    {
                        var contatoInicialStatus = prospeccao.Status.FirstOrDefault(s => s.Status == StatusProspeccao.ContatoInicial);
                        var comPropostaStatus = prospeccao.Status.FirstOrDefault(s => s.Status == StatusProspeccao.ComProposta);

                        if (contatoInicialStatus != null && comPropostaStatus != null)
                            return inicial + (comPropostaStatus.Data - contatoInicialStatus.Data);

                        return inicial;
                    }, diff => diff);

                intervaloDeTempo = new TimeSpan(intervaloDeTempo.Ticks / prospeccoes.Count());
            }
            else
            {
                intervaloDeTempo = new TimeSpan(0);
            }

            return intervaloDeTempo;
        }

        /// <summary>
        /// Ordena uma lista de prospecções de acordo com o parâmetro de ordenação
        /// </summary>
        /// <param name="sortOrder">Tipo de ordenação</param>
        /// <param name="lista">Lista de prospecções a serem ordenadas</param>
        /// <returns></returns>
        public static List<Prospeccao> OrdenarProspecções(string sortOrder, List<Prospeccao> lista)
        {
            var prospeccoes = lista.AsQueryable();
            prospeccoes = sortOrder switch
            {
                "name_desc" => prospeccoes.OrderByDescending(s => s.Empresa.Nome),
                "TipoContratacao" => prospeccoes.OrderBy(s => s.TipoContratacao),
                "tipo_desc" => prospeccoes.OrderByDescending(s => s.TipoContratacao),
                _ => prospeccoes.OrderBy(s => s.Status.OrderBy(k => k.Data).Last().Data),
            };
            return prospeccoes.ToList();
        }

        /// <summary>
        /// Filtra prospecções de acordo com um termo de busca
        /// </summary>
        /// <param name="searchString">Termo de busca (nome de empresa, nome de usuário, etc)</param>
        /// <param name="prospeccoes">Lista de prospecções a serem filtradas</param>
        /// <returns></returns>
        public static List<Prospeccao> FiltrarProspecções(string searchString, List<Prospeccao> prospeccoes)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                prospeccoes = prospeccoes.Where(p =>
                ChecarSubstring(searchString, p.Empresa.Nome) ||
                ChecarSubstring(searchString, p.Empresa.NomeFantasia) ||
                ChecarSubstring(searchString, p.Id) ||
                ChecarSubstring(searchString, p.Usuario.UserName) ||
                ChecarSubstring(searchString, p.NomeProspeccao) ||
                ChecarSubstring(searchString, p.MembrosEquipe)

                ).ToList();
            }

            return prospeccoes;
        }

        /// <summary>
        /// Verfica se um termo de busca está presente dentro de um campo específico
        /// </summary>
        /// <param name="searchString">Termo de busca</param>
        /// <param name="campo">Campo a ser verificado</param>
        /// <returns></returns>
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

        /// <summary>
        /// Filtra produções de acordo com um termo de busca
        /// </summary>
        /// <param name="searchString">Termo de busca</param>
        /// <param name="producoes">Lista de produções a serem filtradas</param>
        /// <returns></returns>
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

        /// <summary>
        /// Verifica se uma prospecção bate com o tipo de contratação passado por parâmetro
        /// </summary>
        /// <param name="prospeccao">Prospecção a ser verificada</param>
        /// <param name="tipoContratacao">Tipo de Contratação</param>
        /// <returns></returns>
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

        /// <summary>
        /// Verifica se uma prospecção possui um determinado status
        /// </summary>
        /// <param name="prospeccao">Prospecção a se verificar</param>
        /// <param name="status">Status específico</param>
        /// <returns></returns>
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