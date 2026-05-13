using BaseDeProjetos.Models.Enums;
using System.Collections.Generic;

namespace BaseDeProjetos.Models.ViewModels
{
    public class IndicadoresPlanejamentoViewModel
    {
        public Instituto Casa { get; set; }
        public int Ano { get; set; }
        public bool PodeEditar { get; set; }
        public List<Instituto> CasasDisponiveis { get; set; } = new List<Instituto>();
        public List<IndicadoresPlanejamentoLinhaViewModel> Linhas { get; set; } = new List<IndicadoresPlanejamentoLinhaViewModel>();
    }

    public class IndicadoresPlanejamentoLinhaViewModel
    {
        public string Grupo { get; set; }
        public string Chave { get; set; }
        public string Nome { get; set; }
        public Dictionary<int, decimal?> Valores { get; set; } = new Dictionary<int, decimal?>();
    }
}
