using System;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Producao
    {
        public virtual int Id { get; set; }

        [Display(Name = "Tipo de Publicação")]
        public virtual GrupoPublicacao Grupo { get; set; }

        [Display(Name = "Título")]
        public virtual string Titulo { get; set; }

        [Display(Name = "Resumo")]
        public virtual string Descricao { get; set; }

        [Display(Name = "Autores")]
        public virtual string Autores { get; set; }

        [Display(Name = "Publicado em")]
        public virtual DateTime Data { get; set; }

        [Display(Name = "Localização")]
        public virtual string Local { get; set; }

        [Display(Name = "DOI")]
        public virtual string DOI { get; set; }

        [Display(Name = "Imagem")]
        public virtual string Imagem { get; set; }

    }
}
