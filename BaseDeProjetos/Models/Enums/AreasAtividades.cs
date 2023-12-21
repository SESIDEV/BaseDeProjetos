using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum AreasAtividades
    {
        [Display(Name = "Atividades de apoio")]
        Apoio,

        [Display(Name = "Atividades básicas")]
        Basicas,

        [Display(Name = "Atividades de execução")]
        Execucao,

        [Display(Name = "Atividades de prospecção")]
        Prospeccao,

        Outros
    }
}
