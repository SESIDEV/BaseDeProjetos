using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class MigrationFixSGI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AtividadesProdutivas",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "AreaAtividade",
                table: "AtividadesProdutivas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Atividade",
                table: "AtividadesProdutivas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "CargaHoraria",
                table: "AtividadesProdutivas",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "AtividadesProdutivas",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DescricaoAtividade",
                table: "AtividadesProdutivas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FonteFomento",
                table: "AtividadesProdutivas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProjetoId",
                table: "AtividadesProdutivas",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AtividadesProdutivas",
                table: "AtividadesProdutivas",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesProdutivas_UsuarioId",
                table: "AtividadesProdutivas",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AtividadesProdutivas",
                table: "AtividadesProdutivas");

            migrationBuilder.DropIndex(
                name: "IX_AtividadesProdutivas_UsuarioId",
                table: "AtividadesProdutivas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AtividadesProdutivas");

            migrationBuilder.DropColumn(
                name: "AreaAtividade",
                table: "AtividadesProdutivas");

            migrationBuilder.DropColumn(
                name: "Atividade",
                table: "AtividadesProdutivas");

            migrationBuilder.DropColumn(
                name: "CargaHoraria",
                table: "AtividadesProdutivas");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "AtividadesProdutivas");

            migrationBuilder.DropColumn(
                name: "DescricaoAtividade",
                table: "AtividadesProdutivas");

            migrationBuilder.DropColumn(
                name: "FonteFomento",
                table: "AtividadesProdutivas");

            migrationBuilder.DropColumn(
                name: "ProjetoId",
                table: "AtividadesProdutivas");
        }
    }
}
