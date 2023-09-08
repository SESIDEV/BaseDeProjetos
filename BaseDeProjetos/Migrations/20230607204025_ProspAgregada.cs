using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class ProspAgregada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Agregadas",
                table: "Prospeccao",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ancora",
                table: "Prospeccao",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agregadas",
                table: "Prospeccao");

            migrationBuilder.DropColumn(
                name: "Ancora",
                table: "Prospeccao");
        }
    }
}
