using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
	public class Cargo
	{
		[Key]
		public int Id { get; set; }
		public string Nome { get; set; }
		public decimal Salario { get; set; }
	}

}
