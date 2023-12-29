using BaseDeProjetos.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class AtividadesProdutivas
    {
        [Key]
        public virtual int Id { get; set; }

        [Display(Name = "Tipo da Atividade")]
        public virtual Atividades Atividade { get; set; }

        [Display(Name = "Área da Atividade")]
        public virtual AreasAtividades AreaAtividade { get; set; }

        [Display(Name = "Fonte de Fomento do Projeto/Prospecção")]
        public virtual TipoContratacao FonteFomento { get; set; }

        [Display(Name = "Projeto/Prospecção")]
        public virtual string ProjetoId { get; set; }

        public virtual Usuario Usuario { get; set; }

        [Display(Name = "Descrição da atividade")]
        public virtual string DescricaoAtividade { get; set; }

        [Display(Name = "Carga horária")]
        public virtual double CargaHoraria { get; set; }

        public virtual DateTime Data { get; set; }
    }
}