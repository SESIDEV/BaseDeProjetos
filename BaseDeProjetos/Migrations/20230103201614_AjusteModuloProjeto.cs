using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AjusteModuloProjeto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Projeto_ProjetoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProjetoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProjetoId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "MembrosEquipe",
                table: "Projeto",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembrosEquipe",
                table: "Projeto");

            migrationBuilder.AddColumn<string>(
                name: "ProjetoId",
                table: "AspNetUsers",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

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
        }
    }
}
