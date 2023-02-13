using System.Linq;
using System.Text.Json;
using BaseDeProjetos.Data;

namespace BaseDeProjetos.Helpers
{
    public static class Helpers
    {
        public static string FormatarValoresDashboards(decimal valor)
        {
            if (valor >= 100000000)
            {
                return (valor / 1000000).ToString("0.# MI");
            }
            if (valor >= 1000000)
            {
                return (valor / 1000000).ToString("0.## MI");
            }
            if (valor >= 100000)
            {
                return (valor / 1000).ToString("0.# mil");
            }
            if (valor >= 10000)
            {
                return (valor / 1000).ToString("0.## mil");
            }

            return valor.ToString("#,0");
        }

        public static string PuxarDadosUsuarios(ApplicationDbContext _context){
            
            var usuarios = _context.Users.Where(u => u.Email != null && u.EmailConfirmed == true).Select(u => new {u.Email, u.UserName, u.Foto, u.Competencia}).ToList();

            string usuariosJson = JsonSerializer.Serialize(usuarios);

            return usuariosJson;

        }

        public static string PuxarTagsProspecoes(ApplicationDbContext _context)
        {

            var tags = _context.Prospeccao.Where(p => p.Tags != null).Select(p => new { p.Tags }).ToList();

            string tagsJson = JsonSerializer.Serialize(tags);

            return tagsJson;

        }

        public static string PuxarDadosEmpresas(ApplicationDbContext _context)
        {

            var empresas = _context.Empresa.Where(e => e.Nome != null);//.Select(e => new {e.Nome, e.Segmento.GetDisplayName()}).ToList();

            string empresasJson = JsonSerializer.Serialize(empresas);

            return empresasJson;

        }

        public static string PuxarDadosProducoes(ApplicationDbContext _context)
        {

            var producoes = _context.Producao.Where(p => p.Titulo != null).Select(p => new {p.Casa, p.Titulo, p.Descricao, p.Autores, p.StatusPub, p.Data, p.Local, p.DOI}).ToList(); //.GetDisplayName() NAO FUNCIONA

            string producoesJson = JsonSerializer.Serialize(producoes);

            return producoesJson;

        }
    }

}