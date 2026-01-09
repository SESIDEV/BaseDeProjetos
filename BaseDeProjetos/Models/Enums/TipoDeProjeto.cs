using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum TipoDeProjeto
    {
        [Display(Name = "PD&I")]
        PDeI = 1,

        [Display(Name = "Serviço tecnológico")]
        Servico_tecnologico = 2,

    }
}
