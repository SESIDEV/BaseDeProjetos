using System;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{

    //Interface para artigos, palestras, notícias, eventos
    public  class Producao
    {

        [Key]
        public int Id { get; set; }
        public TipoProducao Tipo { get; set; }
        public DateTime DataDaRealizacao { get; set; }
        public string RealizadoEm { get; set; }
        public string Descricao { get; set; }
        public string Link { get; set; }
        public int? publico { get; set; }
    }
}
