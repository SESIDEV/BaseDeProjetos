using BaseDeProjetos.Models.DTOs;
using BaseDeProjetos.Models.Helpers;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace BaseDeProjetos.Models.ViewModels
{
    public class ParticipacaoTotalViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public UsuarioParticipacaoDTO Lider { get; set; }
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
        public RankParticipacao RankPorIndicador { get; set; }
        public Dictionary<string, decimal> ValoresMinMedMax { get; set; }
        public List<string> Labels { get; set; }
        public List<decimal> Valores { get; set; }
        public RankParticipacao RankSobreMedia { get; set; }

        public string GuidVisualizacao
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
            set
            {
            }
        }

        //public HtmlString AsHtml(string nomeVariavel, bool financeiro, Dictionary<string, decimal> valoresMinimos, Dictionary<string, decimal> valoresMaximos)
        //{
        //    var rankIndicador = RankPorIndicador[$"Rank{nomeVariavel}"];
        //    decimal valor = ExtrairValorDaVariavel(nomeVariavel);

        //    CultureInfo cultura = new CultureInfo("pt-BR");

        //    StringBuilder sb = new StringBuilder();

        //    string valorRank = rankIndicador.ToString("N", cultura);
        //    string comecoDiv = $"<div data-text=\"{valorRank}\" class=\"d-flex flex-row align-items-center gap-2 tooltip-indicadores\">";
        //    int intervalo = BaseDeProjetos.Helpers.Helpers.VerificarIntervalo(valor, valoresMinimos[nomeVariavel], valoresMaximos[nomeVariavel]);
        //    HtmlString iconeParticipacao = BaseDeProjetos.Helpers.Helpers.ObterIconeParticipacao(intervalo);

        //    sb.Append(comecoDiv);
        //    sb.Append(iconeParticipacao.ToString());

        //    if (financeiro)
        //    {
        //        sb.Append($"{valor:C2}");
        //    }
        //    else
        //    {
        //        if (valor == Math.Floor(valor))
        //        {
        //            sb.Append(valor);
        //        }
        //        else
        //        {
        //            sb.Append($"{valor:F}");
        //        }
        //    }

        //    return new HtmlString(sb.ToString());
        //}

        private decimal ExtrairValorDaVariavel(string nomeVariavel)
        {
            PropertyInfo propertyInfo = this.GetType().GetProperty(nomeVariavel);

            decimal valor = 0;

            if (propertyInfo != null)
            {
                try
                {
                    valor = (decimal)propertyInfo.GetValue(this);
                }
                catch (InvalidCastException)
                {
                    valor = (int)propertyInfo.GetValue(this);
                }
            }

            return valor;
        }
    }
}