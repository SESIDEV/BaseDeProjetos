using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum StatusPub
    {
        [Display(Name = "Submetido", GroupName = "Artigo")]
        Submetido,

        [Display(Name = "Aceito", GroupName = "Artigo")]
        Aceito,

        [Display(Name = "Publicado", GroupName = "Artigo")]
        Publicado,

        [Display(Name = "Busca de Anterioridade", GroupName = "Patente")]
        BuscaAnterioridade,

        [Display(Name = "Em Depósito", GroupName = "Patente")]
        EmDeposito,

        [Display(Name = "Depositada", GroupName = "Patente")]
        Depositada,

        [Display(Name = "Concedida", GroupName = "Patente")]
        Concedida = 5,
    }
}
