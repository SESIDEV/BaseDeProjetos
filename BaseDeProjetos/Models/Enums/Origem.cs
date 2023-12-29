using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum Origem
    {
        [Display(Name = "Recebida")]
        Recebida,

        [Display(Name = "Iniciada")]
        Iniciada
    }
}