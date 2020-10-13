using System.ComponentModel.DataAnnotations;

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
        Materiais,
        [Display(Name = "Otimização de parâmetros de Inspeção não destrutiva em materiais")]
        OtPam,
        [Display(Name = "Sistemas computacionais de Inspeção não destrutiva")]
        SisCompNaoDestrutivas,
        [Display(Name = "Pesquisa na área de corrosão")]
        Corrosao,
        [Display(Name = "Biotecnologia e Biologia Molecular")]
        Biotec,
        [Display(Name = "A definir")]
        Indefinida
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
        NaoConvertida,
        [Display(Name ="Cancelado/Suspenso")]
        Suspensa
    }

    public enum StatusProjeto
    {
        [Display(Name ="Contratrado/Em planejamento")]
        Contratado,
        [Display(Name = "Em execução")]
        EmExecucao,
        Concluido,
        [Display(Name ="Cancelado/Suspenso")]
        Cancelado,
    }

    public enum TipoContratacao
    {
        [Display(Name = "Contratação Direta")]
        ContratacaoDireta,
        Embrapii,
        [Display(Name = "Edital de Inovação SESI/SENAI")]
        EditalInovacao,
        //OutrosEditais,
        [Display(Name = "Agência de Fomento")]
        AgenciaFomento,
        [Display(Name = "ANP/ANEEL")]
        ANP,
        Parceiro,
        [Display(Name ="Projeto Push")]
        Push,
        [Display(Name = "A definir")]
        Indefinida
    }

    public enum TipoInovacao
    {
        Produto,
        Processo
    }

    public enum Estado
    {

        [Display(Name = "Rio de Janeiro")]
        RioDeJaneiro,
        [Display(Name = "São Paulo")]
        SaoPaulo,
        [Display(Name = "Minas Gerais")]
        MinasGerais,
        [Display(Name = "Espírito Santo")]
        EspiritoSanto,
        [Display(Name = "Paraná")]
        Parana,
        [Display(Name = "Santa Catarina")]
        SantaCatarina,
        [Display(Name = "Rio Grande do Sul")]
        RioGrandeDoSul,
        [Display(Name = "Mato Grosso")]
        MatoGrosso,
        [Display(Name = "Mato Grosso do Sul")]
        MatoGrossoDoSul,
        [Display(Name = "Goiás")]
        Goias,
        [Display(Name = "Distrito Federal")]
        DistritoFederal,
        Amazonas,
        [Display(Name = "Pará")]
        Para,
        Roraima,
        Acre,
        [Display(Name = "Rondônia")]
        Rondonia,
        [Display(Name = "Maranhão")]
        Maranhao,
        [Display(Name = "Piauí")]
        Piaui,
        [Display(Name = "Rio Grande do Norte")]
        RioGrandeDoNorte,
        Sergipe,
        Pernambuco,
        [Display(Name = "Paraíba")]
        Paraiba,
        [Display(Name = "Bahia")]
        Bahia,
        [Display(Name = "Tocantins")]
        Tocantins,
        [Display(Name = "Amapá")]
        Amapa,
        [Display(Name = "Ceará")]
        Ceara,
        [Display(Name = "Alagoas")]
        Alagoas,
        [Display(Name = "Fora do país")]
        Estrangeiro
    }

    public enum Casa
    {
        [Display(Name = "ISI - Química Verde")]
        ISIQV,
        [Display(Name = "ISI - Inspeção & Integridade")]
        ISIII,
        [Display(Name = "CIS - Higiene Ocupacional")]
        CISHO,
        [Display(Name ="Supervisão")]
        Super
    }

    public enum MotivosNaoConversao
    {
        [Display(Name="O cliente não aceitou o preço")]
        Preco,
        [Display(Name="O cliente não aceitou o prazo")]
        Prazo,
        [Display(Name="O cliente não pode aceitar por motivos contextuais")]
        Contexto,
        [Display(Name="O cliente não quis informar o motivo")]
        NaoInformada
    }

}
