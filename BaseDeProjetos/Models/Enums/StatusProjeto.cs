using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum StatusProjeto
    {
        [Display(Name = "Contratado/Em planejamento")]
        Contratado,

        [Display(Name = "Em execução")]
        EmExecucao,

        [Display(Name = "Concluído")]
        Concluido,

        [Display(Name = "Cancelado/Suspenso")]
        Cancelado,

        [Display(Name = "Atrasado")]
        Atrasado,
    }
}
