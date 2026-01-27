using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum TipoDeInteracao
    {
        [Display(Name = "Visita à empresa")]
        VisitaEmpresa = 1,

        [Display(Name = "Atendimento na unidade")]
        AtendimentoUnidade = 2,

        [Display(Name = "Telefone ou teleconferência")]
        TelefoneOuTeleconferencia = 3,

        [Display(Name = "Reunião em evento de prospecção")]
        ReuniaoEventoProspeccao = 4,

        [Display(Name = "Outro (detalhar nas observações)")]
        Outro = 5,

        [Display(Name = "A definir")]
        Adefinir = 6
    }
}
