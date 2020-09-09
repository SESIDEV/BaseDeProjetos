using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
    public class Prospeccao
    {
        [Key]
        public virtual string Id { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Pessoa Contato { get; set; }
        public virtual Usuario Usuario { get; set; }
        [Display(Name = "Tipo de Contratação")]
        public virtual TipoContratacao TipoContratacao { get; set; }
        [Display(Name = "Linha de Pesquisa")]
        public virtual LinhaPesquisa LinhaPequisa { get; set; }
        public virtual List<FollowUp> Status { get; set; } = new List<FollowUp>();
        public virtual Casa Casa { get; set; }
    }

    public class FollowUp
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public virtual int Id { get; set; }
        [ForeignKey("OrigemID")]
        public virtual Prospeccao Origem { get; set; }
        public virtual string OrigemID { get; set; }
        public virtual string Anotacoes { get; set; }
        public virtual DateTime Data { get; set; }
        [Display(Name = "Ano da prospecção")]
        public virtual int AnoFiscal
        {
            get => Data.Year;
            set { }
        }
        public virtual StatusProspeccao Status { get; set; }
        public DateTime Vencimento { get; set; } = DateTime.Now.AddDays(14);
        public bool isTratado { get; set; } = false;
    }
}
