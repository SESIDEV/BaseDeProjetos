using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Models
{
    public class Projeto
    {
        [Key]
        [Display(AutoGenerateFilter =true)]
        public string Id { get; set; }
        [Display(AutoGenerateFilter =true, Name ="Nome do Projeto")]
        public string NomeProjeto { get; set; }
        public Empresa Empresa { get; set; }
        public Empresa Proponente { get; set; }
        [Display(AutoGenerateFilter =true, Name ="Linha de Pesquisa")]
        public LinhaPesquisa AreaPesquisa { get; set; }
        [Display(AutoGenerateFilter =true, Name ="Data de Início")]
        public DateTime DataInicio { get; set; }
        public DateTime DataEncerramento { get; set; }
        public List<Usuario> Equipe { get; set; } = new List<Usuario>();
        public Usuario Lider
        {
            get
            {
                if (Equipe.Count >= 1)
                {
                    return this.Equipe.First();
                }
                else
                {
                    return new Usuario { Id = "Teste"};
                    //throw new InvalidOperationException("A equipe não foi definida");
                }
            }
            private set { }
        }

        public string Estado { get; set; }
        [Display(AutoGenerateFilter =true, Name ="Fonte de Fomento")]
        public TipoContratacao FonteFomento { get; set; }
        [Display(AutoGenerateFilter =true, Name ="Inovação")]
        public TipoInovacao Inovacao { get; set; }
        [Display(AutoGenerateFilter =true, Name ="Status do Projeto")]
        public StatusProjeto status { get; set; }
        [Display(AutoGenerateFilter =true, Name ="Duração do Projeto")]
        public int DuracaoProjetoEmMeses { get; set; }
        public double ValorTotalProjeto { get; set; }
        public double ValorAporteRecursos { get; set; }
    }
}
