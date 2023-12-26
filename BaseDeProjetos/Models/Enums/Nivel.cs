using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum Nivel
    {
        [Display(Name = "Usuário")]
        Usuario,

        [Display(Name = "Supervisor")]
        Supervisor,

        [Display(Name = "PMO")]
        PMO,

        [Display(Name = "Desenvolvedor")]
        Dev,

        [Display(Name = "Externos")]
        Externos
    }
}
