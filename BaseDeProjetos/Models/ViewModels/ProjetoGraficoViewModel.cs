using System.Collections.Generic;
using BaseDeProjetos.Models.DTOs;

namespace BaseDeProjetos.Models.ViewModels
{
    public class ProjetoGraficoViewModel
    {
        public string Casa { get; set; }
        public List<ProjetoDTO> ProjetosContratacaoDireta { get; set; }
        public List<ProjetoDTO> ProjetosEditalInovacao { get; set; }
        public List<ProjetoDTO> ProjetosEmbrapii { get; set; }
        public List<ProjetoDTO> ProjetosAgenciaFomento { get; set; }
        public List<ProjetoDTO> ProjetosIndefinido { get; set; }
        public List<ProjetoDTO> ProjetosANP { get; set; }
    }
}