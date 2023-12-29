using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Http;

namespace BaseDeProjetos.Helpers
{
    public class ParametrosFiltroFunil
    {
        public string Casa { get; set; }
        public Usuario Usuario { get; set; }
        public HttpContext HttpContext { get; set; }

        public DbCache Cache { get; set; }
        public string Aba { get; set; }

        public ParametrosFiltroFunil(string casa, Usuario usuario, DbCache cache, string aba, HttpContext httpContext)
        {
            Casa = casa;
            Usuario = usuario;
            Cache = cache;
            Aba = aba;
            HttpContext = httpContext;
        }
    }
}