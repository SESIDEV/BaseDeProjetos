using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Data.Migrations
{
    public partial class Prospeccoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Prospeccao");

            migrationBuilder.DropColumn(
                name: "Empresa",
                table: "Prospeccao");

            migrationBuilder.AddColumn<int>(
                name: "Contatoid",
                table: "Prospeccao",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Prospeccao",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LinhaPequisa",
                table: "Prospeccao",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipoContratacao",
                table: "Prospeccao",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Prospeccao",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true),
                    Segmento = table.Column<string>(nullable: true),
                    Estado = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pessoa",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    empresaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoa", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pessoa_Empresa_empresaId",
                        column: x => x.empresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prospeccao_Contatoid",
                table: "Prospeccao",
                column: "Contatoid");

            migrationBuilder.CreateIndex(
                name: "IX_Prospeccao_EmpresaId",
                table: "Prospeccao",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prospeccao_UsuarioId",
                table: "Prospeccao",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoa_empresaId",
                table: "Pessoa",
                column: "empresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prospeccao_Pessoa_Contatoid",
                table: "Prospeccao",
                column: "Contatoid",
                principalTable: "Pessoa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prospeccao_Empresa_EmpresaId",
                table: "Prospeccao",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prospeccao_AspNetUsers_UsuarioId",
                table: "Prospeccao",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prospeccao_Pessoa_Contatoid",
                table: "Prospeccao");

            migrationBuilder.DropForeignKey(
                name: "FK_Prospeccao_Empresa_EmpresaId",
                table: "Prospeccao");

            migrationBuilder.DropForeignKey(
                name: "FK_Prospeccao_AspNetUsers_UsuarioId",
                table: "Prospeccao");

            migrationBuilder.DropTable(
                name: "Pessoa");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropIndex(
                name: "IX_Prospeccao_Contatoid",
                table: "Prospeccao");

            migrationBuilder.DropIndex(
                name: "IX_Prospeccao_EmpresaId",
                table: "Prospeccao");

            migrationBuilder.DropIndex(
                name: "IX_Prospeccao_UsuarioId",
                table: "Prospeccao");

            migrationBuilder.DropColumn(
                name: "Contatoid",
                table: "Prospeccao");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Prospeccao");

            migrationBuilder.DropColumn(
                name: "LinhaPequisa",
                table: "Prospeccao");

            migrationBuilder.DropColumn(
                name: "TipoContratacao",
                table: "Prospeccao");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Prospeccao");

            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "Prospeccao",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Empresa",
                table: "Prospeccao",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
