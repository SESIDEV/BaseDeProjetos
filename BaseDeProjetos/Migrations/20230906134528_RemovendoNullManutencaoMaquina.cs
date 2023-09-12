using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class RemovendoNullManutencaoMaquina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ManutencaoAnoAnterior",
                table: "Maquina",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ManutencaoAnoAnterior",
                table: "Maquina",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
