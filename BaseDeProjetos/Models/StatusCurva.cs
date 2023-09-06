using System;

namespace BaseDeProjetos.Models
{
	public class StatusCurva
	{
		public virtual int Id { get; set; }
		public virtual int IdProjeto { get; set; }
		public virtual DateTime Data { get; set; }
		public virtual decimal Fisico { get; set; }
		public virtual decimal Financeiro { get; set; }
	}
}
