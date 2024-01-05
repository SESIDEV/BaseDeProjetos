namespace BaseDeProjetos.Models
{
    public class Rubrica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int ConjuntoRubricaId { get; set; }
        public virtual ConjuntoRubrica ConjuntoRubrica { get; set; }
    }
}