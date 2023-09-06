using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AlgoMaisSync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjetoIndicadores_IdProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoIndicadores_IdProjeto",
                table: "ProjetoIndicadores",
                column: "IdProjeto",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProjetoIndicadores_IdProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoIndicadores_IdProjeto",
                table: "ProjetoIndicadores",
                column: "IdProjeto");
        }
    }
}
