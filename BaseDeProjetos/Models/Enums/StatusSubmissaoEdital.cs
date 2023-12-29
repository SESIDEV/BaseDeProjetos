using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum StatusSubmissaoEdital
    {
        [Display(Name = "Submissão de edital")]
        submetido,

        [Display(Name = "Submissão em análise")]
        emAnalise,

        [Display(Name = "Deferido")]
        deferido,

        [Display(Name = "Indeferido")]
        indeferido,

        [Display(Name = "Cancelado")]
        cancelado
    }
}