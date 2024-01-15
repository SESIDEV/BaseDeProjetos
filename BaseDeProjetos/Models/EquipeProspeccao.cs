using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
    public class EquipeProspeccao : IEquipe
    {
        public int Id { get; set; }
        public string IdTrabalho { get; set; }

        [ForeignKey("IdTrabalho")]
        public virtual Prospeccao Projeto { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
        public string IdUsuario { get; set; }
    }
}
