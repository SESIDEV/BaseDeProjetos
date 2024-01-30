using BaseDeProjetos.Data;
using Microsoft.CodeAnalysis.Host.Mef;
using System;
using System.Threading.Tasks;

namespace BaseDeProjetos.Models.Formulario
{
    
    public class GerenciarForms
    {
        public static async Task AdicionarFormulario(Formulario formulario, ApplicationDbContext context)
        {
            formulario.DataCriacao = DateTime.Now;
            context.Add(formulario);
            await context.SaveChangesAsync();
        }
    }
}
