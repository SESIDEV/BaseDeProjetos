using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class RemoverMembrosEquipeDeProspeccao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembrosEquipe",
                table: "Prospeccao");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MembrosEquipe",
                table: "Prospeccao",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
