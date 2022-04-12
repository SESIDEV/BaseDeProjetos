using System;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class RegistroEPI
    {
        [Key]
        public int Id { get; set; }

        public virtual DateTime DataEntrega { get; set; }

        [Display(Name = "Unidade Operacional")]
        public Instituto UnidadeOperacional { get; set; }

        public String UsuarioId { get; set; }

        [Display(Name = "Nome do Usuário")]
        public virtual Usuario Usuario { get; set; }

        [Display(Name = "Motivo da Troca do EPI")]
        public JustificaEPI Justificativa { get; set; }
    }

    public class ModeloEPI
    {
        [Key]
        public uint IdEPI
        {
            get; set;
        }

        public string Descricao;
        public string Marca;
    }

    public class EpiConcreto
    {
        [Key]
        public uint IdEPIEntregavel;

        public string IdRegistro;
        public RegistroEPI DocumentoEPI;

        public uint IdEPI;
        public ModeloEPI TipoEPI;

        public uint NumeroCA;

        public Boolean isAtivo;

        public DateTime DataDeVencimento;
    }
}