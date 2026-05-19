using BaseDeProjetos.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class IndicadoresPlanejamentoMensal
    {
        [Key]
        public int Id { get; set; }

        public Instituto Casa { get; set; }

        public int Ano { get; set; }

        [Required]
        [MaxLength(120)]
        public string Indicador { get; set; }

        public int Coluna { get; set; }

        public decimal Valor { get; set; }
    }
}
