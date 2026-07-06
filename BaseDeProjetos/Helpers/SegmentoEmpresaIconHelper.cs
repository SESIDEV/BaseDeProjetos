using BaseDeProjetos.Models.Enums;

namespace BaseDeProjetos.Helpers
{
    public static class SegmentoEmpresaIconHelper
    {
        public static string GetIconClass(this SegmentoEmpresa segmento)
        {
            switch (segmento)
            {
                case SegmentoEmpresa.AbastecimentoAgua:
                case SegmentoEmpresa.TratamentAgua:
                case SegmentoEmpresa.TratamentEfluentes:
                case SegmentoEmpresa.Saneamento:
                    return "bi bi-droplet-half";
                case SegmentoEmpresa.Agro:
                case SegmentoEmpresa.ProdutosNaturais:
                case SegmentoEmpresa.Extrativos:
                    return "bi bi-flower1";
                case SegmentoEmpresa.AlimentosBebidas:
                    return "bi bi-cup-straw";
                case SegmentoEmpresa.Automacao:
                case SegmentoEmpresa.Tecnologia:
                case SegmentoEmpresa.Quimica4:
                    return "bi bi-cpu";
                case SegmentoEmpresa.Automobilistica:
                    return "bi bi-truck";
                case SegmentoEmpresa.Biomol:
                case SegmentoEmpresa.Biotecnologia:
                case SegmentoEmpresa.Nanotec:
                    return "bi bi-diagram-3";
                case SegmentoEmpresa.Biorrefinaria:
                case SegmentoEmpresa.Sucroenergetico:
                    return "bi bi-recycle";
                case SegmentoEmpresa.Constutora:
                    return "bi bi-building";
                case SegmentoEmpresa.Cosmeticos:
                case SegmentoEmpresa.Farmaceutica:
                case SegmentoEmpresa.Saude:
                    return "bi bi-heart-pulse";
                case SegmentoEmpresa.Embalagens:
                    return "bi bi-box-seam";
                case SegmentoEmpresa.Energia:
                    return "bi bi-lightning-charge";
                case SegmentoEmpresa.Equipamentos:
                    return "bi bi-tools";
                case SegmentoEmpresa.Formulacoes:
                case SegmentoEmpresa.Quimica:
                    return "bi bi-eyedropper";
                case SegmentoEmpresa.Gases:
                    return "bi bi-cloud";
                case SegmentoEmpresa.Lubrificantes:
                case SegmentoEmpresa.PetroleoGas:
                case SegmentoEmpresa.Petroquimica:
                    return "bi bi-fuel-pump";
                case SegmentoEmpresa.Madeira:
                case SegmentoEmpresa.PapelCelulose:
                    return "bi bi-tree";
                case SegmentoEmpresa.Mantas:
                case SegmentoEmpresa.Tecidos:
                    return "bi bi-layers";
                case SegmentoEmpresa.Materiais:
                case SegmentoEmpresa.Polimeros:
                    return "bi bi-bricks";
                case SegmentoEmpresa.Mineiracao:
                case SegmentoEmpresa.SiderurgiaMetalurgia:
                    return "bi bi-gem";
                case SegmentoEmpresa.ProducaoSal:
                    return "bi bi-grid-3x3-gap";
                case SegmentoEmpresa.Remediacao:
                    return "bi bi-shield-check";
                case SegmentoEmpresa.Tintas:
                    return "bi bi-palette";
                case SegmentoEmpresa.Outros:
                default:
                    return "bi bi-tag";
            }
        }
    }
}
