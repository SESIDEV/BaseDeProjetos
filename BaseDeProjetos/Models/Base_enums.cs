using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BaseDeProjetos.Models
{
    public enum Titulacao
    {
        [Display(Name = "Nível Médio")]
        Medio,
        [Display(Name = "Nível Médio Técnico")]
        Tecnico,
        [Display(Name = "Nível Superior - Não Concluído")]
        Graduando,
        [Display(Name = "Nível Superior - Concluído")]
        Graduado,
        [Display(Name = "Pós-Graduado - Lato Sensu")]
        Especialista,
        [Display(Name = "Pós-Graduado - Stricto Sensu (Mestrado)")]
        Mestre,
        [Display(Name = "Pós-Graduado - Stricto Sensu (Doutorado)")]
        Doutor,
        [Display(Name = "Pós-Graduado - Stricto Sensu (Pós-Doutor)")]
        PosDoutor
    }

    public enum JustificaEPI
    {
        [Display(Name = "Primeira Entrega")]
        PrimeiraEntrega,
        [Display(Name = "Substituição - Troca de rotina ou compulsória, em função da periodicidade")]
        Subs_TrocaCompulsoria,
        [Display(Name = "Necessidade de higienização e/ou manutenção periódica")]
        Subs_HigienizacaoManutencao,
        [Display(Name = "Danificado (sem condições de uso) ou extraviado")]
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
        [Display(Name = "Firjan SESI")]
        FirjanSESI,
        [Display(Name = "Firjan SENAI")]
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

    //TODO: Separar tipos diferentes
    public enum TipoProducao
    {
        //_Tipos_de_Produção_Bibliográfica
        Trabalhos_em_Eventos,
        Artigos_Publicados,
        Livros_e_Capítulos, //Ex.: Livro e Capitulo
        Textos_em_Jornais_ou_Revistas,
        Demais_Tipos_de_Produção_Bibliográfica,
        Artigos_Aceitos_Para_Publicação,
        //Tipos_de_Produção_Técnica
        Cultivo,
        Software,
        Patente,
        Desenho_Industrial,
        Marca,
        Topografia_de_Circuito_Integrado,
        Produto_Tecnológico,
        Processos_ou_Técnicas,
        Trabalho_Técnico,
        Demais_Tipos_de_Produção_Técnica
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

    public enum AreasAtividades
    {
        [Display(Name = "Atividades de apoio")]
        Apoio,
        [Display(Name = "Atividades básicas")]
        Basicas,
        [Display(Name = "Atividades de execução")]
        Execucao,
        [Display(Name = "Atividades de prospecção")]
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
        [Display(Name = "Desenvolvedor")]
        Dev,
        [Display(Name = "Supervisor")]
        Supervisor,
        [Display(Name = "PMO")]
        PMO,
        [Display(Name = "PMO")]
        Usuario
    }

    public enum SetoresDeAtividade
    {
        Administração_pública, _defesa_e_seguridade_social,
        Agências_de_viagens, _operadores_turísticos_e_serviços_de_reservas,
        Agricultura, _Pecuária_e_Serviços_Relacionados,
        Alimentação,
        Alojamento,
        Aluguéis_não_imobiliários_e_gestão_de_ativos_intangíveis_não_financeiros,
        Armazenamento_e_atividades_auxiliares_dos_transportes,
        Atividades_artísticas, _criativas_e_de_espetáculos,
        Atividades_auxiliares_dos_serviços_financeiros_seguros, _previdência_complementar_e_planos_de_saúde,
        Atividades_cinematográficas, _produção_de_vídeos_e_de_programas_de_televisão_gravação_de_som_e_edição_de_música,
        Atividades_de_Apoio_à_Extração_de_Minerais,
        Atividades_de_atenção_à_saúde_humana,
        Atividades_de_atenção_à_saúde_humana_integradas_com_assistência_social, _prestadas_em_residências_coletivas_e_particulares,
        Atividades_de_exploração_de_jogos_de_azar_e_apostas,
        Atividades_de_organizações_associativas,
        Atividades_de_prestação_de_serviços_de_informação,
        Atividades_de_rádio_e_de_televisão,
        Atividades_de_sedes_de_empresas_e_de_consultoria_em_gestão_empresarial,
        Atividades_de_serviços_financeiros,
        Atividades_de_vigilância, _segurança_e_investigação,
        Atividades_dos_serviços_de_tecnologia_da_informação,
        Atividades_esportivas_e_de_recreação_e_lazer,
        Atividades_imobiliárias,
        Atividades_jurídicas, _de_contabilidade_e_de_auditoria,
        Atividades_ligadas_ao_patrimônio_cultural_e_ambiental,
        Atividades_veterinárias,
        Captação, _tratamento_e_distribuição_de_água,
        Coleta, _tratamento_e_disposição_de_resíduos_recuperação_de_materiais,
        Comércio_e_reparação_de_veículos_automotores_e_motocicletas,
        Comércio_por_atacado_exceto_veículos_automotores_e_motocicletas,
        Comércio_varejista,
        Confecção_de_artigos_do_vestuário_e_acessórios,
        Construção_de_edifícios,
        Correio_e_outras_atividades_de_entrega,
        Descontaminação_e_outros_serviços_de_gestão_de_resíduos,
        Edição_e_edição_integrada_à_impressão,
        Educação,
        Eletricidade, _gás_e_outras_utilidades,
        Esgoto_e_atividades_relacionadas,
        Extração_de_Carvão_Mineral,
        Extração_de_Minerais_Metálicos,
        Extração_de_Minerais_Não_Metálicos,
        Extração_de_Petróleo_e_Gás_Natural,
        Fabricação_de_Bebidas,
        Fabricação_de_celulose_papel_e_produtos_de_papel,
        Fabricação_de_coque_de_produtos_derivados_do_petróleo_e_de_biocombustíveis,
        Fabricação_de_equipamentos_de_informática_produtos_eletrônicos_e_ópticos,
        Fabricação_de_máquinas, _aparelhos_e_materiais_elétricos,
        Fabricação_de_máquinas_e_equipamentos,
        Fabricação_de_móveis,
        Fabricação_de_outros_equipamentos_de_transporte_exceto_veículos_automotores,
        Fabricação_de_Produtos_Alimentícios,
        Fabricação_de_produtos_de_borracha_e_de_material_plástico,
        Fabricação_de_produtos_de_madeira,
        Fabricação_de_produtos_de_metal_exceto_máquinas_e_equipamentos,
        Fabricação_de_produtos_de_minerais_não_metálicos,
        Fabricação_de_produtos_diversos,
        Fabricação_de_Produtos_do_Fumo,
        Fabricação_de_produtos_farmoquímicos_e_farmacêuticos,
        Fabricação_de_produtos_químicos,
        Fabricação_de_Produtos_Têxteis,
        Fabricação_de_veículos_automotores, _reboques_e_carrocerias,
        Impressão_e_reprodução_de_gravações,
        Manutenção, _reparação_e_instalação_de_máquinas_e_equipamentos,
        Metalurgia,
        Obras_de_infra_estrutura,
        Organismos_internacionais_e_outras_instituições_extraterritoriais,
        Outras_atividades_de_serviços_pessoais,
        Outras_atividades_profissionais, _científicas_e_técnicas,
        Pesca_e_Aqüicultura,
        Pesquisa_e_desenvolvimento_científico,
        Preparação_de_couros_e_fabricação_de_artefatos_de_couro, _artigos_para_viagem_e_calçados,
        Produção_Florestal,
        Publicidade_e_pesquisa_de_mercado,
        Reparação_e_manutenção_de_equipamentos_de_informática_e_comunicação_e_de_objetos_pessoais_e_domésticos,
        Seguros, _resseguros_previdência_complementar_e_planos_de_saúde,
        Seleção, _agenciamento_e_locação_de_mão_de_obra,
        Serviços_de_arquitetura_e_engenharia_testes_e_análises_técnicas,
        Serviços_de_assistência_social_sem_alojamento,
        Serviços_de_escritório, _de_apoio_administrativo_e_outros_serviços_prestados_às_empresas,
        Serviços_domésticos,
        Serviços_especializados_para_construção,
        Serviços_para_edifícios_e_atividades_paisagísticas,
        Telecomunicações,
        Transporte_aéreo,
        Transporte_aquaviário,
        Transporte_terrestre,
    }

    public enum AreasDoConhecimento
    {
        b
    }

    public enum Pais
    {
        Brasil
    }

    public enum Idioma
    {
        ptBR,
        enUS
    }

    public enum MeioDivulgacao
    {
        //TODO: Capitalizar Fonte
        IMPRESSO, WEB, MEIO_MAGNETICO, MEIO_DIGITAL, FILME, HIPERTEXTO, OUTRO, VARIOS, NAO_INFORMADO
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
