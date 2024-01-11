using System.Collections.Generic;

namespace BaseDeProjetos.Models.Helpers
{
    public class ProspeccoesUsuarioParticipacao
    {
        public List<Prospeccao> ProspeccoesTotais { get; set; }
        public List<Prospeccao> ProspeccoesLider { get; set; }
        public List<Prospeccao> ProspeccoesMembro { get; set; }
    }
}