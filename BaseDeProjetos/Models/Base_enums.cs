using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BaseDeProjetos.Models
{
    public enum SegmentoEmpresa
    {

        [Display(Name = "Abastecimento de água")]
        AbastecimentoAgua,

        [Display(Name = "Agro")]
        Agro,

        [Display(Name = "Alimentos e Bebidas")]
        AlimentosBebidas,

        [Display(Name = "Automação")]
        Automacao,

        [Display(Name = "Automobilística")]
        Automobilistica,

        [Display(Name = "Biomol")]
        Biomol,

        [Display(Name = "Biorrefinaria")]
        Biorrefinaria,

        [Display(Name = "Biotecnologia")]
        Biotecnologia,

        [Display(Name = "Construtora")]
        Constutora,

        [Display(Name = "Cosméticos")]
        Cosmeticos,

        [Display(Name = "Embalagens")]
        Embalagens,

        [Display(Name = "Energia")]
        Energia,

        [Display(Name = "Equipamentos")]
        Equipamentos,

        [Display(Name = "Extrativos naturais")]
        Extrativos,

        [Display(Name = "Farmacêutica")]
        Farmaceutica,

        [Display(Name = "Formulações")]
        Formulacoes,

        [Display(Name = "Gases")]
        Gases,

        [Display(Name = "Lubrificantes")]
        Lubrificantes,

        [Display(Name = "Madeira")]
        Madeira,

        [Display(Name = "Mantas absorvedoras")]
        Mantas,

        [Display(Name = "Materiais")]
        Materiais,

        [Display(Name = "Mineiração")]
        Mineiracao,

        [Display(Name = "Nanotecnologia")]
        Nanotec,

        [Display(Name = "Outros")]
        Outros,

        [Display(Name = "Papel e celulose")]
        PapelCelulose,

        [Display(Name = "Petróleo e Gás")]
        PetroleoGas,

        [Display(Name = "Petroquímica")]
        Petroquimica,

        [Display(Name = "Polímeros")]
        Polimeros,

        [Display(Name = "Produção de Sal")]
        ProducaoSal,

        [Display(Name = "Produtos naturais")]
        ProdutosNaturais,

        [Display(Name = "Química")]
        Quimica,

        [Display(Name = "Química 4.0")]
        Quimica4,

        [Display(Name = "Remediação")]
        Remediacao,

        [Display(Name = "Saneamento")]
        Saneamento,

        [Display(Name = "Saúde")]
        Saude,

        [Display(Name = "Siderurgia/Metalurgia")]
        SiderurgiaMetalurgia,

        [Display(Name = "Sucroenergético")]
        Sucroenergetico,

        [Display(Name = "Tecidos")]
        Tecidos,

        [Display(Name = "Tecnologia")]
        Tecnologia,

        [Display(Name = "Tintas")]
        Tintas,

        [Display(Name = "Tratamento de água")]
        TratamentAgua,

        [Display(Name = "Tratamento de Efluentes")]
        TratamentEfluentes,
    }
    public enum GrupoPublicacao
    {
        [Display(Name = "Trabalho em Eventos", GroupName = "Produção Bibliográfica")]
        Trabalho,

        [Display(Name = "Artigo Publicado", GroupName = "Produção Bibliográfica")]
        Artigo,

        [Display(Name = "Livro Publicado ou Organizado", GroupName = "Produção Bibliográfica")]
        Livro,

        [Display(Name = "Capitulo de Livro Publicado", GroupName = "Produção Bibliográfica")]
        Capitulo,

        [Display(Name = "Texto em Jornal ou Revista", GroupName = "Produção Bibliográfica")]
        JornalRevista,

        [Display(Name = "Prefácio/Posfácio", GroupName = "Produção Bibliográfica")]
        PrefacioPosfacio,

        [Display(Name = "Palestras em Eventos", GroupName = "Produção Bibliográfica")]
        Palestra,

        [Display(Name = "Relatório Técnico", GroupName = "Produção Técnica, Patentes e Registros")]
        Relatorio,

        [Display(Name = "Patente", GroupName = "Produção Técnica, Patentes e Registros")]
        Patente = 8,

    }
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

    public enum Competencia
    {
        [Display(Name = "...")]
        Zero = 0,
        [Display(Name = "HTML/CSS")]
        HTMLCSS,
        [Display(Name = "Javascript")]
        JS,
        [Display(Name = "Python")]
        Python,
        [Display(Name = "MySQL")]
        MYSQL,
        [Display(Name = "C#")]
        CSharp,
        [Display(Name = "C++")]
        Cmm,
        [Display(Name = "Photoshop")]
        Photoshop,
        [Display(Name = "Excel")]
        Excel,

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
        [Display(Name = "QV - Quimica e Sustentabilidade", GroupName = "Quimica Verde")]
        QuimicaESustentabilidade,
        [Display(Name = "QV - Tecnologia para Solos e Efluentes", GroupName = "Quimica Verde")]
        TecnologiasSolosEfluentes,
        [Display(Name = "QV - Extraçao de Materias Primas Renovaveis", GroupName = "Quimica Verde")]
        ExtracaoMateriasPrimasRenovaveis,
        [Display(Name = "QV - Quimica Analítica Verde", GroupName = "Quimica Verde")]
        QuimicaAnaliticaVerde,
        [Display(Name = "QV - Petroleo e Petroquimica", GroupName = "Quimica Verde")]
        PetroleoEPetroQuimica,
        [Display(Name = "CIS - Saude e Segurança do Trabalho", GroupName = "Centro de Inovação Sesi - Saúde Ocupacional")]
        SST,
        [Display(Name = "II - Industria 4.0", GroupName = "Inspeção e Integridade")]
        Industria40,
        [Display(Name = "II - Novos Materiais", GroupName = "Inspeção e Integridade")]
        Materiais,
        [Display(Name = "II - Otimizaçao de parametros de Inspeçao nao destrutiva em materiais", GroupName = "Inspeção e Integridade")]
        OtPam,
        [Display(Name = "II - Sistemas computacionais de Inspeçao nao destrutiva", GroupName = "Inspeção e Integridade")]
        SisCompNaoDestrutivas,
        [Display(Name = "II - Pesquisa na área de corrosao", GroupName = "Inspeção e Integridade")]
        Corrosao,
        [Display(Name = "QV - Biotecnologia e Biologia Molecular", GroupName = "Quimica Verde")]
        Biotec,
        [Display(Name = "A definir")]
        Indefinida,
        [Display(Name = "SVP - Sistemas interativos inteligentes em realidade estendida", GroupName = "Sistemas Virtuais de Produção")]
        SistemasInterativosRealidadeEstendida,
        [Display(Name = "SVP - Modelagem matematica para realidade estendida", GroupName = "Sistemas Virtuais de Produção")]
        ModelagemMatematicaRealidadeEstendida,
        [Display(Name = "SVP - Simuladores hibridos", GroupName = "Sistemas Virtuais de Produção")]
        SimuladoresHibridos,
        [Display(Name = "QV - Quimica 4.0", GroupName = "Quimica Verde")]
        Quimica40,
        [Display(Name = "CIS - Biotecnologia Aplicada", GroupName = "Centro de Inovação Sesi - Saúde Ocupacional")]
        BiotecCIS,
        [Display(Name = "CIS - Suporte Científico para Processos e Produtos em Saude", GroupName = "Centro de Inovação Sesi - Saúde Ocupacional")]
        SuporteProcessosSaude,

    }
    public enum StatusProspeccao
    {
        [Display(Name = "Contato inicial")]
        ContatoInicial = 0,

        [Display(Name = "Em discussão")]
        Discussao_EsbocoProjeto = 4,

        [Display(Name = "Com Proposta")]
        ComProposta = 5,

        [Display(Name = "Convertida")]
        Convertida = 6,

        [Display(Name = "Não Convertida")]
        NaoConvertida = 7,

        [Display(Name = "Cancelado/Suspenso")]
        Suspensa = 8,

        [Display(Name = "Planejada")]
        Planejada = 9,

        [Display(Name = "Agregada")]
        Agregada = 10
    }
    public enum StatusProjeto
    {
        [Display(Name = "Contratado/Em planejamento")]
        Contratado,

        [Display(Name = "Em execução")]
        EmExecucao,

        [Display(Name = "Concluído")]
        Concluido,

        [Display(Name = "Cancelado/Suspenso")]
        Cancelado,

        [Display(Name = "Atrasado")]
        Atrasado,
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
    public enum StatusPub
    {
        [Display(Name = "Submetido", GroupName = "Artigo")]
        Submetido,
        [Display(Name = "Aceito", GroupName = "Artigo")]
        Aceito,
        [Display(Name = "Publicado", GroupName = "Artigo")]
        Publicado,

        [Display(Name = "Busca de Anterioridade", GroupName = "Patente")]
        BuscaAnterioridade,

        [Display(Name = "Em Depósito", GroupName = "Patente")]
        EmDeposito,

        [Display(Name = "Depositada", GroupName = "Patente")]
        Depositada,

        [Display(Name = "Concedida", GroupName = "Patente")]
        Concedida = 5,


    }

    public enum Origem
    {
        [Display(Name = "Recebida")]
        Recebida,

        [Display(Name = "Iniciada")]
        Iniciada
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
        Super,

        [Display(Name = "ISI - Sistemas Virtuais de Produção")]
        ISISVP
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

        [Display(Name = "O cliente não concordou com o escopo")]
        Escopo,

        [Display(Name = "O cliente não pode aceitar por motivos contextuais (orçamento do ano, estratégia empresarial,etc)")]
        Contexto,

        [Display(Name = "O cliente não quis informar o motivo")]
        NaoInformada
    }
    public enum Nivel
    {
        [Display(Name = "Usuário")]
        Usuario,

        [Display(Name = "Supervisor")]
        Supervisor,

        [Display(Name = "PMO")]
        PMO,

        [Display(Name = "Desenvolvedor")]
        Dev,

        [Display(Name = "Externos")]
        Externos
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
    public enum Pais
    {
        [Display(Name = "Brasil")]
        Brasil,
        [Display(Name = "Exterior")]
        Exterior
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
    public enum StatusSubmissaoEdital
    {
        [Display(Name = "Submissão de edital")]
        submetido,
        [Display(Name = "Submissão em análise")]
        emAnalise,
        [Display(Name = "Deferido")]
        deferido,
        [Display(Name = "Indeferido")]
        indeferido,
        [Display(Name = "Cancelado")]
        cancelado
    }
    public enum StatusEdital
    {
        [Display(Name = "Em aberto")]
        aberto,
        [Display(Name = "Edital Encerrado")]
        encerrado
    }
    public enum AgenciaDeFomento
    {
        [Display(Name = "Finep")]
        finep,
        [Display(Name = "Faperj")]
        faperj,
        [Display(Name = "Cnpq")]
        cnpq,
        [Display(Name = "Sesi/Senai de Inovação")]
        sesi_senai_inovacao,
        [Display(Name = "Outros")]
        outros
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
            return field?.GetCustomAttribute<DisplayAttribute>();
        }
    }
}