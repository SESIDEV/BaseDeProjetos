using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace BaseDeProjetos.Migrations
{
    public partial class CP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AtividadesProdutivas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Atividade = table.Column<int>(nullable: false),
                    AreaAtividade = table.Column<int>(nullable: false),
                    FonteFomento = table.Column<int>(nullable: false),
                    ProjetoId = table.Column<string>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: true),
                    DescricaoAtividade = table.Column<string>(nullable: true),
                    CargaHoraria = table.Column<double>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadesProdutivas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtividadesProdutivas_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesProdutivas_UsuarioId",
                table: "AtividadesProdutivas",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtividadesProdutivas");
        }
    }
}