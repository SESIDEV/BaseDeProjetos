using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
    public enum Atividades
    {
        //Apoio
        [Display(Name = "Apoio Institucional")]
        ApoioInstitucional,

        //Atividades Básicas
        [Display(Name = "Treinamento e Autodesenvolvimento")]
        TreinamentoEAutodesenvolvimento,

        [Display(Name = "Atividade Meio")]
        AtividadeMeio,

        [Display(Name = "Reunião de grupo")]
        ReuniaoDeGrupo,

        //Atividades de execução
        [Display(Name = "Atividade de Follow-up")]
        FollowUp,

        [Display(Name = "Atividade Experimental")]
        Experimentais,

        [Display(Name = "Atividade Administrativa")]
        Administrativas,

        //Atividades de prospecção
        [Display(Name = "Elaboração/Prospecção de Projeto Push")]
        AtividadePush,

        [Display(Name = "Relacionamento com empresa")]
        RelacionamentoComEmpresa,

        [Display(Name = "Elaboração de proposta")]
        ElaboracaoProposta,

        [Display(Name = "Participação em eventos")]
        ParticipacaoEmEventos,

        //Outros
        Outros
    }
}