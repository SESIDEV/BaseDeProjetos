using System;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{

    //Interface para artigos, palestras, notícias, eventos
    public  class Producao
    {

        [Key]
        public int Id { get; set; }
        [Display(Name = "Tipo de Produção")]
        public TipoProducao Tipo { get; set; }
        public Casa Casa { get; set; }
        [Display(Name = "Data de Realização")]
        public DateTime DataDaRealizacao { get; set; }
        [Display(Name = "Local onde ocorreu o(a) Evento/Palestra/Submissão/Publicação")]
        public string RealizadoEm { get; set; }
        [Display(Name ="Descrição da Produção")]
        public string Descricao { get; set; }
        public string Link { get; set; }
        [Display(Name ="Público alcançado (pessoas)")]
        public int? publico { get; set; }
    }
}
