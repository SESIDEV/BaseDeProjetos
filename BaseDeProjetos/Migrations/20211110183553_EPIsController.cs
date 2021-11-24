using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class EPIsController : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistroEPI",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataEntrega = table.Column<DateTime>(nullable: false),
                    UnidadeOperacional = table.Column<int>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true),
                    Justificativa = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroEPI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroEPI_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistroEPI_UsuarioId",
                table: "RegistroEPI",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistroEPI");
        }
    }
}
