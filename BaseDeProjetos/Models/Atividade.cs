using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Models
{
    public class AtividadesProdutivas
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual Atividades Atividade { get; set; }
        public virtual AreasAtividades AreaAtividade { get; set; }
        public virtual TipoContratacao FonteFomento { get; set; }
        public virtual string ProjetoId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual string DescricaoAtividade { get; set; }
        public virtual double CargaHoraria { get; set; }
        public virtual DateTime Data { get; set; }

    }
}
