using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Models
{
    public class Prospeccao
    {
        [Key]
        public string Id { get; set; }
        public Empresa Empresa { get; set; }
        public Pessoa Contato { get; set; }
        public Usuario Usuario { get; set; }
        [Display(Name ="Tipo de Contratação")]
        public TipoContratacao TipoContratacao { get; set; }
        [Display(Name ="Linha de Pesquisa")]
        public LinhaPesquisa LinhaPequisa { get; set; }
        public List<FollowUp> Status { get; set; } = new List<FollowUp>();
        public Casa Casa { get; set; }
    }

    public class FollowUp
    {

        [Key]
        public int Id { get; set; }
        [ForeignKey("OrigemID")]
        public Prospeccao Origem { get; set; }
        public string OrigemID { get; set; }
        public string Anotacoes { get; set; }
        public DateTime Data { get; set; }
        [Display(Name ="Ano da prospecção")]
        public int AnoFiscal
        {
            get
            { return this.Data.Year; }
            set { }
        }
       public StatusProspeccao Status { get; set; }
    }
}
