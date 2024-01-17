using BaseDeProjetos.Models.Enums;
using NUnit.Framework;
using System.Collections.Generic;

namespace BaseDeProjetos.Models.DTOs
{
    public class ProspeccaoModalPreCreateProspDTO
    {
        public string Id { get; set; }
        public List<FollowUp> Status { get; set; }
        public Empresa Empresa { get; set; }
        public Usuario Usuario { get; set; }
    }
}
