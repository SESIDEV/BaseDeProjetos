using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class ProponenteProjeto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_Empresa_ProponenteId",
                table: "Projeto");

            migrationBuilder.AlterColumn<string>(
                name: "ProponenteId",
                table: "Projeto",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_AspNetUsers_ProponenteId",
                table: "Projeto",
                column: "ProponenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_AspNetUsers_ProponenteId",
                table: "Projeto");

            migrationBuilder.AlterColumn<int>(
                name: "ProponenteId",
                table: "Projeto",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_Empresa_ProponenteId",
                table: "Projeto",
                column: "ProponenteId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
