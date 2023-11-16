using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseDeProjetos.Models
{
    public class Projeto
    {
        public Projeto()
        {
        }

        public Projeto(ProjetoIndicadores indicador)
        {
            Indicadores = new List<ProjetoIndicadores>() { indicador };
        }

        [Key]
        [Display(AutoGenerateFilter = true)]
        public virtual string Id { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Nome do Projeto")]
        public virtual string NomeProjeto { get; set; }

        public virtual Empresa Empresa { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Linha de Pesquisa")]
        public virtual LinhaPesquisa AreaPesquisa { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Data de Início")]
        public virtual DateTime DataInicio { get; set; } = DateTime.Now;

        [Display(AutoGenerateFilter = true, Name = "Data de Encerramento")]

        public virtual DateTime DataEncerramento { get; set; } = DateTime.Now;

        [Display(AutoGenerateFilter = true, Name = "Membros da Equipe")]
        public virtual List<EquipeProjeto> EquipeProjeto { get; set; }

        public virtual Estado Estado { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Fonte de Fomento")]
        public virtual TipoContratacao FonteFomento { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Inovação")]
        public virtual TipoInovacao Inovacao { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Status do Projeto")]
        public virtual StatusProjeto Status { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Duração do Projeto em meses")]
        public virtual int DuracaoProjetoEmMeses { get; set; } // TODO: Não deveriamos inferir isso a partir da data de inicio e data de fim?

        [Display(AutoGenerateFilter = true, Name = "Valor Total do Projeto")]
        public virtual double ValorTotalProjeto { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Valor de Aporte de Recursos")]
        public virtual double ValorAporteRecursos { get; set; }

        public virtual Instituto Casa { get; set; }

        public virtual List<ProjetoIndicadores> Indicadores { get; set; } = new List<ProjetoIndicadores>();

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        public virtual string UsuarioId { get; set; }

        [ForeignKey(nameof(ProponenteId))]
        public virtual Usuario Proponente { get; set; }

        public virtual string ProponenteId { get; set; }

        public virtual List<CurvaFisicoFinanceira> CurvaFisicoFinanceira { get; set; }

        public virtual double? SatisfacaoClienteParcial { get; set; }

        public virtual double? SatisfacaoClienteFinal { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Custo de Hora Homem")]
        public virtual decimal CustoHH { get; set; }

        [Display(AutoGenerateFilter = true, Name = "Custo de Hora Máquina")]
        public virtual decimal CustoHM { get; set; }

        [Display(Name = "Rúbricas do Projeto")]
        [ForeignKey("ConjuntoRubricasId")]
        public virtual ConjuntoRubrica ConjuntoRubricas { get; set; }

        public virtual int? ConjuntoRubricasId { get; set; }
    }
}