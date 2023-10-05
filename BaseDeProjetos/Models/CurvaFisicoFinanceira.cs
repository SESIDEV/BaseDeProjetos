using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
    public class CurvaFisicoFinanceira
    {
        public virtual int Id { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual decimal PercentualFisico { get; set; }
        public virtual decimal PercentualFinanceiro { get; set; }
    }
}