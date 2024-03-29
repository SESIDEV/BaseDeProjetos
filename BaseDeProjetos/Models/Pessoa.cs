﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
    public class Pessoa
    {
        [Key]
        public virtual int id { get; set; }

        [Display(Name = "Nome da pessoa")]
        public virtual string Nome { get; set; }

        [Display(Name = "Email da pessoa")]
        public virtual string Email { get; set; }

        [Display(Name = "Telefone da pessoa")]
        public virtual string Telefone { get; set; }

        [ForeignKey("EmpresaId")]
        public virtual Empresa empresa { get; set; }

        public int? EmpresaId { get; set; }

        public virtual string Cargo { get; set; }
    }
}