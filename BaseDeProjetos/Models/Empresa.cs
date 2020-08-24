using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Empresa
    {
        public virtual int Id { get; set; }
        [Display(Name ="Nome da Empresa")]
        public virtual string Nome { get; set; }
        [Display(Name ="CNPJ da Empresa")]
        public virtual string CNPJ { get; set; }
        [Display(Name ="Segmento da Empresa")]
        public virtual string Segmento { get; set; }
        [Display(Name ="Estado da Empresa")]
        public virtual string Estado { get; set; }
    }
}