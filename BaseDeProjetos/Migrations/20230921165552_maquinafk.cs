using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class maquinafk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adicionado manualmente favor não remover
            migrationBuilder.AddColumn<int>(
                name: "ProjetoId",
                table: "Maquina",
                nullable: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_Maquina_ProjetoId",
                table: "Maquina",
                column: "ProjetoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Maquina_ProjetoId",
                table: "Maquina");

            migrationBuilder.DropColumn(
                name: "ProjetoId",
                table: "Maquina");

        }
    }
}
