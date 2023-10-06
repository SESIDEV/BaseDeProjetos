using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class cff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Financeiro",
                table: "StatusCurva");

            migrationBuilder.DropColumn(
                name: "Fisico",
                table: "StatusCurva");

            migrationBuilder.AddColumn<decimal>(
                name: "PercentualFinanceiro",
                table: "StatusCurva",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentualFisico",
                table: "StatusCurva",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentualFinanceiro",
                table: "StatusCurva");

            migrationBuilder.DropColumn(
                name: "PercentualFisico",
                table: "StatusCurva");

            migrationBuilder.AddColumn<decimal>(
                name: "Financeiro",
                table: "StatusCurva",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Fisico",
                table: "StatusCurva",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}