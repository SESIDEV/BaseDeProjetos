using BaseDeProjetos.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Formulario
{
    public class PerguntasForm
    {
        public int Id { get; set; }
        [Display(Name = "Título da Pergunta")]
        public string TituloPergunta { get; set; }
        [Display(Name = "Tipo da Questão")]
        public TipoResposta Tipo { get; set; }
        public bool Obrigatorio { get; set; }
    }
}
