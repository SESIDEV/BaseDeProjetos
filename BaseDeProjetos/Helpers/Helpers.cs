using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Html;

namespace BaseDeProjetos.Helpers
{
    public static class Helpers
    {

        public static decimal CalcularMediaFatoresProjeto(Projeto projeto)
        {
            decimal? custoHH = projeto.ColaboradoresDoProjeto.Sum(usuario => usuario.Cargo.Salario);
            decimal? custoHM = projeto.MaquinasUsadasNoProjeto.Sum(maquina => maquina.CustoHoraMaquina);
            decimal mediaFatores = (decimal)projeto.ValorTotalProjeto + (decimal)custoHH + (decimal)custoHM / 3;
            return mediaFatores;
        }

        public static decimal EncontrarMediaMaximaDeFatoresEmProjeto(List<Projeto> projetos)
        {
            decimal maiorMediaFatores = decimal.MinValue;

            foreach (var projeto in projetos)
            {
                decimal custoTotalProjeto = CalcularMediaFatoresProjeto(projeto);

                if (custoTotalProjeto > maiorMediaFatores)
                {
                    maiorMediaFatores = custoTotalProjeto;
                }
            }

            return maiorMediaFatores;
        }

        public static decimal EncontrarMediaMinimaDeFatoresEmProjeto(List<Projeto> projetos)
        {
            decimal menorMediaFatores = decimal.MaxValue;

            foreach (var projeto in projetos)
            {
                decimal custoTotalProjeto = CalcularMediaFatoresProjeto(projeto);

                if (custoTotalProjeto < menorMediaFatores)
                {
                    menorMediaFatores = custoTotalProjeto;

                    if(menorMediaFatores < 0)
                    {
                        menorMediaFatores = 0;
                    }
                }
            }

            return menorMediaFatores;
        }


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
        public static string PuxarDadosUsuarios(ApplicationDbContext _context){
            
            var usuarios = _context.Users.Where(u => u.Email != null).Select(u => new {u.Email, u.UserName, u.Foto, u.Competencia, u.EmailConfirmed}).ToList();

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