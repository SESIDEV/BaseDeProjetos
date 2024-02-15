using NUnit.Framework;
using System.Collections.Generic;

namespace BaseDeProjetos.Models.DTOs
{
    public class ProspeccaoAgregadaDTO
    {
        public string Id { get; set; }
        public string NomeProspeccao { get; set; }
        public List<FollowUp> Status { get; set; }
        public string EmpresaNome { get; set; }

    }
}
