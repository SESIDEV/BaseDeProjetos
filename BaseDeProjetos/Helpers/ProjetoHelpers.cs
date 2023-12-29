using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Text;

namespace BaseDeProjetos.Helpers
{
    public static class ProjetoHelpers
    {
        //public static HtmlString GerarContainersModais(string projetoId)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    string[] modalTypes = new string[]
        //    {
        //        "EditCFF", "DetalhesRubricas", "IncluirRubricas"
        //    };

        //    foreach (var modalType in modalTypes)
        //    {
        //        sb.Append($"<div id=\"modal{modalType}Projeto-{projetoId}-container\"></div>\n");
        //    }

        //    return new HtmlString(sb.ToString());
        //}

        public static HtmlString GerarContainersModais(List<CurvaFisicoFinanceira> curvas)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var curva in curvas)
            {
                sb.Append($"<div id=\"modalEditCFFProjeto-{curva.Id}-container\"></div>\n");
            }

            return new HtmlString(sb.ToString());
        }

        public static HtmlString GerarContainersModais(List<Rubrica> rubricas)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var rubrica in rubricas)
            {
                sb.Append($"<div id=\"modalEditRubricasProjeto-{rubrica.Id}-container\"></div>\n");
            }

            return new HtmlString(sb.ToString());
        }
    }
}