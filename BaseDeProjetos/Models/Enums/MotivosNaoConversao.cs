using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum MotivosNaoConversao
    {
        [Display(Name = "O cliente não aceitou o preço")]
        Preco,

        [Display(Name = "O cliente não aceitou o prazo")]
        Prazo,

        [Display(Name = "O cliente não concordou com o escopo")]
        Escopo,

        [Display(Name = "O cliente não pode aceitar por motivos contextuais (orçamento do ano, estratégia empresarial,etc)")]
        Contexto,

        [Display(Name = "O cliente não quis informar o motivo")]
        NaoInformada
    }
}