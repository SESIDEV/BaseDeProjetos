using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Usuario : IdentityUser
    {
        public Instituto Casa { get; set; }

        public Nivel Nivel { get; set; }

        [Display(Name = "Matrícula")]
        public int Matricula { get; set; }

        [Display(Name = "Titulação Máxima")]
        public Titulacao Titulacao { get; set; }

        public TipoVinculo Vinculo { get; set; }

        [Display(Name = "Foto do Perfil")]
        public string Foto { get; set; }

        [Display(Name = "Competências")]
        public string Competencia { get; set; }
    }
}