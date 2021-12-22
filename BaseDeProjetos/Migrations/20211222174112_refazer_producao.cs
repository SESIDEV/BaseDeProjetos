using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class refazer_producao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Casa",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "DataDaRealizacao",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "Pessoa",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "RealizadoEm",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "publico",
                table: "Producao");

            migrationBuilder.AddColumn<int>(
                name: "DadosBasicosId",
                table: "Producao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InformacoesAdicionais",
                table: "Producao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PalavrasChaves",
                table: "Producao",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProducaoId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DadosBasicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosBasicos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Producao_DadosBasicosId",
                table: "Producao",
                column: "DadosBasicosId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProducaoId",
                table: "AspNetUsers",
                column: "ProducaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Producao_ProducaoId",
                table: "AspNetUsers",
                column: "ProducaoId",
                principalTable: "Producao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Producao_DadosBasicos_DadosBasicosId",
                table: "Producao",
                column: "DadosBasicosId",
                principalTable: "DadosBasicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Producao_ProducaoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Producao_DadosBasicos_DadosBasicosId",
                table: "Producao");

            migrationBuilder.DropTable(
                name: "DadosBasicos");

            migrationBuilder.DropIndex(
                name: "IX_Producao_DadosBasicosId",
                table: "Producao");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProducaoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DadosBasicosId",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "InformacoesAdicionais",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "PalavrasChaves",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "ProducaoId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Casa",
                table: "Producao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataDaRealizacao",
                table: "Producao",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Producao",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Producao",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pessoa",
                table: "Producao",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealizadoEm",
                table: "Producao",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Producao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "publico",
                table: "Producao",
                type: "int",
                nullable: true);
        }
    }
}
