using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class InclusaoDeValorDeSatisfacaoDeProjetoIndicadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ValorSatisfacaoFimProjeto",
                table: "ProjetoIndicadores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ValorSatisfacaoMetadeProjeto",
                table: "ProjetoIndicadores",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorSatisfacaoFimProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.DropColumn(
                name: "ValorSatisfacaoMetadeProjeto",
                table: "ProjetoIndicadores");
        }
    }
}
