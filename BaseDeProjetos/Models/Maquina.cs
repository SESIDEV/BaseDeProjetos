using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
	public class Maquina
	{
		[Key]
		public int Id { get; set; }
		[Display(Name = "Nome da Máquina")]
		public string Nome { get; set; }
		[Display(Name = "Identificação da Máquina pode ser número de patrimonio ou algum outro que seja único")]
		public string? IdentificacaoDaMaquina { get; set; }
		[Display(Name = "Informar se houve manutenção ")]
		public bool ManutencaoAnoAnterior { get; set; }
		[Display(Name = "Valor da Manutenção")]
		public decimal? ValorManutenCaoAnoAnterior { get; set; }
		[Display(Name = "Informar tempo máximo em horas que a máquina fica disponível")]
		public decimal OcupacaoMax { get; set; }
		[Display(Name = "Informar tempo em horas que a máquina será usada no projeto")]
		public decimal OcupacaoAtual { get; set; } = 0;
		[Display(Name = "Valor da máquina")]
		public decimal PrecoBase { get; set; }
		[Display(Name = "Valor hora máquina")]
		public decimal CustoHoraMaquina
		{
			//Cálculo hora máquina fictício (10% do preço base, divido por 12 meses, divido por 30 dias, divido por 24h)
			get
			{ 
				return PrecoBase * 0.10m / 12 / 30 / 24; 
			}

		}

		//Relacionamento Projeto
		public virtual Projeto? Projeto { get; set; }

	}
}
