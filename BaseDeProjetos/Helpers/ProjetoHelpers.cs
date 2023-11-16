using Microsoft.AspNetCore.Html;
using System.Text;

namespace BaseDeProjetos.Helpers
{
    public static class ProjetoHelpers
    {
        public static HtmlString GerarContainersModais(string projetoId)
        {
            StringBuilder sb = new StringBuilder();

            string[] modalTypes = new string[]
            {
                "Edit", "Delete", "History", "Details",
                "IncluirIndicador", "DetalhesIndicador", "IncluirCFF",
                "DetalhesCFF", "EditCFF", "DetalhesRubricas", "IncluirRubricas"
            };

            foreach (var modalType in modalTypes)
            {
                sb.Append($"<div id=\"modal{modalType}Projeto-{projetoId}-container\"></div>\n");
            }

            return new HtmlString(sb.ToString());
        }
    }
}
