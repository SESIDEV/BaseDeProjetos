using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum TipoResposta
    {
        [Display(Name ="Objetiva Simples")]
        objetivaSimples,

        [Display(Name ="Objetiva Múltipla")]
        objetivaMultipla,

        [Display(Name ="Descritiva")]
        discursiva
    }
}
