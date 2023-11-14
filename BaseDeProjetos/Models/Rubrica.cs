using NUnit.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
    public class Rubrica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int ConjuntoRubricaId { get; set; }

        [ForeignKey("ConjuntoRubricaId")]
        public virtual ConjuntoRubrica ConjuntoRubrica {get;set;}
}
}
