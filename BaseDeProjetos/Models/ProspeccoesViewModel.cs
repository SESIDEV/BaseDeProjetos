using BaseDeProjetos.Helpers;
using System.Collections.Generic;

namespace BaseDeProjetos.Models
{
    public class ProspeccoesViewModel
    {
        public List<Prospeccao> Prospeccoes { get; set; }
        public List<Prospeccao>? ProspeccoesGrafico { get; set; }
        public List<Prospeccao>? ProspeccoesAgregadas { get; set; }
        public List<Prospeccao>? ProspeccoesAtivas { get; set; }
        public List<Prospeccao>? ProspeccoesNaoPlanejadas { get; set; }
        public List<Prospeccao>? ProspeccoesAvancadas { get; set; }

        public Pager Pager { get; set; }
    }
}