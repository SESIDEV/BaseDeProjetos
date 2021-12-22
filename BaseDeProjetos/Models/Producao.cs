using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{

    public class Producao
    {

        [Key]
        public int Id { get; set; }
        public TipoProducao Tipo { get; set; }
        public virtual DadosBasicos DadosBasicos { get; set; }
        public virtual Detalhamento Detalhamento { get; set; }
        public virtual SetoresDeAtividade SetorDeAtividade { get; set; }
        public string AreasDoConhecimento { get; set; }
        public string PalavrasChaves { get; set; }
        public string InformacoesAdicionais { get; set; }
        public virtual List<Usuario> Autores { get; set; }

    }

    public class DadosBasicos
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        [Display(Name = "Meio de Divulgação")]
        public MeioDivulgacao MeioDivulgacao { get; set; }
        public int Ano { get; set; }
        [Display(Name = "País")]
        public Pais Pais { get; set; }
        public Idioma Idioma { get; set; }
        [Display(Name = "O trabalho está entre os mais relevantes da produção?")]
        public bool FlagRelevancia { get; set; }
    }

    public class Detalhamento
    {
        [Key]
        public int Id { get; set; }
    }

    public class DadosBasicosArtigo : DadosBasicos
    {
    }
}
