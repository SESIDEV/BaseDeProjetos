using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum Competencia
    {
        [Display(Name = "...")]
        Zero = 0,

        [Display(Name = "HTML/CSS")]
        HTMLCSS,

        [Display(Name = "Javascript")]
        JS,

        [Display(Name = "Python")]
        Python,

        [Display(Name = "MySQL")]
        MYSQL,

        [Display(Name = "C#")]
        CSharp,

        [Display(Name = "C++")]
        Cmm,

        [Display(Name = "Photoshop")]
        Photoshop,

        [Display(Name = "Excel")]
        Excel,
    }
}