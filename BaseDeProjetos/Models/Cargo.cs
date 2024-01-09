using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Cargo
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual decimal Salario { get; set; }
        public virtual int HorasSemanais { get; set; }
        public virtual bool Tributos { get; set; }
    }
}