using BaseDeProjetos.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
    public class Entrega
    {
        public string Id { get; set; }
        public virtual Projeto Projeto { get; set; }
        public string ProjetoId { get; set; }
        [Display(Name = "Nome da entrega")]
        public string NomeEntrega { get; set; }
        [Display(Name = "Descrição da entrega")]
        public string DescricaoEntrega { get; set; }
        [Display(Name ="Data de início da atividade")]
        public virtual DateTime DataInicioEntrega { get; set; }
        [Display(Name ="Data prevista de término da atividade")]
        public virtual DateTime DataFim { get; set; }
        public bool Concluida { get; set; } = false;

        [NotMapped]
        public bool Atrasada { get
            {
                return Concluida ? false : DateTime.Today < DataFim;
            }
        }
    }
}
