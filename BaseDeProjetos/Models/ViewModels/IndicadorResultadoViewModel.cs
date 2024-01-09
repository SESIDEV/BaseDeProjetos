using BaseDeProjetos.Models.DTOs;

namespace BaseDeProjetos.Models.ViewModels
{
    public class IndicadorResultadoViewModel
    {
        public UsuarioParticipacaoDTO Pesquisador { get; set; }

        public object Valor { get; set; }

        public decimal Rank { get; set; }
    }
}