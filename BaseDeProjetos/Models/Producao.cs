using System;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Producao
    {
        public virtual int Id { get; set; }

        [Display(Name = "Tipo de Produção")]
        public virtual GrupoProducao Grupo { get; set; }

        [Display(Name = "Instituto")]
        public virtual Instituto Casa { get; set; }

        [Display(Name = "Título")]
        public virtual string Titulo { get; set; }

        [Display(Name = "Descrição/Resumo")]
        public virtual string Descricao { get; set; }

        [Display(Name = "Autores/Participantes")]
        public virtual string Autores { get; set; }

        [Display(Name = "Status")]
        public virtual StatusPub StatusPub { get; set; }

        [Display(Name = "Data:")]
        public virtual DateTime Data { get; set; }

        [Display(Name = "Localização (Estado ou País)")]
        public virtual string Local { get; set; }

        [Display(Name = "DOI")]
        public virtual string DOI { get; set; }

        [Display(Name = "Imagem")]
        public virtual string Imagem { get; set; }

        [Display(Name = "Projeto Associado")]
        public virtual Projeto Projeto { get; set; }

        [Display(Name = "Empresa Associada")]
        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Escritório/Responsável")]
        public virtual string Responsavel { get; set; }

        [Display(Name = "Número de Patente")]
        public virtual string NumPatente { get; set; }

    }
}
