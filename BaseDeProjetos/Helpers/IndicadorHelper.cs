using BaseDeProjetos.Controllers;
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


        public Dictionary<string, int> QuantidadeDeProspeccoes(Func<Prospeccao, object> propriedade)
        {
            List<Prospeccao> listaProsps = ListaDeProspeccoes;

			// Verificar se a propriedade é do tipo Usuario
			if (propriedade.Invoke(listaProsps.FirstOrDefault()) is Usuario)
			{
				listaProsps = listaProsps.Where(pesquisador => pesquisador.Usuario.EmailConfirmed == true).ToList();
			}

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

            return quantidadeDeProspeccoesPorCasa.OrderByDescending(p => p.Value).Take(10).ToDictionary(k => k.Key, v => v.Value);
        }
        public Dictionary<string, string> ValorSomaProspeccoes(Func<Prospeccao, object> propriedade)
        {
            var listaProsps = ListaDeProspeccoes;

			// Verificar se a propriedade é do tipo Usuario
			if (propriedade.Invoke(listaProsps.FirstOrDefault()) is Usuario)
			{
				listaProsps = listaProsps.Where(pesquisador => pesquisador.Usuario.EmailConfirmed == true).ToList();
			}

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

            return valorDeProspeccoesPorCasa.OrderByDescending(o => o.Value).Take(10).ToDictionary(e => e.Key, v =>
            {
                return Helpers.FormatarValoresDashboards(v.Value);
            });
        }
    }
}

