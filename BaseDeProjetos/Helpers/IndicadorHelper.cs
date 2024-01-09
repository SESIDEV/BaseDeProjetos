using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseDeProjetos.Helpers
{
    public class IndicadorHelper
    {
        public IndicadorHelper(List<Prospeccao> listaDeProspeccoes)
        {
            ListaDeProspeccoes = listaDeProspeccoes;
        }

        public List<Prospeccao> ListaDeProspeccoes { get; set; }

        public static decimal DivisaoSegura(decimal numerador, decimal denominador)
        {
            if (denominador == 0)
            {
                return 0;
            }
            else
            {
                return numerador / denominador;
            }
        }

        public Dictionary<string, int> QuantidadeDeProspeccoes(Func<Prospeccao, object> propriedade, int? ano)
        {
            List<Prospeccao> listaProsps = ListaDeProspeccoes;

            if (ano != null)
            {
                listaProsps = ListaDeProspeccoes.Where(p => p.Status.Any(f => f.Data.Year == ano)).ToList();

                if (listaProsps == null)
                {
                    return new Dictionary<string, int>() { };
                }
            }

            // Verificar se a propriedade é do tipo Usuario
            if (propriedade.Invoke(listaProsps.FirstOrDefault()) is Usuario)
            {
                listaProsps = ListaDeProspeccoes.Where(pesquisador => pesquisador.Usuario.EmailConfirmed == true).ToList();
            }

            var listaEmGrupo = listaProsps.GroupBy(propriedade);

            if (listaEmGrupo == null)
            {
                return new Dictionary<string, int>() { };
            }

            Dictionary<string, int> quantidadeDeProspeccoesPorCasa = new Dictionary<string, int>();

            foreach (var p in listaEmGrupo)
            {
                if (quantidadeDeProspeccoesPorCasa.ContainsKey(p.Key.ToString()))
                {
                    if (p.Count().GetType() == typeof(int))
                    {
                        quantidadeDeProspeccoesPorCasa[p.Key.ToString()] += p.Count();
                    }
                    else
                    {
                        quantidadeDeProspeccoesPorCasa[p.Key.ToString()] += 0;
                    }
                }
                else
                {
                    if (p.Count().GetType() == typeof(int))
                    {
                        quantidadeDeProspeccoesPorCasa.Add(p.Key.ToString(), p.Count());
                    }
                    else
                    {
                        quantidadeDeProspeccoesPorCasa.Add(p.Key.ToString(), 0);
                    }
                }
            }

            return quantidadeDeProspeccoesPorCasa.OrderByDescending(p => p.Value).Take(10).ToDictionary(k => k.Key, v => v.Value);
        }

        public Dictionary<string, string> ValorSomaProspeccoes(Func<Prospeccao, object> propriedade, int? ano)
        {
            List<Prospeccao> listaProsps = ListaDeProspeccoes;

            if (ano != null)
            {
                listaProsps = ListaDeProspeccoes.Where(p => p.Status.Any(f => f.Data.Year == ano)).ToList();

                if (listaProsps == null)
                {
                    return new Dictionary<string, string>() { };
                }
            }
            if (propriedade == null)
            {
                return new Dictionary<string, string>() { };
            }

            // Verificar se a propriedade é do tipo Usuario
            if (propriedade.Invoke(listaProsps.FirstOrDefault()) is Usuario)
            {
                listaProsps = ListaDeProspeccoes.Where(pesquisador => pesquisador.Usuario.EmailConfirmed == true).ToList();
            }

            var listaEmGrupo = listaProsps.GroupBy(propriedade);

            if (listaEmGrupo == null)
            {
                return new Dictionary<string, string>() { };
            }

            Dictionary<string, decimal> valorDeProspeccoesPorCasa = new Dictionary<string, decimal>();

            foreach (var p in listaEmGrupo)
            {
                if (valorDeProspeccoesPorCasa.ContainsKey(p.Key.ToString()))
                {
                    if (p.Count().GetType() == typeof(int))
                    {
                        valorDeProspeccoesPorCasa[p.Key.ToString()] += p.Sum(v => v.ValorProposta) + p.Sum(v => v.ValorEstimado);
                    }
                    else
                    {
                        valorDeProspeccoesPorCasa[p.Key.ToString()] += (decimal)0.0;
                    }
                }
                else
                {
                    if (p.Count().GetType() == typeof(int))
                    {
                        valorDeProspeccoesPorCasa.Add(p.Key.ToString(), p.Sum(v => v.ValorProposta) + p.Sum(v => v.ValorEstimado));
                    }
                    else
                    {
                        valorDeProspeccoesPorCasa.Add(p.Key.ToString(), (decimal)0.0);
                    }
                }
            }

            return valorDeProspeccoesPorCasa.OrderByDescending(o => o.Value).Take(10).ToDictionary(e => e.Key, v =>
            {
                return Helpers.FormatarValoresDashboards(v.Value);
            });
        }

        public Dictionary<string, decimal> CalcularTaxaDeConversao(Func<Prospeccao, object> propriedade, int? ano)
        {
            List<Prospeccao> listaProsp = ListaDeProspeccoes;

            Dictionary<string, decimal> taxaDeConversaoPorPesquisador = new Dictionary<string, decimal>();

            if (ano != null)
            {
                listaProsp = ListaDeProspeccoes.Where(p => p.Status.Any(f => f.Data.Year == ano)).ToList();
            }

            if (propriedade.Invoke(ListaDeProspeccoes.FirstOrDefault()) is Usuario)
            {
                listaProsp = ListaDeProspeccoes.Where(pesquisador => pesquisador.Usuario.EmailConfirmed == true).ToList();
            }

            var listaEmGrupo = listaProsp.GroupBy(propriedade);

            foreach (var p in listaEmGrupo)
            {
                if (p.Count().GetType() == typeof(int))
                {
                    int convertidas = p.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.Convertida).Count();

                    int naoConvertidas = p.Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Status == StatusProspeccao.NaoConvertida).Count();

                    if (convertidas > 0 && naoConvertidas > 0)
                    {
                        decimal taxa = (100 * convertidas) / (convertidas + naoConvertidas);

                        if (taxaDeConversaoPorPesquisador.ContainsKey(p.Key.ToString()))
                        {
                            taxaDeConversaoPorPesquisador[p.Key.ToString()] += taxa;
                        }
                        else
                        {
                            taxaDeConversaoPorPesquisador.Add(p.Key.ToString(), taxa);
                        }
                    }
                }
            }

            return taxaDeConversaoPorPesquisador.OrderByDescending(v => v.Value).ToDictionary(p => p.Key, k => k.Value);
        }
    }
}