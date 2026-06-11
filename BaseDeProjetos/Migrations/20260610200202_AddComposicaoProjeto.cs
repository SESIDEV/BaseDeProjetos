using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AddComposicaoProjeto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PapelNaComposicao",
                table: "Projeto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjetoPaiId",
                table: "Projeto",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ValorParticipacaoComposicao",
                table: "Projeto",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_ProjetoPaiId",
                table: "Projeto",
                column: "ProjetoPaiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_Projeto_ProjetoPaiId",
                table: "Projeto",
                column: "ProjetoPaiId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_Projeto_ProjetoPaiId",
                table: "Projeto");

            migrationBuilder.DropIndex(
                name: "IX_Projeto_ProjetoPaiId",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "PapelNaComposicao",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "ProjetoPaiId",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "ValorParticipacaoComposicao",
                table: "Projeto");
        }
    }
}
