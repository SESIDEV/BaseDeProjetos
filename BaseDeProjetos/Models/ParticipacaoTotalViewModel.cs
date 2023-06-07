using System;
using System.Collections.Generic;

namespace BaseDeProjetos.Models
{
    public class ParticipacaoTotalViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Usuario Lider { get; set; }
        public decimal ValorTotalProspeccoes { get; set; }
        public decimal ValorMedioProspeccoes { get; set; }
        public int QuantidadeProspeccoes { get; set; }
        public decimal TaxaConversaoProposta { get; set; }
        public List<ParticipacaoViewModel> Participacoes { get; set; }
		public int QuantidadeProjetos { get; set; }
		public decimal ValorTotalProjetos { get; set; }
		public decimal ValorMedioProjetos { get; set; }
        public int QuantidadeProspeccoesComProposta { get; set; }
        public decimal TaxaConversaoProjeto { get; set; }
        public int QuantidadeProspeccoesProjetizadas { get; set; }
        public decimal Rank { get; set; }
    }

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
        public string MembrosEquipe { get; set; }
    }
}
