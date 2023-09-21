using System;

namespace BaseDeProjetos.Models
{
    public class ParticipacaoViewModel
    {
        public Guid Id { get; set; }
        public decimal TotalLider { get; set; }
        public decimal TotalEquipe { get; set; }
        public int QuantidadePesquisadores { get; set; } = 0;
        public int QuantidadeBolsistas { get; set; } = 0;
        public int QuantidadeEstagiarios { get; set; } = 0;
        public int QuantidadeMembros { get; set; } = 0;
        public decimal ValorEstagiarios { get; set; }
        public decimal ValorBolsistas { get; set; }
        public decimal ValorPesquisadores { get; set; }
        public decimal ValorLider { get; set; }
        public decimal ValorNominal { get; set; }
        public string NomeProjeto { get; set; }
        public string EmpresaProjeto { get; set; }
        public string MembrosEquipe { get; set; }
        public decimal ValorPorBolsista { get; set; }
        public decimal ValorPorEstagiario { get; set; }
        public decimal ValorPorPesquisador { get; set; }
        public bool Convertida { get; set; }
        public bool Planejada { get; set; }
        public bool Suspensa { get; set; }
        public bool NaoConvertida { get; set; }
        public bool EmDiscussao { get; set; }
        public bool ComProposta { get; set; }
    }
}
