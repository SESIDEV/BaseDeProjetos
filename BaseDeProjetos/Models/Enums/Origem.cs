using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum Origem
    {
        [Display(Name = "Unidade")]
        Unidade,

        [Display(Name = "ICT parceira")]
        ICTParceira,

        [Display(Name = "Empresa / Cliente")]
        EmpresaCliente,

        [Display(Name = "Catalisa SEBRAE")]
        CatalisaSebrae,

        [Display(Name = "Programa Prospectores")]
        ProgramaProspectores,

        [Display(Name = "A definir")]
        Adefinir
    }
}
