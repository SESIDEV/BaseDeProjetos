using System;

namespace BaseDeProjetos.Models
{
	public class StatusCurva
	{
		public virtual int Id { get; set; }
		public virtual DateTime Data { get; set; }
		public virtual decimal Fisico { get; set; }
		public virtual decimal Financeiro { get; set; }

		public virtual Projeto Projeto { get; set; }
		public string ProjetoId { get; set; }
	}
}
