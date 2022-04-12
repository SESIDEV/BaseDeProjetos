using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Pessoa
    {
        [Key]
        public virtual int id { get; set; }

        [Display(Name = "Nome da pessoa")]
        public virtual string Nome { get; set; }

        [Display(Name = "Email da pessoa")]
        public virtual string Email { get; set; }

        [Display(Name = "Telefone da pessoa")]
        public virtual string Telefone { get; set; }

        public virtual Empresa empresa { get; set; }
    }
}