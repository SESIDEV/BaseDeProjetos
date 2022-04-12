using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class NovasProducoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreasDoConhecimento",
                table: "Producao",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DetalhamentoId",
                table: "Producao",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SetorDeAtividade",
                table: "Producao",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Producao",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Ano",
                table: "DadosBasicos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "FlagRelevancia",
                table: "DadosBasicos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Idioma",
                table: "DadosBasicos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MeioDivulgacao",
                table: "DadosBasicos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Pais",
                table: "DadosBasicos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "DadosBasicos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Detalhamento",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalhamento", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Producao_DetalhamentoId",
                table: "Producao",
                column: "DetalhamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Producao_Detalhamento_DetalhamentoId",
                table: "Producao",
                column: "DetalhamentoId",
                principalTable: "Detalhamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producao_Detalhamento_DetalhamentoId",
                table: "Producao");

            migrationBuilder.DropTable(
                name: "Detalhamento");

            migrationBuilder.DropIndex(
                name: "IX_Producao_DetalhamentoId",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "AreasDoConhecimento",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "DetalhamentoId",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "SetorDeAtividade",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "Ano",
                table: "DadosBasicos");

            migrationBuilder.DropColumn(
                name: "FlagRelevancia",
                table: "DadosBasicos");

            migrationBuilder.DropColumn(
                name: "Idioma",
                table: "DadosBasicos");

            migrationBuilder.DropColumn(
                name: "MeioDivulgacao",
                table: "DadosBasicos");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "DadosBasicos");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "DadosBasicos");
        }
    }
}