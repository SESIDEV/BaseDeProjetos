using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        [Display(Name ="Nome da Empresa")]
        public string Nome { get; set; }
        [Display(Name ="CNPJ da Empresa")]
        public string CNPJ { get; set; }
        [Display(Name ="Segmento da Empresa")]
        public string Segmento { get; set; }
        [Display(Name ="Estado da Empresa")]
        public string Estado { get; set; }
    }
}