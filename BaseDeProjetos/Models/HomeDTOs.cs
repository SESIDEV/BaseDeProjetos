using System.Collections.Generic;

namespace BaseDeProjetos.Models
{
    public class ProspeccaoHomeDTO
    {
        public Instituto Casa { get; set; }
        public decimal ValorEstimado { get; set; }
        public List<FollowUp> Status { get; set; }
        public Empresa Empresa { get; set; }
        public LinhaPesquisa LinhaPequisa { get; set; }
    }

    public class ProjetosHomeDTO
    {
        public Instituto Casa { get; set; }
        public StatusProjeto Status { get; set; }
    }

    public class UsuariosHomeDTO
    {
        public string Id { get; set; }
        public Instituto Casa { get; set; }
        public bool EmailConfirmed { get; set; }
        public Nivel Nivel { get; set; }
    }
}