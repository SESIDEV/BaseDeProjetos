using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum CasaFirjan
    {
        Firjan,

        [Display(Name = "Firjan SESI")]
        FirjanSESI,

        [Display(Name = "Firjan SENAI")]
        FirjanSENAI,
    }
}
