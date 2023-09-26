using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
	public interface IEquipe
	{
		[Key]
		public int Id { get; set; }

		public string IdTrabalho { get; set; }

		[ForeignKey("IdUsuario")]
		public Usuario Usuario { get; set; }

		public string IdUsuario { get; set; }
	}
}