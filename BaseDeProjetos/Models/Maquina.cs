using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Maquina
    {
        [Key]
        public int Id {  get; set; }

        public string Nome { get; set; }

        public decimal OcupacaoMax { get; set; }

		public decimal OcupacaoAtual { get; set; }

        public decimal PrecoBase { get; set; }

		public decimal CustoHoraMaquina
		{
			//Cálculo hora máquina fictício (10% do preço base, divido por 12 meses, divido por 30 dias, divido por 24h)
			get
			{ 
				return PrecoBase * 0.10m / 12 / 30 / 24; 
			}
			set
			{
				value = PrecoBase;
			}
		}

		//Relacionamento Projeto
		public virtual Projeto? Projeto { get; set; }

	}
}	
