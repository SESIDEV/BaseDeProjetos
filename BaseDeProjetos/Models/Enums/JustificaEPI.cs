using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum JustificaEPI
    {
        [Display(Name = "Primeira Entrega")]
        PrimeiraEntrega,

        [Display(Name = "Substituição - Troca de rotina ou compulsória, em função da periodicidade")]
        Subs_TrocaCompulsoria,

        [Display(Name = "Necessidade de higienização e/ou manutenção periódica")]
        Subs_HigienizacaoManutencao,

        [Display(Name = "Danificado (sem condições de uso) ou extraviado")]
        Subs_DanificadoExtraviado
    }
}