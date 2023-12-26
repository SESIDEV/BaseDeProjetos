// Classe Bean simples para armazenar a receita do período
using System;
using System.ComponentModel.DataAnnotations;
using BaseDeProjetos.Models.Enums;

namespace BaseDeProjetos.Models
{
    public class IndicadoresFinanceiros
    {
        [Key]
        public int Id { get; set; }

        public DateTime Data { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C2}")]
        [Display(Name = "Receita Por Competência")]
        public decimal Receita { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C2}")]
        public decimal Despesa { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:C2}")]
        [Display(Name = "Receita Por Caixa")]
        public decimal Investimento { get; set; }

        public float QualiSeguranca { get; set; }
        public Instituto Casa { get; set; }
    }
}