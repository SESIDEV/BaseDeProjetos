using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class MembrosEquipe_Funil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MembrosEquipe",
                table: "Prospeccao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MembrosEquipe",
                table: "Projeto",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembrosEquipe",
                table: "Prospeccao");

            migrationBuilder.DropColumn(
                name: "MembrosEquipe",
                table: "Projeto");
        }
    }
}
