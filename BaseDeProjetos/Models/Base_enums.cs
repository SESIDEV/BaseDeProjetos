using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Models
{
    public enum LinhaPesquisa
    {
        [Display(Name = "Química e Sustentabilidade")]
        QuimicaESustentabilidade,
        [Display(Name = "Tecnologia para Solos e Efluentes")]
        TecnologiasSolosEfluentes,
        [Display(Name = "Extração de Matérias Primas Renováveis")]
        ExtracaoMateriasPrimasRenovaveis,
        [Display(Name = "Química Analítica Verde")]
        QuimicaAnaliticaVerde,
        [Display(Name = "Petróleo e Petroquímica")]
        PetroleoEPetroQuimica,
        [Display(Name = "Saúde e Segurança do Trabalho")]
        SST,
        [Display(Name = "Indústria 4.0")]
        Industria40,
        [Display(Name = "Novos Materiais")]
        Materiais
    }

    public enum StatusProspeccao
    {
        [Display(Name = "Contato inicial")]
        ContatoInicial, 
        [Display(Name = "Em discussão: Busca de Literatura")]
        Discussao_BuscaLiteratura,
        [Display(Name = "Em discussão: Draft de Ideias")]
        Discussao_DraftIdeias, 
        [Display(Name = "Em discussão: Definição de Desenho Experimental")]
        Discussao_DesenhoExperimental,
        [Display(Name = "Em discussão: Esboço do Projeto")]
        Discussao_EsbocoProjeto,
        [Display(Name = "Com Proposta")]
        ComProposta,
        [Display(Name = "Convertida")]
        Convertida,
        [Display(Name = "Não Convertida")]
        NaoConvertida
    }

    public enum StatusProjeto
    {
        Contratado,
        [Display(Name = "Em execução")]
        EmExecucao,
        Concluido,
        Cancelado, 
        Suspenso
    }

    public enum TipoContratacao
    {
        [Display(Name = "Contratação Direta")]
        ContratacaoDireta, 
        Embrapii, 
        [Display(Name = "Edital de Inovação")]
        EditalInovacao, 
        [Display(Name = "Agência de Fomento")]
        AgenciaFomento, 
        ANP,
        Parceiro
    }

    public enum TipoInovacao
    {
        Produto,
        Processo
    }

}
