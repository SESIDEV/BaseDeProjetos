using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class RemoverAssociacaoProjetoIdProjetoIndicadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetoIndicadores_Projeto_IdProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.DropIndex(
                name: "IX_ProjetoIndicadores_IdProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.DropColumn(
                name: "IdProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.AddColumn<string>(
                name: "ProjetoId",
                table: "ProjetoIndicadores",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoIndicadores_ProjetoId",
                table: "ProjetoIndicadores",
                column: "ProjetoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetoIndicadores_Projeto_ProjetoId",
                table: "ProjetoIndicadores",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjetoIndicadores_Projeto_ProjetoId",
                table: "ProjetoIndicadores");

            migrationBuilder.DropIndex(
                name: "IX_ProjetoIndicadores_ProjetoId",
                table: "ProjetoIndicadores");

            migrationBuilder.DropColumn(
                name: "ProjetoId",
                table: "ProjetoIndicadores");

            migrationBuilder.AddColumn<string>(
                name: "IdProjeto",
                table: "ProjetoIndicadores",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoIndicadores_IdProjeto",
                table: "ProjetoIndicadores",
                column: "IdProjeto");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetoIndicadores_Projeto_IdProjeto",
                table: "ProjetoIndicadores",
                column: "IdProjeto",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
