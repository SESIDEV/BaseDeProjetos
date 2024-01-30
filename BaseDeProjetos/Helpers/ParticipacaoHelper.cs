using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Enums;
using BaseDeProjetos.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseDeProjetos.Helpers
{
    public class ParticipacaoHelper
    {
        /// <summary>
        /// Filtra as prospecções passadas por parâmetro com status do mês e ano inicial até o mês e ano final
        /// </summary>
        /// <returns></returns>
        public static List<Prospeccao> FiltrarProspeccoesPorPeriodo(DateTime dataInicio, DateTime dataFim, List<Prospeccao> prospeccoes)
        {
            return prospeccoes
                .Where(p => p.Status.OrderBy(f => f.Data).LastOrDefault().Data >= dataInicio && p.Status.OrderBy(f => f.Data).LastOrDefault().Data <= dataFim)
                .ToList();
        }

        /// <summary>
        /// Filtrar os projetos passados por parâmetro até a data de encerramento
        /// </summary>
        /// <param name="mesFim"></param>
        /// <param name="anoFim"></param>
        /// <param name="projetosUsuario"></param>
        /// <returns></returns>
        private List<Projeto> FiltrarProjetosPorPeriodo(string mesFim, string anoFim, List<Projeto> projetos)
        {
            int yearFim = int.Parse(anoFim);
            int monthFim = int.Parse(mesFim);

            DateTime dataFiltro = new DateTime(yearFim, monthFim, DateTime.DaysInMonth(yearFim, monthFim));

            return projetos
                .Where(p => dataFiltro <= p.DataEncerramento)
                .ToList();
        }

        /// <summary>
        /// Filtra as prospecções que possuam status "Com Proposta"
        /// Convertidas também se incluem de acordo com a logica
        /// </summary>
        /// <param name="prospeccoes"></param>
        /// <returns></returns>
        public static List<Prospeccao> FiltrarProspeccoesComProposta(List<Prospeccao> prospeccoes)
        {
            return prospeccoes.Where(p => p.Status.Any(f => f.Status == StatusProspeccao.Convertida || p.Status.Any(f => f.Status == StatusProspeccao.ComProposta))).ToList();
        }

        /// <summary>
        /// Filtra a lista de prospecções para obter as convertidas
        /// </summary>
        /// <param name="prospeccoes"></param>
        /// <returns></returns>
        public static List<Prospeccao> FiltrarProspeccoesConvertidas(List<Prospeccao> prospeccoes)
        {
            return prospeccoes.Where(p => p.Status.Any(f => f.Status == StatusProspeccao.Convertida)).ToList();
        }

        /// <summary>
        /// Filtra as prospecções do usuario de acordo com a data de inicio e data de fim do filtro
        /// </summary>
        /// <param name="prospeccoes"></param>
        /// <param name="dataInicio"></param>
        /// <param name="dataFim"></param>
        public static void PeriodizarProspeccoesUsuario(ProspeccoesUsuarioParticipacao prospeccoes, DateTime dataInicio, DateTime dataFim)
        {
            prospeccoes.ProspeccoesMembro = FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesMembro);
            prospeccoes.ProspeccoesLider = FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesLider);
            prospeccoes.ProspeccoesTotais = FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesTotais);
            prospeccoes.ProspeccoesTotaisComProposta = FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesTotaisComProposta);
            prospeccoes.ProspeccoesTotaisConvertidas = FiltrarProspeccoesPorPeriodo(dataInicio, dataFim, prospeccoes.ProspeccoesTotaisConvertidas);
        }
    }
}
