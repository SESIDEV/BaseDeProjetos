using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum StatusProspeccao
    {
        [Display(Name = "Contato inicial")]
        ContatoInicial = 0,

        [Display(Name = "Em discussão")]
        Discussao_EsbocoProjeto = 4,

        [Display(Name = "Com Proposta")]
        ComProposta = 5,

        [Display(Name = "Convertida")]
        Convertida = 6,

        [Display(Name = "Não Convertida")]
        NaoConvertida = 7,

        [Display(Name = "Cancelado/Suspenso")]
        Suspensa = 8,

        [Display(Name = "Planejada")]
        Planejada = 9,

        [Display(Name = "Agregada")]
        Agregada = 10
    }
}
