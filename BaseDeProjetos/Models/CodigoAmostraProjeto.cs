using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BaseDeProjetos.Models
{
    public class CodigoAmostraProjeto
    {

        public CodigoAmostraProjeto()
        {

        }

        [Key]
        public virtual int Id { get; set; }

        [Display(Name = "Projeto")]
        public virtual Projeto Projeto { get; set; }

        [Display(Name = "Código")]
        public virtual string Codigo { get; set; }

    }
}