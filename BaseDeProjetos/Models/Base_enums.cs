using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BaseDeProjetos.Models
{
    public enum Titulacao
    {
        [Display(Name ="Nível Médio")]
        Medio,
        [Display(Name ="Nível Médio Técnico")]
        Tecnico,
        [Display(Name ="Nível Superior - Não Concluído")]
        Graduando,
        [Display(Name ="Nível Superior - Concluído")]
        Graduado,
        [Display(Name ="Pós-Graduado - Lato Sensu")]
        Especialista,
        [Display(Name ="Pós-Graduado - Stricto Sensu (Mestrado)")]
        Mestre,
        [Display(Name ="Pós-Graduado - Stricto Sensu (Doutorado)")]
        Doutor,
        [Display(Name ="Pós-Graduado - Stricto Sensu (Pós-Doutor)")]
        PosDoutor
    }

    public enum JustificaEPI
    {
        [Display(Name ="Primeira Entrega")]
        PrimeiraEntrega,
        [Display(Name ="Substituição - Troca de rotina ou compulsória, em função da periodicidade")]
        Subs_TrocaCompulsoria,
        [Display(Name ="Necessidade de higienização e/ou manutenção periódica")]
        Subs_HigienizacaoManutencao,
        [Display(Name ="Danificado (sem condições de uso) ou extraviado")]
        Subs_DanificadoExtraviado
    }

    public enum TipoVinculo
    {
        Visitante, 
        Aluno,
        JovemAprendiz,
        Estagiario,
        Empregado
    }

    public enum CasaFirjan
    {
         Firjan,
         [Display(Name="Firjan SESI")]
         FirjanSESI,
         [Display(Name="Firjan SENAI")]
         FirjanSENAI,
    }

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
        [Display(Name = "Cancelado/Suspenso")]
        Suspensa
    }

    public enum StatusProjeto
    {
        [Display(Name = "Contratrado/Em planejamento")]
        Contratado,
        [Display(Name = "Em execução")]
        EmExecucao,
        Concluido,
        [Display(Name = "Cancelado/Suspenso")]
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
        [Display(Name = "Projeto Push")]
        Push,
        [Display(Name = "A definir")]
        Indefinida
    }

    public enum TipoInovacao
    {
        Produto,
        Processo
    }

    public enum TipoProducao
    {
        Palestra,
        Artigo,
        Patente,
        [Display(Name = "Segredo Industrial")]
        SegredoIndustrial,
        Evento,
        Noticia,
        Dica
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

    public enum Instituto
    {
        [Display(Name = "ISI - Química Verde")]
        ISIQV,
        [Display(Name = "ISI - Inspeção & Integridade")]
        ISIII,
        [Display(Name = "CIS - Higiene Ocupacional")]
        CISHO,
        [Display(Name = "Supervisão")]
        Super
    }

    public enum Atividades
    {
        //Apoio
        [Display(Name ="Apoio Institucional")]
        ApoioInstitucional,
        //Atividades Básicas
        [Display(Name ="Treinamento e Autodesenvolvimento")]
        TreinamentoEAutodesenvolvimento, 
        [Display(Name ="Atividade Meio")]
        AtividadeMeio,
        [Display(Name ="Reunião de grupo")]
        ReuniaoDeGrupo,
        //Atividades de execução
        [Display(Name ="Atividade de Follow-up")]
        FollowUp,
        [Display(Name ="Atividade Experimental")]
        Experimentais,
        [Display(Name ="Atividade Administrativa")]
        Administrativas,
        //Atividades de prospecção
        [Display(Name ="Elaboração/Prospecção de Projeto Push")]
        AtividadePush,
        [Display(Name ="Relacionamento com empresa")]
        RelacionamentoComEmpresa,
        [Display(Name ="Elaboração de proposta")]
        ElaboracaoProposta,
        [Display(Name ="Participação em eventos")]
        ParticipacaoEmEventos,
        //Outros
        Outros
    }

    public enum AreasAtividades
    {
        [Display(Name ="Atividades de apoio")]
        Apoio,
        [Display(Name ="Atividades básicas")]
        Basicas,
        [Display(Name ="Atividades de execução")]
        Execucao,
        [Display(Name ="Atividades de prospecção")]
        Prospeccao,
        Outros
    }

    public enum MotivosNaoConversao
    {
        [Display(Name = "O cliente não aceitou o preço")]
        Preco,
        [Display(Name = "O cliente não aceitou o prazo")]
        Prazo,
        [Display(Name = "O cliente não pode aceitar por motivos contextuais")]
        Contexto,
        [Display(Name = "O cliente não quis informar o motivo")]
        NaoInformada
    }

    public enum Nivel
    {
        [Display(Name ="Desenvolvedor")]
        Dev,
        [Display(Name ="Supervisor")]
        Supervisor,
        [Display(Name ="PMO")]
        PMO,
        [Display(Name ="PMO")]
        Usuario
    }

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enu)
        {
            DisplayAttribute attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Name : enu.ToString();
        }

        public static string GetDescription(this Enum enu)
        {
            DisplayAttribute attr = GetDisplayAttribute(enu);
            return attr != null ? attr.Description : enu.ToString();
        }

        private static DisplayAttribute GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Type {0} is not an enum", type));
            }

            // Get the enum field.
            FieldInfo field = type.GetField(value.ToString());
            return field == null ? null : field.GetCustomAttribute<DisplayAttribute>();
        }
    }
}
