using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class Rubricas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RubricasId",
                table: "Projeto",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConjuntoRubrica",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConjuntoRubrica", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rubrica",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<decimal>(nullable: false),
                    ConjuntoRubricaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rubrica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rubrica_ConjuntoRubrica_ConjuntoRubricaId",
                        column: x => x.ConjuntoRubricaId,
                        principalTable: "ConjuntoRubrica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_RubricasId",
                table: "Projeto",
                column: "RubricasId");

            migrationBuilder.CreateIndex(
                name: "IX_Rubrica_ConjuntoRubricaId",
                table: "Rubrica",
                column: "ConjuntoRubricaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_ConjuntoRubrica_RubricasId",
                table: "Projeto",
                column: "RubricasId",
                principalTable: "ConjuntoRubrica",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_ConjuntoRubrica_RubricasId",
                table: "Projeto");

            migrationBuilder.DropTable(
                name: "Rubrica");

            migrationBuilder.DropTable(
                name: "ConjuntoRubrica");

            migrationBuilder.DropIndex(
                name: "IX_Projeto_RubricasId",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "RubricasId",
                table: "Projeto");
        }
    }
}
