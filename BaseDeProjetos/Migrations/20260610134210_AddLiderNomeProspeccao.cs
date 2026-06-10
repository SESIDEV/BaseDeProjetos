using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AddLiderNomeProspeccao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LiderNome",
                table: "prospeccao",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "IndicadoresReceitaGestor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Casa = table.Column<int>(nullable: false),
                    AnoBase = table.Column<int>(nullable: false),
                    Ordem = table.Column<int>(nullable: false),
                    Empresa = table.Column<string>(maxLength: 200, nullable: true),
                    Iniciativa = table.Column<string>(maxLength: 160, nullable: true),
                    ValorTotal = table.Column<decimal>(nullable: true),
                    ReceitaTotal = table.Column<decimal>(nullable: true),
                    ReceitaAnoBase = table.Column<decimal>(nullable: true),
                    ProjecaoAno1 = table.Column<decimal>(nullable: true),
                    ProjecaoAno2 = table.Column<decimal>(nullable: true),
                    ProjecaoAno3 = table.Column<decimal>(nullable: true),
                    ProjecaoAno4 = table.Column<decimal>(nullable: true),
                    ProjecaoAno5 = table.Column<decimal>(nullable: true),
                    ParceiroInterno = table.Column<string>(maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicadoresReceitaGestor", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndicadoresPlanejamentoMensal_Casa_Ano_Indicador_Coluna",
                table: "IndicadoresPlanejamentoMensal",
                columns: new[] { "Casa", "Ano", "Indicador", "Coluna" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndicadoresReceitaGestor_Casa_AnoBase_Ordem",
                table: "IndicadoresReceitaGestor",
                columns: new[] { "Casa", "AnoBase", "Ordem" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndicadoresPlanejamentoMensal");

            migrationBuilder.DropTable(
                name: "IndicadoresReceitaGestor");

            migrationBuilder.DropColumn(
                name: "LiderNome",
                table: "prospeccao");
        }
    }
}
