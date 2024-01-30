using System;
using System.Collections.Generic;

namespace BaseDeProjetos.Models.Formulario
{
    public class SubmissaoForm
    {
        public int Id {  get; set; }
        public string NomeProjeto { get; set; }
        public string EmailRemetente { get; set; }
        public DateTime DataSubmissao { get; set; }
        public string Respostas { get; set; }
    }
}
