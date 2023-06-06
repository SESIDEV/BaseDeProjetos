using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseDeProjetos.Helpers
{
    public class IndicadorHelper
    {
        private readonly ApplicationDbContext _dbContext;

        public IndicadorHelper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Prospeccao> ListaDeProspeccoes { get => _dbContext.Prospeccao.Where(p => p.Status.FirstOrDefault().Status != StatusProspeccao.Planejada).ToList(); }

        private List<string> TratarListaPotenciaisParceiros()
        {
            List<string> potenciaisParceiros = new List<string>();

            foreach (string item in ListaDeProspeccoes.Where(p => p.PotenciaisParceiros != null).Select(p => p.PotenciaisParceiros).ToList())
            {
                if (item != null)
                {
                    foreach (string elemento in item.Split(","))
                    {
                        potenciaisParceiros.Add(elemento.ToLower());
                    };
                }
            }

            return potenciaisParceiros;
        }
        public Dictionary<string, int> QuantidadeDeProspeccaoPorPotencialParceiro()
        {
            List<string> potenciaisParceiros = TratarListaPotenciaisParceiros();

            List<Prospeccao> prospeccoes = ListaDeProspeccoes.Where(p => p.PotenciaisParceiros != null).ToList();

            Dictionary<string, int> dict = new Dictionary<string, int>
            {
                {"Parceiro", 1}
            };


            return dict;

        }
        public decimal ValorSomaProspeccaoPorPotenciaisParceiros(List<Prospeccao> lista)
        {
            decimal valorSoma = 0;

            lista.GroupBy(v => v.ValorProposta);

            foreach (Prospeccao prospeccao in lista)
            {
                valorSoma += prospeccao.ValorProposta;
            }

            return valorSoma;
        }

        public Dictionary<string, int> QuantidadeProspeccaoPorEstado(Func<Prospeccao, object> propriedade)
        {
            var listaProsps = ListaDeProspeccoes;

            var listGroup = listaProsps.GroupBy(propriedade).ToList();

            Dictionary<string, int> dict = new Dictionary<string, int>();

            foreach (var prospeccao in listGroup)
            {
                if (prospeccao.Count() > 0)
                {
                    dict.Add(prospeccao.Key.ToString(), prospeccao.Count());
                }
                else
                {
                    dict.Add(prospeccao.Key.ToString(), 0);
                }
            }

            return dict.OrderByDescending(d => d.Value).ToDictionary(k => k.Key, v => v.Value);
        }
        public Dictionary<string, int> QuantidadeDeProspeccoes(Func<Prospeccao, object> propriedade)
        {
            List<Prospeccao> listaProsps = ListaDeProspeccoes;

            var listaEmGrupo = listaProsps.GroupBy(propriedade);

            Dictionary<string, int> quantidadeDeProspeccoesPorCasa = new Dictionary<string, int>();


            foreach (var p in listaEmGrupo)
            {

                if (!quantidadeDeProspeccoesPorCasa.ContainsKey(p.Key.ToString()))
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

            return quantidadeDeProspeccoesPorCasa.OrderByDescending(p => p.Value).ToDictionary(k => k.Key, v => v.Value);
        }
        public Dictionary<string, string> ValorSomaProspeccoes(Func<Prospeccao, object> propriedade)
        {
            var listaProsps = ListaDeProspeccoes;

            var listaEmGrupo = listaProsps.GroupBy(propriedade);

            Dictionary<string, decimal> valorDeProspeccoesPorCasa = new Dictionary<string, decimal>();

            foreach (var p in listaEmGrupo)
            {
                if (!valorDeProspeccoesPorCasa.ContainsKey(p.Key.ToString()))
                {
                    if (p.Count().GetType() == typeof(int))
                    {
                        valorDeProspeccoesPorCasa.Add(p.Key.ToString(), (decimal)p.Sum(v => v.ValorProposta));
                    }
                    else
                    {
                        valorDeProspeccoesPorCasa.Add(p.Key.ToString(), (decimal)0.0);
                    }

                }

            }

            return valorDeProspeccoesPorCasa.OrderByDescending(o => o.Value).ToDictionary(e => e.Key, v =>
            {
                return Helpers.FormatarValoresDashboards(v.Value);
            });
        }
    }
}

