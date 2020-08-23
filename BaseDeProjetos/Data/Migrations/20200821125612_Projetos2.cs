using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Data.Migrations
{
    public partial class Projetos2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Casa",
                table: "Projeto",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Casa",
                table: "Projeto");
        }
    }
}
