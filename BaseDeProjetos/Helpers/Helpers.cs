using System.Linq;
using System.Text.Json;
using BaseDeProjetos.Data;

namespace BaseDeProjetos.Helpers
{
    public static class Helpers
    {

        /// <summary>
        /// Formata um valor decimal para melhor visualização em Dashboard
        /// </summary>
        /// <param name="valor">Valor a ser formatado</param>
        /// <returns></returns>
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

        /// <summary>
        /// Retorna Email, Nome de Usuário, Foto e Competência dos usuários com email cadastrado em formato JSON
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        public static string PuxarDadosUsuarios(ApplicationDbContext _context){
            
            var usuarios = _context.Users.Where(u => u.Email != null && u.EmailConfirmed == true).Select(u => new {u.Email, u.UserName, u.Foto, u.Competencia}).ToList();

            string usuariosJson = JsonSerializer.Serialize(usuarios);

            return usuariosJson;

        }

        /// <summary>
        /// Retorna as tags não nulas de uma prospecção em JSON
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        public static string PuxarTagsProspecoes(ApplicationDbContext _context)
        {

            var tags = _context.Prospeccao.Where(p => p.Tags != null).Select(p => new { p.Tags }).ToList();

            string tagsJson = JsonSerializer.Serialize(tags);

            return tagsJson;

        }

        /// <summary>
        /// Retorna os dados de uma empresa, onde os nomes não são nulos, em JSON
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        public static string PuxarDadosEmpresas(ApplicationDbContext _context)
        {

            var empresas = _context.Empresa.Where(e => e.Nome != null);//.Select(e => new {e.Nome, e.Segmento.GetDisplayName()}).ToList();

            string empresasJson = JsonSerializer.Serialize(empresas);

            return empresasJson;

        }

        /// <summary>
        /// Retorna em JSON os dados de uma produção, onde o nome não é nulo, selecionando: Casa, Titulo, Descrição, Autores, Status de Publicação, Data, Local e DOI
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        public static string PuxarDadosProducoes(ApplicationDbContext _context)
        {

            var producoes = _context.Producao.Where(p => p.Titulo != null).Select(p => new {p.Casa, p.Titulo, p.Descricao, p.Autores, p.StatusPub, p.Data, p.Local, p.DOI}).ToList(); //.GetDisplayName() NAO FUNCIONA

            string producoesJson = JsonSerializer.Serialize(producoes);

            return producoesJson;

        }
    }

}