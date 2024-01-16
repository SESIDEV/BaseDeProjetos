using BaseDeProjetos.Helpers;
using BaseDeProjetos.Models.Helpers;
using System.Collections.Generic;

namespace BaseDeProjetos.Models.ViewModels
{
    public class ProspeccoesViewModel
    {
        public List<Prospeccao> Prospeccoes { get; set; }
        public int ProspeccoesConcluidas { get; set; }
        public int ProspeccoesComProposta { get; set; }
        public int ProspeccoesPlanejadas { get; set; }
        public List<Prospeccao> ProspeccoesGrafico { get; set; }
        public List<Prospeccao> ProspeccoesAgregadas { get; set; }
        public List<Prospeccao> ProspeccoesAtivas { get; set; }
        public List<Prospeccao> ProspeccoesNaoPlanejadas { get; set; }
        public List<Prospeccao> ProspeccoesAvancadas { get; set; }

        public Pager Pager { get; set; }
    }
}