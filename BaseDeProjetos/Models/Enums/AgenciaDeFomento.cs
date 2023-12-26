using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum AgenciaDeFomento
    {
        [Display(Name = "Finep")]
        finep,

        [Display(Name = "Faperj")]
        faperj,

        [Display(Name = "Cnpq")]
        cnpq,

        [Display(Name = "Sesi/Senai de Inovação")]
        sesi_senai_inovacao,

        [Display(Name = "Outros")]
        outros
    }
}
