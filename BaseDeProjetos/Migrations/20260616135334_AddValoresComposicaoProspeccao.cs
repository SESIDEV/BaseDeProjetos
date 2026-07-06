using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AddValoresComposicaoProspeccao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValorFinal",
                table: "prospeccao",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "prospeccao_valor_composicao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProspeccaoId = table.Column<string>(nullable: true),
                    Origem = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: true),
                    Natureza = table.Column<string>(nullable: true),
                    Valor = table.Column<decimal>(nullable: true),
                    Observacao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prospeccao_valor_composicao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_prospeccao_valor_composicao_prospeccao_ProspeccaoId",
                        column: x => x.ProspeccaoId,
                        principalTable: "prospeccao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_prospeccao_valor_composicao_ProspeccaoId",
                table: "prospeccao_valor_composicao",
                column: "ProspeccaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prospeccao_valor_composicao");

            migrationBuilder.DropColumn(
                name: "ValorFinal",
                table: "prospeccao");
        }
    }
}
