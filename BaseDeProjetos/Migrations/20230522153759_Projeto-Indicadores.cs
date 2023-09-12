using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class ProjetoIndicadores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjetoIndicadores",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IdProjeto = table.Column<string>(nullable: true),
                    Regramento = table.Column<bool>(nullable: false),
                    Repasse = table.Column<bool>(nullable: false),
                    ComprasServico = table.Column<bool>(nullable: false),
                    ComprasMaterial = table.Column<bool>(nullable: false),
                    Bolsista = table.Column<bool>(nullable: false),
                    SatisfacaoMetadeProjeto = table.Column<bool>(nullable: false),
                    SatisfacaoFimProjeto = table.Column<bool>(nullable: false),
                    Relatorios = table.Column<bool>(nullable: false),
                    PrestacaoContas = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoIndicadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoIndicadores_Projeto_IdProjeto",
                        column: x => x.IdProjeto,
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoIndicadores_IdProjeto",
                table: "ProjetoIndicadores",
                column: "IdProjeto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjetoIndicadores");
        }
    }
}
