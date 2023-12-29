using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum TipoContratacao
    {
        [Display(Name = "Contratação Direta")]
        ContratacaoDireta,

        Embrapii,

        [Display(Name = "Edital de Inovação SESI/SENAI")]
        EditalInovacao,

        //OutrosEditais,
        [Display(Name = "Agência de Fomento")]
        AgenciaFomento,

        [Display(Name = "ANP/ANEEL")]
        ANP,

        Parceiro,

        [Display(Name = "Projeto Push")]
        Push,

        [Display(Name = "A definir")]
        Indefinida
    }
}