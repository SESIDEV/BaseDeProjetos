using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseDeProjetos.Models
{
    public enum LinhaPesquisa
    {
        QuimicaESustentabilidade,
        TecnologiasSolosEfluentes,
        ExtracaoMateriasPrimasRenovaveis,
        QuimicaAnaliticaVerde,
        PetroleoEPetroQuimica,
        SST,
        InteligenciaArtifical,
        Materiais
    }

    public enum StatusProspeccao
    {
        ContatoInicial, 
        Discussao_BuscaLiteratura,
        Discussao_DraftIdeias, 
        Discussao_DesenhoExperimental,
        Discussao_EsbocoProjeto,
        ComProposta,
        Convertida,
        NaoConvertida
    }

    public enum TipoContratacao
    {
        ContratacaoDireta, 
        Embrapii, 
        EditalInovacao, 
        AgenciaFomento, 
        ANP, 
    }

}
