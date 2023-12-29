using BaseDeProjetos.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{
    public class Empresa
    {
        public virtual int Id { get; set; }

        [Display(Name = "Razão Social")]
        public virtual string RazaoSocial { get; set; }

        [Display(Name = "Nome")]
        public virtual string Nome { get; set; }

        [Display(Name = "Logo da Empresa")]
        public virtual string Logo { get; set; }

        [Display(Name = "CNPJ da Empresa")]
        public virtual string CNPJ { get; set; }

        [Display(Name = "Segmento da Empresa")]
        public virtual SegmentoEmpresa Segmento { get; set; }

        [Display(Name = "Estado da Empresa")]
        public virtual Estado Estado { get; set; }

        [Display(Name = "Enquadramento CNAE")]
        public bool Industrial { get; set; }

        public string EmpresaUnique
        {
            get => Nome + " - [" + CNPJ + "]" + " - " + Id;
            private set { }
        }
    }
}