using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
	public class StatusCurva
	{
		public virtual int Id { get; set; }
		public virtual DateTime Data { get; set; }
		public virtual decimal Fisico { get; set; }
		public virtual decimal Financeiro { get; set; }
		[ForeignKey("ProjetoId")]
		public virtual Projeto Projeto { get; set; }
		public virtual string ProjetoId { get; set; }
	}
}
