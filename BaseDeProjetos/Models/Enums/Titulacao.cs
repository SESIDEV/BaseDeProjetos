using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum Titulacao
    {
        [Display(Name = "Nível Médio")]
        Medio,

        [Display(Name = "Nível Médio Técnico")]
        Tecnico,

        [Display(Name = "Nível Superior - Não Concluído")]
        Graduando,

        [Display(Name = "Nível Superior - Concluído")]
        Graduado,

        [Display(Name = "Pós-Graduado - Lato Sensu")]
        Especialista,

        [Display(Name = "Pós-Graduado - Stricto Sensu (Mestrado)")]
        Mestre,

        [Display(Name = "Pós-Graduado - Stricto Sensu (Doutorado)")]
        Doutor,

        [Display(Name = "Pós-Graduado - Stricto Sensu (Pós-Doutor)")]
        PosDoutor
    }
}
