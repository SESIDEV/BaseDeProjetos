using System;

namespace BaseDeProjetos.Models
{
    public class CurvaFisicoFinanceira
    {
        public virtual int Id { get; set; }
        public virtual DateTime Data { get; set; } = DateTime.Now;
        public virtual decimal PercentualFisico { get; set; }
        public virtual decimal PercentualFinanceiro { get; set; }
    }
}