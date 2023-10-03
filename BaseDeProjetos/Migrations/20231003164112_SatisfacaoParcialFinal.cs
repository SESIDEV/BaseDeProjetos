using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class SatisfacaoParcialFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SatisfacaoCliente",
                table: "Projeto");

            migrationBuilder.AddColumn<float>(
                name: "SatisfacaoClienteFinal",
                table: "Projeto",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "SatisfacaoClienteParcial",
                table: "Projeto",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SatisfacaoClienteFinal",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "SatisfacaoClienteParcial",
                table: "Projeto");

            migrationBuilder.AddColumn<float>(
                name: "SatisfacaoCliente",
                table: "Projeto",
                type: "float",
                nullable: true);
        }
    }
}
