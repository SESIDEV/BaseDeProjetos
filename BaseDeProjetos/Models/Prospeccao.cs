using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BaseDeProjetos.Models
{
    public class Prospeccao : IEquatable<Prospeccao>
    {
        [Key]
        public virtual string Id { get; set; }

        [Display(Name = "Nome da prospecção ou potencial projeto")]
        public virtual string NomeProspeccao { get; set; }

        [Display(Name = "Potenciais Parceiros da Prospeccção")]
        public virtual string PotenciaisParceiros { get; set; } //Por ora é uma string separada por vírgulas

        public virtual Empresa Empresa { get; set; }
        public virtual Pessoa Contato { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Display(Name = "Tipo de Contratação")]
        public virtual TipoContratacao TipoContratacao { get; set; }

        [Display(Name = "Linha de Pesquisa")]
        public virtual LinhaPesquisa LinhaPequisa { get; set; }

        public virtual List<FollowUp> Status { get; set; } = new List<FollowUp>();
        public virtual Instituto Casa { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "R{0:C2}")]
        [Display(Name = "Valor da Proposta (R$)")]
        public virtual decimal ValorProposta { get; set; } = 0;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "R{0:C2}")]
        [Display(Name = "Valor Estimado da Prospecção (R$)")]
        public virtual decimal ValorEstimado { get; set; } = 0;

        public bool Equals([AllowNull] Prospeccao other)
        {
            if (other is null) return false;
            return other.Id == Id;
        }

        public override int GetHashCode() => (Id).GetHashCode();
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
        public virtual MotivosNaoConversao MotivoNaoConversao { get; set; }
        public DateTime Vencimento { get; set; } = DateTime.Now.AddDays(14);
        public bool isTratado { get; set; } = false;
    }
}