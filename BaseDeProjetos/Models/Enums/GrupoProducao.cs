using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum GrupoProducao
    {
        [Display(Name = "Trabalho em Eventos", GroupName = "Produção Bibliográfica")]
        Trabalho,

        [Display(Name = "Artigo Publicado", GroupName = "Produção Bibliográfica")]
        Artigo,

        [Display(Name = "Livro Publicado ou Organizado", GroupName = "Produção Bibliográfica")]
        Livro,

        [Display(Name = "Capitulo de Livro Publicado", GroupName = "Produção Bibliográfica")]
        Capitulo,

        [Display(Name = "Texto em Jornal ou Revista", GroupName = "Produção Bibliográfica")]
        JornalRevista,

        [Display(Name = "Prefácio/Posfácio", GroupName = "Produção Bibliográfica")]
        PrefacioPosfacio,

        [Display(Name = "Palestras em Eventos", GroupName = "Produção Bibliográfica")]
        Palestra,

        [Display(Name = "Relatório Técnico", GroupName = "Produção Técnica, Patentes e Registros")]
        Relatorio,

        [Display(Name = "Patente", GroupName = "Produção Técnica, Patentes e Registros")]
        Patente = 8, //precisa ser 8 por causa do JS

        [Display(Name = "Participação em Eventos/Palestras", GroupName = "Outros")]
        Participacao,

        [Display(Name = "Visita Técnica", GroupName = "Outros")]
        Visita,

        [Display(Name = "Cooperação", GroupName = "Outros")]
        Cooperacao,

        [Display(Name = "Prêmio", GroupName = "Outros")]
        Premio,
    }
}
