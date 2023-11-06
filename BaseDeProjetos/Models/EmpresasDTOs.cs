using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models
{

    public class ProspeccaoEmpresasDTO
    {
        public string Id { get; set; }
        public virtual List<FollowUp> Status { get; set; }
    }

    public class EmpresaDTO
    {
        public virtual int Id { get; set; }
        [Display(Name = "Razão Social")]
        public virtual string RazaoSocial { get; set; }
        [Display(Name = "Nome")]
        public virtual string Nome { get; set; }
        [Display(Name = "CNPJ da Empresa")]
        public virtual string CNPJ { get; set; }
    }
}