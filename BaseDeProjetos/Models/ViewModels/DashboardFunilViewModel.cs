using BaseDeProjetos.Models.Helpers;

namespace BaseDeProjetos.Models.ViewModels
{
    public class DashboardFunilViewModel
    {
        IndicadoresProspeccao IndicadoresProspeccao { get; set; }
        StatusGeralProspeccaoPizza StatusGeralProspeccaoPizza { get; set; }
        StatusProspeccoesPropostaPizza StatusProspeccoesPropostaPizza { get; set; }
        StatusProspPropostaIndicadores StatusProspPropostaIndicadores { get; set; }
    }
}
