using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace BaseDeProjetos.Models
{
    public class Editais
    {
        [Key]
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
        public virtual decimal ValorEdital { get; set; }
        [Display(Name = "Link do edital")]
        public virtual string LinkEdital { get; set; }

        public virtual List<Submissao> Submissoes { get; set; }

        [Display(Name = "Data do resultado")]
        public virtual DateTime DataResultado { get; set; }


    }

    public class Submissao
    {
        [Display(Name = "Id da Sumbmissão")]
        [Key]
        public virtual string Id { get; set; }
        [Display(Name = "Empresa do Edital")]
        public virtual bool ComEmpresa { get; set; }
        [Display(Name = "Edital")]
        public virtual Editais Edital { get; set; }
        [Display(Name = "Id do Edital")]
        public virtual string EditalId { get; set; }
        [Display(Name = "Prospecção")]
        public virtual Prospeccao Prospeccao { get; set; }
        [Display(Name = "Id da Prospecção")]
        public virtual string ProspeccaoId { get; set; }
        [Display(Name = "Proponente do Edital")]
        public virtual string Proponente { get; set; }
        [Display(Name = "Projeto proposto")]
        public virtual string ProjetoProposto { get; set; }
        [Display(Name = "Líder do projeto")]
        public virtual string ResponsavelSubmissao { get; set; }
        [Display(Name = "Status da submissão")]
        public virtual StatusSubmissaoEdital StatusSubmissao { get; set; }
    }
}
