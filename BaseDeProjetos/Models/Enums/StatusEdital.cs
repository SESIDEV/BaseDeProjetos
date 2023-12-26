using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum StatusEdital
    {
        [Display(Name = "Em aberto")]
        aberto,

        [Display(Name = "Edital Encerrado")]
        encerrado
    }
}
