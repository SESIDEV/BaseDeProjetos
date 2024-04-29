using BaseDeProjetos.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Controle
    {
        public virtual int Id { get; set; }

        [Display(Name = "Codigo")]
        public virtual string Codigo { get; set; }

        [Display(Name = "Item")]
        public virtual string Item { get; set; }

        [Display(Name = "Responsável")]
        public virtual Usuario Usuario { get; set; }

        [Display(Name = "Projeto")]
        public virtual string Projeto { get; set; }

        [Display(Name = "Anotações")]
        public virtual string Anotações { get; set; }

        [Display(Name = "Data de Cadastro")]
        public virtual DateTime Data { get; set; } = DateTime.Now;

    }
}