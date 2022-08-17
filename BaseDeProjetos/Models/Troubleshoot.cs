using System;
using System.ComponentModel.DataAnnotations;
namespace BaseDeProjetos.Models
{
    public class Troubleshoot
    {
        public virtual int Id { get; set; }

        [Display(Name = "Categoria")]
        public virtual CategoriaTroubleshoot Tag { get; set; }
    
        [Display(Name = "Título")]
        public virtual string Titulo { get; set; }

        [Display(Name = "Descrição")]
        public virtual string Descricao { get; set; }
        
        public virtual DateTime Data { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual StatusTroubleshoot Status { get; set; }

    }
}
