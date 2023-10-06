using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class CodigoAmostraProjeto
    {
        public CodigoAmostraProjeto()
        {
        }

        [Key]
        public virtual int Id { get; set; }

        [Display(Name = "Projeto")]
        public virtual Projeto Projeto { get; set; }

        [Display(Name = "Código")]
        public virtual string Codigo { get; set; }
    }
}