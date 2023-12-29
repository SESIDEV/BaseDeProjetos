using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum Instituto
    {
        [Display(Name = "ISI - Química Verde")]
        ISIQV,

        [Display(Name = "ISI - Inspeção & Integridade")]
        ISIII,

        [Display(Name = "CIS - Saúde Ocupacional")]
        CISHO,

        [Display(Name = "Supervisão")]
        Super,

        [Display(Name = "ISI - Sistemas Virtuais de Produção")]
        ISISVP
    }
}