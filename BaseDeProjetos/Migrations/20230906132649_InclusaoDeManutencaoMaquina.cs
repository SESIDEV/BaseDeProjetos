using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class InclusaoDeManutencaoMaquina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CustoHoraMaquina",
                table: "Maquina",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<string>(
                name: "IdentificacaoDaMaquina",
                table: "Maquina",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentificacaoDaMaquina",
                table: "Maquina");

            migrationBuilder.AlterColumn<decimal>(
                name: "CustoHoraMaquina",
                table: "Maquina",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
