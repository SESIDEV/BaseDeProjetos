using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Html;
using System;
using System.Linq;
using System.Text.Json;

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
        /// Calcula a diferença de meses (ex: Janeiro a Fevereiro), 1 mês de diferença
        /// OBS: Se você precisa da quantidade de meses, some um ao resultado
        /// </summary>
        /// <param name="dataFim"></param>
        /// <param name="dataInicio"></param>
        /// <param name="isInclusivo">Se o retorno deve contar a quantidade de meses, e não apenas retornar a diferença. Padrão: false</param>
        /// <returns></returns>
        public static int DiferencaMeses(DateTime dataFim, DateTime dataInicio, bool isInclusivo = false)
        {
            if (isInclusivo)
            {
                return Math.Abs((dataFim.Month - dataInicio.Month) + 12 * (dataFim.Year - dataInicio.Year)) + 1;
            }
            else
            {
                return Math.Abs((dataFim.Month - dataInicio.Month) + 12 * (dataFim.Year - dataInicio.Year));
            }
        }

        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(dateTime.ToUniversalTime() - epoch).TotalMilliseconds;
        }

        public static int VerificarIntervalo(decimal valor, decimal valorMinimo, decimal valorMaximo)
        {
            decimal primeiroTerco, segundoTerco;
            decimal valorFaixa = (valorMaximo - valorMinimo) / 3;

            primeiroTerco = valorMinimo + valorFaixa;
            segundoTerco = primeiroTerco + valorFaixa;

            if (valor >= valorMinimo && valor <= primeiroTerco)
            {
                return -1;
            }
            else if (valor > primeiroTerco && valor <= segundoTerco)
            {
                return 0;
            }
            else if (valor > segundoTerco && valor <= valorMaximo)
            {
                return 1;
            }
            else
            {
                return int.MaxValue;
            }
        }

        /// <summary>
        /// Dado um ano e um mês, retorna um DateTime com o último dia do mês desse ano
        /// </summary>
        /// <param name="ano"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public static DateTime ObterUltimoDiaMes(int ano, int mes)
        {
            return new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
        }

        public static HtmlString ObterIconeParticipacao(int valorIndicador)
        {
            HtmlString htmlString = new HtmlString($"");

            if (valorIndicador == -1)
            {
                htmlString = new HtmlString("<i style=\"color: red\" class=\"bi bi-chevron-down\"></i>");
            }
            else if (valorIndicador == 0)
            {
                htmlString = new HtmlString("<i style=\"color: blue\" class=\"bi bi-chevron-bar-contract\"></i>");
            }
            else if (valorIndicador == 1)
            {
                htmlString = new HtmlString("<i style=\"color: green\" class=\"bi bi-chevron-up\"></i>");
            }
            else
            {
                htmlString = new HtmlString("<i class=\"bi bi-question-circle-fill\"></i>");
            }

            return htmlString;
        }

        /// <summary>
        /// Retorna Email, Nome de Usuário, Foto e Competência dos usuários com email cadastrado em formato JSON
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        public static string PuxarDadosUsuarios(ApplicationDbContext _context)
        {
            var usuarios = _context.Users.Where(u => u.Email != null).Select(u => new { u.Email, u.UserName, u.Foto, u.Competencia, u.EmailConfirmed }).ToList();

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
            var producoes = _context.Producao.Where(p => p.Titulo != null).Select(p => new { p.Casa, p.Titulo, p.Descricao, p.Autores, p.StatusPub, p.Data, p.Local, p.DOI }).ToList(); //.GetDisplayName() NAO FUNCIONA

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