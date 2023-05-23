using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class CnaeIndustrial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Industrial",
                table: "Empresa",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Industrial",
                table: "Empresa");
        }
    }
}
