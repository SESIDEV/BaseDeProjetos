using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseDeProjetos.Helpers
{
    public class Pager
    {
        public int ItensTotais { get; private set; }
        public int PaginaAtual { get; private set; }
        public int TamanhoDaPagina { get; private set; }
        public int TotalPaginas { get; private set; }

        public List<int> Pages { get; set; }

        /// <summary>
        /// Construtor para o Pager (Paginação)
        /// </summary>
        /// <param name="itensTotais">Quantidade de items totais numa página</param>
        /// <param name="paginaAtual">Número da página atual</param>
        /// <param name="tamanhoDaPagina">Quantidade máxima de itens numa página</param>
        /// <param name="maximoDePaginas">Número máximo de páginas</param>
        public Pager(int itensTotais, int paginaAtual, int tamanhoDaPagina, int maximoDePaginas)
        {
            ItensTotais = itensTotais;
            PaginaAtual = paginaAtual;
            TamanhoDaPagina = tamanhoDaPagina;

            // Numero de paginas
            TotalPaginas = (int)Math.Ceiling((double)ItensTotais / TamanhoDaPagina);

            PaginaAtual = Math.Max(1, Math.Min(PaginaAtual, TotalPaginas));

            int paginaInicial = 1;
            int paginaFinal = TotalPaginas;

            if (TotalPaginas > maximoDePaginas)
            {
                int maxPagesBeforeCurrentPage = (int)Math.Floor((double)maximoDePaginas / 2);
                int maxPagesAfterCurrentPage = (int)Math.Ceiling((double)maximoDePaginas / 2) - 1;
                if (PaginaAtual <= maxPagesBeforeCurrentPage)
                {
                    paginaFinal = maximoDePaginas;
                }
                else if (PaginaAtual >= TotalPaginas - maxPagesAfterCurrentPage)
                {
                    paginaInicial = TotalPaginas - maximoDePaginas + 1;
                }
                else
                {
                    paginaInicial = PaginaAtual - maxPagesBeforeCurrentPage;
                    paginaFinal = PaginaAtual + maxPagesAfterCurrentPage;
                }
            }

            Pages = Enumerable.Range(paginaInicial, paginaFinal - paginaInicial + 1).ToList();
        }

        public bool TemPaginaAnterior => PaginaAtual > 1;
        public bool TemProximaPagina => PaginaAtual < TotalPaginas;
    }
}
