using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Models
{
    public class Projeto
    {
        [Key]
        public string Id { get; set; }
        public string Empresa {get;set;}
    }
}
