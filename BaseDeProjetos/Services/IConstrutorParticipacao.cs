using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseDeProjetos.Services
{
    public interface IConstrutorParticipacao
    {
        Task AtribuirAssertividadePrecificacao(Usuario usuario, DateTime dataInicio, DateTime dataFim, ParticipacaoTotalViewModel participacao, ProspeccoesUsuarioParticipacao prospeccoesUsuario);
        Task AtribuirParticipacoesIndividuais(ParticipacaoTotalViewModel participacao, List<Prospeccao> prospeccoesUsuario);
        void AtribuirQuantidadesDeProspeccao(Usuario usuario, ParticipacaoTotalViewModel participacao, ProspeccoesUsuarioParticipacao prospeccoesUsuario);
        void AtribuirValoresFinanceirosDeProspeccao(Usuario usuario, ParticipacaoTotalViewModel participacao, ProspeccoesUsuarioParticipacao prospeccoesUsuario);
    }
}
