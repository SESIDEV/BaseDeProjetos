using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class statuscurvafk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjetoId",
                table: "StatusCurva",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusCurva_ProjetoId",
                table: "StatusCurva",
                column: "ProjetoId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusCurva_Projeto_ProjetoId",
                table: "StatusCurva",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusCurva_Projeto_ProjetoId",
                table: "StatusCurva");

            migrationBuilder.DropIndex(
                name: "IX_StatusCurva_ProjetoId",
                table: "StatusCurva");

            migrationBuilder.DropColumn(
                name: "ProjetoId",
                table: "StatusCurva");
        }
    }
}
