using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class FloatParaDecimalProjetoSatisfacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "SatisfacaoClienteParcial",
                table: "Projeto",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "SatisfacaoClienteFinal",
                table: "Projeto",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "SatisfacaoClienteParcial",
                table: "Projeto",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "SatisfacaoClienteFinal",
                table: "Projeto",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
