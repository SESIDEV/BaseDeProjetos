using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class ConjuntoRubrica
    {
        public int Id { get; set; }

        [Display(Name = "Lista de Rúbricas")]
        public virtual List<Rubrica> Rubricas { get; set; }
    }
}