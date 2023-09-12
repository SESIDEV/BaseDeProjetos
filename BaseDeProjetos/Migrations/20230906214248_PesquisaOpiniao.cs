using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class PesquisaOpiniao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorSatisfacaoFimProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.DropColumn(
                name: "ValorSatisfacaoMetadeProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.CreateTable(
                name: "PesquisaProjeto",
                columns: table => new
                {
                    IdPesquisa = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<string>(nullable: true),
                    ResultadoFinal = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PesquisaProjeto", x => x.IdPesquisa);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PesquisaProjeto");

            migrationBuilder.AddColumn<int>(
                name: "ValorSatisfacaoFimProjeto",
                table: "ProjetoIndicadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ValorSatisfacaoMetadeProjeto",
                table: "ProjetoIndicadores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
