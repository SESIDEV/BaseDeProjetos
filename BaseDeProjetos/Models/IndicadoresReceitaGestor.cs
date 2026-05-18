using BaseDeProjetos.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class IndicadoresReceitaGestor
    {
        [Key]
        public int Id { get; set; }

        public Instituto Casa { get; set; }

        public int AnoBase { get; set; }

        public int Ordem { get; set; }

        [MaxLength(200)]
        public string Empresa { get; set; }

        [MaxLength(160)]
        public string Iniciativa { get; set; }

        public decimal? ValorTotal { get; set; }

        public decimal? ReceitaTotal { get; set; }

        public decimal? ReceitaAnoBase { get; set; }

        public decimal? ProjecaoAno1 { get; set; }

        public decimal? ProjecaoAno2 { get; set; }

        public decimal? ProjecaoAno3 { get; set; }

        public decimal? ProjecaoAno4 { get; set; }

        public decimal? ProjecaoAno5 { get; set; }

        [MaxLength(120)]
        public string ParceiroInterno { get; set; }
    }
}
