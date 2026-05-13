using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AddIndicadoresPlanejamentoMensal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IndicadoresPlanejamentoMensal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Casa = table.Column<int>(nullable: false),
                    Ano = table.Column<int>(nullable: false),
                    Indicador = table.Column<string>(maxLength: 120, nullable: false),
                    Coluna = table.Column<int>(nullable: false),
                    Valor = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicadoresPlanejamentoMensal", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndicadoresPlanejamentoMensal_Casa_Ano_Indicador_Coluna",
                table: "IndicadoresPlanejamentoMensal",
                columns: new[] { "Casa", "Ano", "Indicador", "Coluna" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndicadoresPlanejamentoMensal");
        }
    }
}
