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
		public decimal? ValorManutenCaoAnoAnterior { get; set; }
		public decimal OcupacaoMax { get; set; }
		public decimal OcupacaoAtual { get; set; } = 0;
		public decimal PrecoBase { get; set; }
		public decimal? CustoHoraMaquina
		{
			//Cálculo hora máquina fictício (10% do preço base, divido por 12 meses, divido por 30 dias, divido por 24h)
			get => (PrecoBase * 0.10m) / 12 / 30 / 24;

			set => _ = PrecoBase;
		}
	}
}
