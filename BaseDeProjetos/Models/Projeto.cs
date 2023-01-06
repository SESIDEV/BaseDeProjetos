using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BaseDeProjetos.Models
{
    public class Projeto
    {
        [Key]
        [Display(AutoGenerateFilter = true)]
        public virtual string Id { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Nome do Projeto")]
        public virtual string NomeProjeto { get; set; }

        public virtual Empresa Empresa { get; set; }
        public virtual Empresa Proponente { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Linha de Pesquisa")]
        public virtual LinhaPesquisa AreaPesquisa { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Data de Início")]
        public virtual DateTime DataInicio { get; set; }

        public virtual DateTime DataEncerramento { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Membros da Equipe")]
        public virtual string MembrosEquipe { get; set; }

        public virtual Estado Estado { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Fonte de Fomento")]
        public virtual TipoContratacao FonteFomento { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Inovação")]
        public virtual TipoInovacao Inovacao { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Status do Projeto")]
        public virtual StatusProjeto Status { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Duração do Projeto em meses")]
        public virtual int DuracaoProjetoEmMeses { get; set; }

        public virtual double ValorTotalProjeto { get; set; }

        public virtual double ValorAporteRecursos { get; set; }

        public virtual Instituto Casa { get; set; }
    }
}