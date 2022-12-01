using System;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Producao
    {
        public virtual int Id { get; set; }

        [Display(Name = "Tipo de Publicação")]
        public virtual GrupoPublicacao Grupo { get; set; }

        [Display(Name = "Instituto")]
        public virtual Instituto Casa { get; set; }

        [Display(Name = "Título")]
        public virtual string Titulo { get; set; }

        [Display(Name = "Resumo")]
        public virtual string Descricao { get; set; }

        [Display(Name = "Autores/Inventores")]
        public virtual string Autores { get; set; }

        [Display(Name = "Status")]
        public virtual StatusPub StatusPub { get; set; }

        [Display(Name = "Publicado em")]
        public virtual DateTime Data { get; set; }

        [Display(Name = "Localização")]
        public virtual string Local { get; set; }

        [Display(Name = "DOI")]
        public virtual string DOI { get; set; }

        [Display(Name = "Imagem")]
        public virtual string Imagem { get; set; }

        [Display(Name = "Projeto Associado")]
        public virtual string Projeto { get; set; }
        
        [Display(Name = "Empresa Associada")]
        public virtual string Empresa { get; set; }

        [Display(Name = "Escritório/Responsável")]
        public virtual string Responsavel { get; set; }

        [Display(Name = "Num Patente")]
        public virtual string NumPatente { get; set; }

    }
}
