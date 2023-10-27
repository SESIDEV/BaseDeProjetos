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
        public int QuantidadeProspeccoesLider { get; set; }
        public decimal TaxaConversaoProposta { get; set; }
        public List<ParticipacaoViewModel> Participacoes { get; set; }
        public int QuantidadeProjetos { get; set; }
        public decimal ValorTotalProspeccoesComProposta { get; set; }
        public decimal ValorMedioProspeccoesComProposta { get; set; }
        public decimal ValorTotalProspeccoesConvertidas { get; set; }
        public decimal ValorMedioProspeccoesConvertidas { get; set; }
        public int QuantidadeProspeccoesComProposta { get; set; }
        public int QuantidadeProspeccoesMembro { get; set; }
        public decimal TaxaConversaoProjeto { get; set; }
        public int QuantidadeProspeccoesProjeto { get; set; }
        public decimal AssertividadePrecificacao { get; set; }
        public decimal FatorContribuicaoFinanceira { get; set; }
        public decimal MediaFatores { get; set; }
        public Dictionary<string, decimal> RankPorIndicador { get; set; }
        public Dictionary<string, decimal> ValoresMinMedMax { get; set; }
        public List<string> Labels { get; set; }
        public List<decimal> Valores { get; set; }
        public List<decimal> RankingsMedios { get; set; }
    }
}