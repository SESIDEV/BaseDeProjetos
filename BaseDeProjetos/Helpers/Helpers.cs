using System;
using System.Linq;
using System.Text.Json;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Html;

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

        public static HtmlString PrintarValorIndicadorParticipacao(decimal valorIndicadorCru, decimal valorMedio)
        {
            HtmlString htmlString = new HtmlString($"");

            if (valorIndicadorCru > valorMedio * (decimal)1.33333333)
            {
                htmlString = new HtmlString("<svg style=\"color: green\" xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-chevron-up\" viewBox=\"0 0 16 16\">\r\n <path fill-rule=\"evenodd\" d=\"M7.646 4.646a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1-.708.708L8 5.707l-5.646 5.647a.5.5 0 0 1-.708-.708l6-6z\" />\r\n </svg>");
            }
            else if (valorIndicadorCru <= (decimal)1.33333333 && valorIndicadorCru > valorMedio / (decimal)1.51515151)
            {
                htmlString = new HtmlString("<svg style=\"color: dodgerblue\" xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-chevron-bar-contract\" viewBox=\"0 0 16 16\">\r\n <path fill-rule=\"evenodd\" d=\"M3.646 14.854a.5.5 0 0 0 .708 0L8 11.207l3.646 3.647a.5.5 0 0 0 .708-.708l-4-4a.5.5 0 0 0-.708 0l-4 4a.5.5 0 0 0 0 .708zm0-13.708a.5.5 0 0 1 .708 0L8 4.793l3.646-3.647a.5.5 0 0 1 .708.708l-4 4a.5.5 0 0 1-.708 0l-4-4a.5.5 0 0 1 0-.708zM1 8a.5.5 0 0 1 .5-.5h13a.5.5 0 0 1 0 1h-13A.5.5 0 0 1 1 8z\" />\r\n </svg>");
            }
            else
            {
                htmlString = new HtmlString("<svg style=\"color: red\" xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-chevron-down\" viewBox=\"0 0 16 16\">\r\n <path fill-rule=\"evenodd\" d=\"M1.646 4.646a.5.5 0 0 1 .708 0L8 10.293l5.646-5.647a.5.5 0 0 1 .708.708l-6 6a.5.5 0 0 1-.708 0l-6-6a.5.5 0 0 1 0-.708z\" />\r\n </svg>");
            }

            return htmlString;
        }

        public static HtmlString PrintarValorIndicadorParticipacao(decimal valorIndicadorCru)
        {
            HtmlString htmlString = new HtmlString($"");

            if (valorIndicadorCru <= 1 && valorIndicadorCru > (decimal)66 / 100)
            {
                htmlString = new HtmlString("<svg style=\"color: green\" xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-chevron-up\" viewBox=\"0 0 16 16\">\r\n <path fill-rule=\"evenodd\" d=\"M7.646 4.646a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1-.708.708L8 5.707l-5.646 5.647a.5.5 0 0 1-.708-.708l6-6z\" />\r\n </svg>");                   
            }
            else if (valorIndicadorCru <= (decimal)66 / 100 && valorIndicadorCru > (decimal)33 / 100)
            {
                htmlString = new HtmlString("<svg style=\"color: dodgerblue\" xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-chevron-bar-contract\" viewBox=\"0 0 16 16\">\r\n <path fill-rule=\"evenodd\" d=\"M3.646 14.854a.5.5 0 0 0 .708 0L8 11.207l3.646 3.647a.5.5 0 0 0 .708-.708l-4-4a.5.5 0 0 0-.708 0l-4 4a.5.5 0 0 0 0 .708zm0-13.708a.5.5 0 0 1 .708 0L8 4.793l3.646-3.647a.5.5 0 0 1 .708.708l-4 4a.5.5 0 0 1-.708 0l-4-4a.5.5 0 0 1 0-.708zM1 8a.5.5 0 0 1 .5-.5h13a.5.5 0 0 1 0 1h-13A.5.5 0 0 1 1 8z\" />\r\n </svg>");                                     
            }
            else
            {
                htmlString = new HtmlString("<svg style=\"color: red\" xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-chevron-down\" viewBox=\"0 0 16 16\">\r\n <path fill-rule=\"evenodd\" d=\"M1.646 4.646a.5.5 0 0 1 .708 0L8 10.293l5.646-5.647a.5.5 0 0 1 .708.708l-6 6a.5.5 0 0 1-.708 0l-6-6a.5.5 0 0 1 0-.708z\" />\r\n </svg>");                          
            }

            return htmlString;
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

		public static Tuple<string, string> ObterNomeCasaAbreviado(Instituto casa)
		{
			string nomeCasa = Enum.GetName(typeof(Instituto), casa);

			if (nomeCasa == "CISHO")
			{
				return new Tuple<string, string>("CIS-SO", nomeCasa);
			}
			else if (nomeCasa == "ISIQV")
			{
				return new Tuple<string, string>("ISI-QV", nomeCasa);
			}
			else if (nomeCasa == "ISISVP")
			{
				return new Tuple<string, string>("ISI-SVP", nomeCasa);
			}
			else if (nomeCasa == "ISIII")
			{
				return new Tuple<string, string>("ISI-II", nomeCasa);
			}
			else
			{
				return new Tuple<string, string>(nomeCasa, nomeCasa);
			}
		}
	}

}