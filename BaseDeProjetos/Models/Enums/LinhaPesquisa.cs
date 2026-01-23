using System.ComponentModel.DataAnnotations;

namespace BaseDeProjetos.Models.Enums
{
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

        [Display(Name = "CIS - Desenvolvimento e Otimização de Marcadores Biológicos e Moleculares", GroupName = "Centro de Inovação Sesi - Saúde Ocupacional")]
        MarcadoresBiologicos,

        [Display(Name = "CIS - Tecnologias Digitais para Saúde e Segurança Ocupacional", GroupName = "Centro de Inovação Sesi - Saúde Ocupacional")]
        TecnologiasDigitais,

        [Display(Name = "A definir")]
        Indefinida,
    }
}