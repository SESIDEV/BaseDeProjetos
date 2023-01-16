using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class SubmissaoDeEditais : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjetoProposto",
                table: "Editais");

            migrationBuilder.DropColumn(
                name: "Proponente",
                table: "Editais");

            migrationBuilder.DropColumn(
                name: "ResponsavelSubmissaoid",
                table: "Editais");

            migrationBuilder.DropColumn(
                name: "StatusSubmissao",
                table: "Editais");

            migrationBuilder.CreateTable(
                name: "Submissoes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ComEmpresa = table.Column<bool>(nullable: false),
                    EditalId1 = table.Column<int>(nullable: true),
                    EditalId = table.Column<string>(nullable: true),
                    ProspeccaoId = table.Column<string>(nullable: true),
                    Proponente = table.Column<string>(nullable: true),
                    ProjetoProposto = table.Column<string>(nullable: true),
                    ResponsavelSubmissao = table.Column<string>(nullable: true),
                    StatusSubmissao = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissoes_Editais_EditalId1",
                        column: x => x.EditalId1,
                        principalTable: "Editais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissoes_Prospeccao_ProspeccaoId",
                        column: x => x.ProspeccaoId,
                        principalTable: "Prospeccao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Submissoes_EditalId1",
                table: "Submissoes",
                column: "EditalId1");

            migrationBuilder.CreateIndex(
                name: "IX_Submissoes_ProspeccaoId",
                table: "Submissoes",
                column: "ProspeccaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Submissoes");

            migrationBuilder.AddColumn<string>(
                name: "ProjetoProposto",
                table: "Editais",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Proponente",
                table: "Editais",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusSubmissao",
                table: "Editais",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
