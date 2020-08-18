using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Pessoa
    {
        [Key]
        public int id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public Empresa empresa { get; set; }
    }
}