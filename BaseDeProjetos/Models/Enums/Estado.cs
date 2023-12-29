using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum Estado
    {
        [Display(Name = "Rio de Janeiro")]
        RioDeJaneiro,

        [Display(Name = "São Paulo")]
        SaoPaulo,

        [Display(Name = "Minas Gerais")]
        MinasGerais,

        [Display(Name = "Espírito Santo")]
        EspiritoSanto,

        [Display(Name = "Paraná")]
        Parana,

        [Display(Name = "Santa Catarina")]
        SantaCatarina,

        [Display(Name = "Rio Grande do Sul")]
        RioGrandeDoSul,

        [Display(Name = "Mato Grosso")]
        MatoGrosso,

        [Display(Name = "Mato Grosso do Sul")]
        MatoGrossoDoSul,

        [Display(Name = "Goiás")]
        Goias,

        [Display(Name = "Distrito Federal")]
        DistritoFederal,

        Amazonas,

        [Display(Name = "Pará")]
        Para,

        Roraima,
        Acre,

        [Display(Name = "Rondônia")]
        Rondonia,

        [Display(Name = "Maranhão")]
        Maranhao,

        [Display(Name = "Piauí")]
        Piaui,

        [Display(Name = "Rio Grande do Norte")]
        RioGrandeDoNorte,

        Sergipe,
        Pernambuco,

        [Display(Name = "Paraíba")]
        Paraiba,

        [Display(Name = "Bahia")]
        Bahia,

        [Display(Name = "Tocantins")]
        Tocantins,

        [Display(Name = "Amapá")]
        Amapa,

        [Display(Name = "Ceará")]
        Ceara,

        [Display(Name = "Alagoas")]
        Alagoas,

        [Display(Name = "Fora do país")]
        Estrangeiro
    }
}