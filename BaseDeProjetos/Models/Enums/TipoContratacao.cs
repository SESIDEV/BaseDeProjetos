using System;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum TipoContratacao
    {
        [Display(Name = "Contratação direta")]
        ContratacaoDireta = 0,

        [Display(Name = "Embrapii")]
        Embrapii = 1,

        [Display(Name = "Edital SESI-SENAI")]
        EditalInovacao = 2,

        [Display(Name = "ANEEL")]
        AgenciaFomento = 3,

        [Display(Name = "ANP")]
        ANP = 4,

        [Display(Name = "Finep")]
        Parceiro = 5,

        [Display(Name = "Embrapii + ANP")]
        Push = 6,

        [Display(Name = "Embrapii + ANEEL")]
        EmbrapiiANEEL = 7,

        [Display(Name = "Edital - outros")]
        EditalOutros = 8,

        [Display(Name = "A definir")]
        Indefinida = 9,

    }
}