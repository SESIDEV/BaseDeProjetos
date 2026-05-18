using BaseDeProjetos.Models.Enums;
using System.Collections.Generic;

namespace BaseDeProjetos.Models.ViewModels
{
    public class IndicadoresReceitaGestorViewModel
    {
        public Instituto Casa { get; set; }
        public int AnoBase { get; set; }
        public bool PodeEditar { get; set; }
        public List<Instituto> CasasDisponiveis { get; set; } = new List<Instituto>();
        public List<IndicadoresReceitaGestorLinhaViewModel> Linhas { get; set; } = new List<IndicadoresReceitaGestorLinhaViewModel>();
    }

    public class IndicadoresReceitaGestorLinhaViewModel
    {
        public int Id { get; set; }
        public string Empresa { get; set; }
        public string Iniciativa { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? ReceitaTotal { get; set; }
        public decimal? ReceitaAnoBase { get; set; }
        public decimal? ProjecaoAno1 { get; set; }
        public decimal? ProjecaoAno2 { get; set; }
        public decimal? ProjecaoAno3 { get; set; }
        public decimal? ProjecaoAno4 { get; set; }
        public decimal? ProjecaoAno5 { get; set; }
        public string ParceiroInterno { get; set; }
    }
}
