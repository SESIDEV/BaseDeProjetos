using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class RemoverNulidadeQtdPesquisadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QtdPesquisadores",
                table: "IndicadoresFinanceiros",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QtdPesquisadores",
                table: "IndicadoresFinanceiros",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
