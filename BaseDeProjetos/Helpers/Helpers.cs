using System.Linq;
using System.Text.Json;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using System.Collections.Generic;

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
            
            var usuarios = _context.Users.Where(u => u.Email != null).Select(u => new {u.Email, u.UserName, u.Foto}).ToList(); //, u.Foto

            string usuariosJson = JsonSerializer.Serialize(usuarios);

            return usuariosJson;

        }
    }

}