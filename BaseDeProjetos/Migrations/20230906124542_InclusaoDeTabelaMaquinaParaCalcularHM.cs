using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class InclusaoDeTabelaMaquinaParaCalcularHM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Maquina",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    ManutencaoAnoAnterior = table.Column<bool>(nullable: true),
                    ValorManutenCaoAnoAnterior = table.Column<decimal>(nullable: true),
                    OcupacaoMax = table.Column<decimal>(nullable: false),
                    OcupacaoAtual = table.Column<decimal>(nullable: false),
                    PrecoBase = table.Column<decimal>(nullable: false),
                    CustoHoraMaquina = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquina", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Maquina");
        }
    }
}
