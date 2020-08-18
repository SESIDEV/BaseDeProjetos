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
        public TipoContratacao TipoContratacao { get; set; }
        public LinhaPesquisa LinhaPequisa { get; set; }
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
        public int AnoFiscal
        {
            get
            { return this.Data.Year; }
            set { }
        }
       public StatusProspeccao Status { get; set; }
    }
}
