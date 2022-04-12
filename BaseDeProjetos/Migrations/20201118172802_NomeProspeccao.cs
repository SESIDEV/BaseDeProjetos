using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class NomeProspeccao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeProspeccao",
                table: "Prospeccao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PotenciaisParceiros",
                table: "Prospeccao",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeProspeccao",
                table: "Prospeccao");

            migrationBuilder.DropColumn(
                name: "PotenciaisParceiros",
                table: "Prospeccao");
        }
    }
}