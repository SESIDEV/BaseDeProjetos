using BaseDeProjetos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Html;

namespace BaseDeProjetos.Helpers
{
    public static class FunilHelpers
    {
        public static HtmlString VerificarTemperatura(int qtdDias)
        {
            if (qtdDias < 7)
            {
                return new HtmlString($"<span class='badge badge-funil badge-quente text-dark'>Quente ({qtdDias} Dias)</span>");
            }
            else if (qtdDias >= 7 && qtdDias <= 15)
            {
                return new HtmlString($"<span class='badge badge-funil badge-morno text-dark'>Morno ({qtdDias} Dias)</span>");
            }
            else if (qtdDias >= 16 && qtdDias <= 30)
            {
                return new HtmlString($"<span class='badge badge-funil badge-esfriando text-dark'>Esfriando ({qtdDias} Dias)</span>");
            }
            else
            {
                return new HtmlString($"<span class='badge badge-funil badge-frio text-dark'>Frio ({qtdDias} Dias)</span>");
            }
        }
        public static bool ProspeccaoExists(string id, ApplicationDbContext _context)
        {
            return _context.Prospeccao.Any(e => e.Id == id);
        }
        public static List<Prospeccao> VincularCasaProspeccao(Usuario usuario, List<Prospeccao> listaProsp)
        {

            if (usuario.Casa == Instituto.Super || usuario.Casa == Instituto.ISIQV || usuario.Casa == Instituto.CISHO)
            {
                return listaProsp.Where(p => p.Casa == Instituto.ISIQV || p.Casa == Instituto.CISHO).ToList();

            }
            else
            {
                return listaProsp.Where(p => p.Casa == usuario.Casa).ToList();
            }

        }
        public static List<Prospeccao> PeriodizarProspecções(string ano, List<Prospeccao> lista)
        {
            if (ano.Equals("Todos") || string.IsNullOrEmpty(ano))
            {
                return lista;
            }

            return lista.Where(s => s.Status.Any(k => k.Data.Year == Convert.ToInt32(ano))).ToList();
        }
        public static List<Prospeccao> OrdenarProspecções(string sortOrder, List<Prospeccao> lista)
        {
            var prosps = lista.AsQueryable<Prospeccao>();
            prosps = sortOrder switch
            {
                "name_desc" => prosps.OrderByDescending(s => s.Empresa.Nome),
                "TipoContratacao" => prosps.OrderBy(s => s.TipoContratacao),
                "tipo_desc" => prosps.OrderByDescending(s => s.TipoContratacao),
                _ => prosps.OrderBy(s => s.Status.OrderBy(k => k.Data).Last().Data),
            };
            return prosps.ToList();
        }
        public static List<Prospeccao> FiltrarProspecções(string searchString, List<Prospeccao> lista)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                lista = lista.Where(s => s.Empresa.Nome.ToLower().Contains(searchString) || s.Usuario.UserName.ToLower().Contains(searchString)).ToList();
            }

            return lista;
        }
        public static bool VerificarContratacaoDireta(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.ContratacaoDireta;
        }
        public static bool VerificarContratacaoEditalInovacao(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.EditalInovacao;
        }
        public static bool VerificarContratacaoAgenciaFomento(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.AgenciaFomento;
        }
        public static bool VerificarContratacaoEmbrapii(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.Embrapii;
        }
        public static bool VerificarContratacaoIndefinida(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.Indefinida;
        }
        public static bool VerificarContratacaoParceiro(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.Parceiro;
        }
        public static bool VerificarContratacaoANP(Prospeccao p)
        {
            return p.TipoContratacao == TipoContratacao.ANP;
        }
        public static bool VerificarStatusContatoInicial(Prospeccao p)
        {
            if (p.Status.Count == 0)
            {
                return false;
            }
            return p.Status.Any(s => s.Status == StatusProspeccao.ContatoInicial);
        }
        public static bool VerificarStatusEmDiscussao(Prospeccao p)
        {
            if (p.Status.Count == 0)
            {
                return false;
            }
            return p.Status.Any(k => k.Status < StatusProspeccao.ComProposta && k.Status > StatusProspeccao.ContatoInicial);
        }
        public static bool VerificarStatusComProposta(Prospeccao p)
        {
            if (p.Status.Count == 0)
            {
                return false;
            }
            return p.Status.Any(s => s.Status == StatusProspeccao.ComProposta);
        }
    }

}