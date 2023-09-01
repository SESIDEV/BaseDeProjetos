using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class HMMOCK
    {
        [Key]
        public int Id {  get; set; }

        public string Nome { get; set; }

        public decimal OcupacaoMax {  get; set; }

        public decimal OcupacaoAtual { get; set; }

        public decimal PrecoBase { get; set; }

        //Relacionamento com projeto
        public virtual Projeto? Projeto { get; set; }
    }
}
