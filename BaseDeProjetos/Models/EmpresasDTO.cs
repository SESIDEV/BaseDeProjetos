using System.Collections.Generic;

namespace BaseDeProjetos.Models
{
    public class ProspeccaoEmpresasDTO
    {
        public string Id { get; set; }
        public virtual List<FollowUp> Status { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual string NomeProspeccao { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}