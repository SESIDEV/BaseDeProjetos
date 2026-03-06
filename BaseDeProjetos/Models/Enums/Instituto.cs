using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum Instituto
    {
        [Display(Name = "ISI - Química Verde")]
        ISIQV,

        [Display(Name = "CIS - Saúde Ocupacional")]
        CISHO,

        [Display(Name = "ISIBF - [CBT] Biossintéticos e Fibras")]
        ISIBF_CBT,

        [Display(Name = "ISIBF - [CIN] Biossintéticos e Fibras")]
        ISIBF_CIN,

        [Display(Name = "ISIBF - [CPQ] Biossintéticos e Fibras")]
        ISIBF_CPQ,

        [Display(Name = "ISIBF - [CEP] Biossintéticos e Fibras")]
        ISIBF_CEP,

        [Display(Name = "ISIBF - [CSQ] Biossintéticos e Fibras")]
        ISIBF_CSQ,

        [Display(Name = "GPD - Serviços")] // Gerência de Pesquisa e Desenvolvimento
        Serviços_GPD,

        [Display(Name = "Supervisão")]
        Super,

        [Display(Name = "ISI - Sistemas Virtuais de Produção")]
        ISISVP,

        [Display(Name = "ISI - Inspeção & Integridade")]
        ISIII
    }
}