using BaseDeProjetos.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTesting.Models
{
    public class Entrega
    {
        public int Id { get; set; }
        public virtual Projeto projeto { get; set; }
        public string projetoId { get; set; }
        [Display(Name = "Nome da entrega")]
        public string NomeEntrega { get; set; }
        [Display(Name = "Descrição da entrega")]
        public string DescricaoEntrega { get; set; }
        [Display(Name ="Data de início da atividade")]
        public virtual DateTime DataInicio { get; set; }
        [Display(Name ="Data prevista de término da atividade")]
        public virtual DateTime DataFim { get; set; }

        public bool Concluida { get; set; } = false;
    }
}
