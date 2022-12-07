using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class string_to_enum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Empresa",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "Projeto",
                table: "Producao");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Producao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjetoId",
                table: "Producao",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Producao_EmpresaId",
                table: "Producao",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producao_ProjetoId",
                table: "Producao",
                column: "ProjetoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Producao_Empresa_EmpresaId",
                table: "Producao",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Producao_Projeto_ProjetoId",
                table: "Producao",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producao_Empresa_EmpresaId",
                table: "Producao");

            migrationBuilder.DropForeignKey(
                name: "FK_Producao_Projeto_ProjetoId",
                table: "Producao");

            migrationBuilder.DropIndex(
                name: "IX_Producao_EmpresaId",
                table: "Producao");

            migrationBuilder.DropIndex(
                name: "IX_Producao_ProjetoId",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Producao");

            migrationBuilder.DropColumn(
                name: "ProjetoId",
                table: "Producao");

            migrationBuilder.AddColumn<string>(
                name: "Empresa",
                table: "Producao",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Projeto",
                table: "Producao",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
