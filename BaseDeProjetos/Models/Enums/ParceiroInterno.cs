using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum ParceiroInterno
    {
        [Display(Name = "ISI-BF")]
        ISI_BF = 1,

        [Display(Name = "ISI-II")]
        ISI_II = 2,

        [Display(Name = "ISI-SVP")]
        ISI_SVP = 3,

        [Display(Name = "CIS-SO")]
        CIS_SO = 4,

        [Display(Name = "IST-QMA")]
        IST_QMA = 5,

        [Display(Name = "IST-EDI")]
        IST_EDI = 6,

        [Display(Name = "ISI-QV")]
        ISI_QV = 7,

        [Display(Name = "A definir")]
        Adefinir = 8,

        [Display(Name = "Não há")]
        NaoHa = 9
    }
}
