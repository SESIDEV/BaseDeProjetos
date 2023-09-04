using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class UsuarioEmProjeto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Projeto",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_UsuarioId",
                table: "Projeto",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_AspNetUsers_UsuarioId",
                table: "Projeto",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_AspNetUsers_UsuarioId",
                table: "Projeto");

            migrationBuilder.DropIndex(
                name: "IX_Projeto_UsuarioId",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Projeto");
        }
    }
}
