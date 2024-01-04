using System;
using System.Collections.Generic;

namespace BaseDeProjetos.Models.ViewModels
{
    public class ParticipacaoTotalViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Usuario Lider { get; set; }
        public decimal ValorTotalProspeccoes { get; set; } = 0;
        public decimal ValorMedioProspeccoes { get; set; } = 0;
        public int QuantidadeProspeccoes { get; set; } = 0;
        public int QuantidadeProspeccoesLider { get; set; } = 0;
        public decimal TaxaConversaoProposta { get; set; } = 0;
        public List<ParticipacaoViewModel> Participacoes { get; set; }
        public int QuantidadeProjetos { get; set; } = 0;
        public decimal ValorTotalProspeccoesComProposta { get; set; } = 0;
        public decimal ValorMedioProspeccoesComProposta { get; set; } = 0;
        public decimal ValorTotalProspeccoesConvertidas { get; set; } = 0;
        public decimal ValorMedioProspeccoesConvertidas { get; set; } = 0;
        public decimal QuantidadeProspeccoesComProposta { get; set; }
        public int QuantidadeProspeccoesMembro { get; set; } = 0;
        public decimal TaxaConversaoProjeto { get; set; } = 0;
        public decimal QuantidadeProspeccoesProjeto { get; set; } = 0;
        public decimal AssertividadePrecificacao { get; set; } = 0;
        public decimal FatorContribuicaoFinanceira { get; set; } = 0;
        public decimal MediaFatores { get; set; } = 0;
        public Dictionary<string, decimal> RankPorIndicador { get; set; }
        public Dictionary<string, decimal> ValoresMinMedMax { get; set; }
        public List<string> Labels { get; set; }
        public List<decimal> Valores { get; set; }
        public List<decimal> RankingsMedios { get; set; }
    }
}