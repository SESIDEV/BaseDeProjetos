using Microsoft.AspNetCore.Http;

namespace BaseDeProjetos.Models.Helpers
{
    public class ParametrosFunil
    {
        public string Aba { get; set; }
        public string SearchString { get; set; }
        public int NumeroPagina { get; set; }
        public int TamanhoPagina { get; set; }
        public string SortOrder { get; set; }

        public void ObterParametrosSession(ISession session)
        {
            Aba = session.GetString("aba");
            SearchString = session.GetString("searchString");
            NumeroPagina = session.GetInt32("numeroPagina") ?? 1;
            TamanhoPagina = session.GetInt32("tamanhoPagina") ?? 20;
            SortOrder = session.GetString("sortOrder");
        }
    }
}
