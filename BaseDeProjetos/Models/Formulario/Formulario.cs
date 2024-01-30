using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Formulario
{
    public class Formulario
    {
        public int Id { get; set; }
        [Display(Name = "Título do Formulário")]
        public string TituloFormulario { get; set; }
        [Display(Name = "Descrição do Formulário")]
        public string DescricaoFormulario { get; set; }
        public virtual List<PerguntasForm> Perguntas { get; set; }
        [Display(Name = "Data de Criação")]
        public DateTime? DataCriacao { get; set; }
        public string Identificador { get; set; }
        public virtual List<SubmissaoForm> SubmissoesFormulario { get; set; }
    }
}
