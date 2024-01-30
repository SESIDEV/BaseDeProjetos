using BaseDeProjetos.Models;
using BaseDeProjetos.Models.Helpers;
using BaseDeProjetos.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseDeProjetos.Services
{
    public interface IAdministradorDadosParticipacao
    {
        Task<List<ParticipacaoTotalViewModel>> GetParticipacoesTotaisUsuarios(Usuario usuarioAtivo, DateTime dataInicio, DateTime dataFim);
        Task<List<Prospeccao>> ObterProspeccoesParaParticipacao();
        ProspeccoesUsuarioParticipacao ObterProspeccoesUsuario(Usuario usuario);
    }
}
