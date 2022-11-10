using System;
using System.ComponentModel.DataAnnotations;
namespace BaseDeProjetos.Models
{
    public class Editais
    {
        public virtual int Id { get; set; }
        [Display(Name = "Nome do edital")]
        public virtual string Name { get; set; }
        [Display(Name = "Descrição do edital")]
        public virtual string Descricao { get; set; }
        [Display(Name = "Local")]
        public virtual string Local { get; set; }
        [Display(Name = "Status do edital")]
        public virtual StatusEdital StatusEdital { get; set; }
        [Display(Name = "Agência de fomento")]
        public virtual AgenciaDeFomento AgenciaFomento { get; set; }
        [Display(Name = "Prazo de submissão")]
        public virtual DateTime PrazoSubmissao { get; set; }
        [Display(Name = "Valor total do edital")]
        public virtual decimal  ValorEdital { get; set; }
        [Display(Name = "Proponente")]
        public virtual string Proponente { get; set; }
        [Display(Name = "Link do edital")]
        public virtual string LinkEdital { get; set; }
        [Display(Name = "Projeto proposto")]
        public virtual string ProjetoProposto { get; set; }
        [Display(Name = "Líder do projeto")]
        public virtual Pessoa ResponsavelSubmissao { get; set; }
        [Display(Name = "Data do resultado")]
        public virtual DateTime DataResultado { get; set; }
        [Display(Name = "Status da submissão")]
        public virtual StatusSubmissaoEdital StatusSubmissao { get; set; }


    }
}
