using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class Casas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Despeita",
                table: "IndicadoresFinanceiros");

            migrationBuilder.AddColumn<int>(
                name: "Casa",
                table: "IndicadoresFinanceiros",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Despesa",
                table: "IndicadoresFinanceiros",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Casa",
                table: "IndicadoresFinanceiros");

            migrationBuilder.DropColumn(
                name: "Despesa",
                table: "IndicadoresFinanceiros");

            migrationBuilder.AddColumn<decimal>(
                name: "Despeita",
                table: "IndicadoresFinanceiros",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
