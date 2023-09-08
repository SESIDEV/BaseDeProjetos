using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AjustesPesquisaOpiniao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ResultadoFinal",
                table: "PesquisaProjeto",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "RepresentacaoTextualQuestionario",
                table: "PesquisaProjeto",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepresentacaoTextualQuestionario",
                table: "PesquisaProjeto");

            migrationBuilder.AlterColumn<int>(
                name: "ResultadoFinal",
                table: "PesquisaProjeto",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
