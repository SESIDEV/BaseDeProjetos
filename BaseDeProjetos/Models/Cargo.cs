using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
	public class Cargo
	{

		public int Id { get; set; }
		private string _nome { get; set; } = "Não informado";
		private decimal _salario { get; set; }
		public string Nome
		{
			get
			{
				return _nome;
			}
			set
			{
				if (value == "Não informado" || value == null)
				{
					_salario = 0;
				}
				else
				{
					_nome = value;
				}
			}
		}
		public decimal Salario { get => _salario; set => _salario = value; }

	}

}
