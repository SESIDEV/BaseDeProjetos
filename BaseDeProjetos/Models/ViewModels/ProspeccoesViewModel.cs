using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Helpers;
using System.Collections.Generic;

namespace BaseDeProjetos.Models.ViewModels
{
    public class ProspeccoesViewModel
    {
        public List<Prospeccao> Prospeccoes { get; set; }
        public List<Prospeccao> ProspeccoesGrafico { get; set; }
        public List<ProspeccaoAgregadaDTO> ProspeccoesAgregadas { get; set; }

        public Pager Pager { get; set; }
    }
}