using System;

namespace BaseDeProjetos.Models
{

    //Interface para artigos, palestras, notícias, eventos
    public interface IProducao
    {
        public string RealizadoEm { get; set; }
        public DateTime DataDaRealizacao { get; set; }
    }
    public class Artigo : IProducao
    {
        public string RealizadoEm { get; set; }
        public DateTime DataDaRealizacao { get; set; }
    }

    public class Noticia : IProducao
    {
        public string RealizadoEm { get; set; }
        public DateTime DataDaRealizacao { get; set; }

    }

    public class Evento : IProducao
    {
        public string RealizadoEm { get; set; }
        public DateTime DataDaRealizacao { get; set; }

    }

    public class Palestra : IProducao
    {
        public string RealizadoEm { get; set; }
        public DateTime DataDaRealizacao { get; set; }

    }
}
