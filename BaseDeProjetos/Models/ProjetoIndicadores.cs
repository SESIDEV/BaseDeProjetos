using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
	public class ProjetoIndicadores
	{
		[Key]
		[Display(Name = "Id do indicador do projeto")]
		public virtual string Id { get; set; }

		[ForeignKey("IdProjeto")]
		public virtual Projeto Projeto { get; set; }

		public virtual string IdProjeto { get; set; }

		[Display(Name = "Regramento")]
		public virtual bool Regramento { get; set; }

		[Display(Name = "Repasse")]
		public virtual bool Repasse { get; set; }

		[Display(Name = "Abertura de SC (Serviço)")]
		public virtual bool ComprasServico { get; set; }

		[Display(Name = "Abertura de SC (Material)")]
		public virtual bool ComprasMaterial { get; set; }

		[Display(Name = "Contratação de Bolsista")]
		public virtual bool Bolsista { get; set; }

		[Display(Name = "Questionário de Satisfação na Metade do Projeto")]
		public virtual bool SatisfacaoMetadeProjeto { get; set; }
		public virtual int ValorSatisfacaoMetadeProjeto { get; set; }

		[Display(Name = "Questionário de Satisfação no Fim do Projeto")]
		public virtual bool SatisfacaoFimProjeto { get; set; }
		public virtual int ValorSatisfacaoFimProjeto { get; set; }
		[Display(Name = "Relatórios")]
		public virtual bool Relatorios { get; set; }

		[Display(Name = "Prestação de Contas")]
		public virtual bool PrestacaoContas { get; set; }
	}
}
