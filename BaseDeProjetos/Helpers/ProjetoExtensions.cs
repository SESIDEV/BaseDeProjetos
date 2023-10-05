using BaseDeProjetos.Models;

public static class ProjetoExtensions
{
    public static ProjetoDTO ToDto(this Projeto projeto)
    {
        return new ProjetoDTO
        {
            Id = projeto.Id,
            NomeProjeto = projeto.NomeProjeto,
            Empresa = projeto.Empresa,
            Proponente = projeto.Proponente,
            AreaPesquisa = projeto.AreaPesquisa,
            DataInicio = projeto.DataInicio,
            DataEncerramento = projeto.DataEncerramento,
            Estado = projeto.Estado,
            FonteFomento = projeto.FonteFomento,
            Inovacao = projeto.Inovacao,
            Status = projeto.Status,
            DuracaoProjetoEmMeses = projeto.DuracaoProjetoEmMeses,
            ValorTotalProjeto = projeto.ValorTotalProjeto,
            ValorAporteRecursos = projeto.ValorAporteRecursos,
            Casa = projeto.Casa,
            Usuario = projeto.Usuario,
            UsuarioId = projeto.UsuarioId,
            StatusCurva = projeto.CurvaFisicoFinanceira,
            SatisfacaoClienteParcial = projeto.SatisfacaoClienteParcial,
            SatisfacaoClienteFinal = projeto.SatisfacaoClienteFinal,
        };
    }
}