using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
    public class EquipeProjeto : IEquipe
    {
        public int Id { get; set; }
        public string IdTrabalho { get; set; }

        [ForeignKey("IdTrabalho")]
        public virtual Projeto Projeto { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        public string IdUsuario { get; set; }
    }
}