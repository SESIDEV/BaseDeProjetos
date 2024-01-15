using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class EquipeProspeccao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipeProspeccao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTrabalho = table.Column<string>(nullable: true),
                    IdUsuario = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipeProspeccao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipeProspeccao_Prospeccao_IdTrabalho",
                        column: x => x.IdTrabalho,
                        principalTable: "Prospeccao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipeProspeccao_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipeProspeccao_IdTrabalho",
                table: "EquipeProspeccao",
                column: "IdTrabalho");

            migrationBuilder.CreateIndex(
                name: "IX_EquipeProspeccao_IdUsuario",
                table: "EquipeProspeccao",
                column: "IdUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipeProspeccao");
        }
    }
}
