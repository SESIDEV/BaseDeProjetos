using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseDeProjetos.Helpers
{
    public class ParticipacaoHelper
    {
        public static List<Usuario> TratarMembrosEquipeString(Prospeccao prospeccao, ApplicationDbContext _context)
        {
            List<string> membrosNaoTratados = prospeccao.MembrosEquipe?.Split(";").ToList();
            List<UsuarioParticipacaoDTO> usuarios = _context.Users
                .Select(u => new UsuarioParticipacaoDTO
                {
                    Cargo = new CargoDTO { Nome = u.Cargo.Nome, Id = u.Cargo.Id },
                    Casa = u.Casa,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    Nivel = u.Nivel,
                    Id = u.Id,
                    UserName = u.UserName
                })
                .ToList();

            List<Usuario> membrosEquipe = new List<Usuario>();

            if (membrosNaoTratados != null)
            {
                foreach (var membro in membrosNaoTratados)
                {
                    if (!string.IsNullOrEmpty(membro))
                    {
                        Usuario usuarioEquivalente = usuarios.Find(u => u.Email == membro).ToUsuario();
                        if (usuarioEquivalente != null)
                        {
                            membrosEquipe.Add(usuarioEquivalente);
                        }
                    }
                }
            }

            return membrosEquipe;
        }

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
    }
}
