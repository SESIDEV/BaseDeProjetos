using System.Collections.Generic;

namespace BaseDeProjetos.Models.Helpers
{
    public class ProspeccoesUsuarioParticipacao
    {
        public List<Prospeccao> ProspeccoesTotais { get; set; }
        public List<Prospeccao> ProspeccoesLider { get; set; }
        public List<Prospeccao> ProspeccoesMembro { get; set; }
        public List<Prospeccao> ProspeccoesTotaisConvertidas { get; internal set; }
        public List<Prospeccao> ProspeccoesTotaisComProposta { get; internal set; }
        public object ProspeccoesLiderConvertidas { get; internal set; }
    }
}