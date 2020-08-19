using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Data.Migrations
{
    public partial class ProjetoCompleto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Empresa",
                table: "Projeto");

            migrationBuilder.AddColumn<int>(
                name: "AreaPesquisa",
                table: "Projeto",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEncerramento",
                table: "Projeto",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "Projeto",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DuracaoProjetoEmMeses",
                table: "Projeto",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Projeto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Projeto",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FonteFomento",
                table: "Projeto",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Inovacao",
                table: "Projeto",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LiderId",
                table: "Projeto",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeProjeto",
                table: "Projeto",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProponenteId",
                table: "Projeto",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ValorAporteRecursos",
                table: "Projeto",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ValorTotalProjeto",
                table: "Projeto",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Projeto",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProjetoId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_EmpresaId",
                table: "Projeto",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_LiderId",
                table: "Projeto",
                column: "LiderId");

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_ProponenteId",
                table: "Projeto",
                column: "ProponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProjetoId",
                table: "AspNetUsers",
                column: "ProjetoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Projeto_ProjetoId",
                table: "AspNetUsers",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_Empresa_EmpresaId",
                table: "Projeto",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_AspNetUsers_LiderId",
                table: "Projeto",
                column: "LiderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_Empresa_ProponenteId",
                table: "Projeto",
                column: "ProponenteId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Projeto_ProjetoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_Empresa_EmpresaId",
                table: "Projeto");

            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_AspNetUsers_LiderId",
                table: "Projeto");

            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_Empresa_ProponenteId",
                table: "Projeto");

            migrationBuilder.DropIndex(
                name: "IX_Projeto_EmpresaId",
                table: "Projeto");

            migrationBuilder.DropIndex(
                name: "IX_Projeto_LiderId",
                table: "Projeto");

            migrationBuilder.DropIndex(
                name: "IX_Projeto_ProponenteId",
                table: "Projeto");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjetoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AreaPesquisa",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "DataEncerramento",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "DuracaoProjetoEmMeses",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "FonteFomento",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "Inovacao",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "LiderId",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "NomeProjeto",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "ProponenteId",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "ValorAporteRecursos",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "ValorTotalProjeto",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "ProjetoId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Empresa",
                table: "Projeto",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
