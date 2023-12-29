using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum ResultadoOpiniao
    {
        [Display(Name = "Muito Insatisfeito")]
        Pessimo,

        [Display(Name = "Insatisfeito")]
        Ruim,

        [Display(Name = "Regular")]
        Regular,

        [Display(Name = "Satisfeito")]
        Bom,

        [Display(Name = "Muito Satisfeito")]
        Otimo
    }
}