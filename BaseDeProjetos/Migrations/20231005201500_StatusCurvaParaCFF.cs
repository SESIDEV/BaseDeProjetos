using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class StatusCurvaParaCFF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusCurva_Projeto_ProjetoId",
                table: "StatusCurva");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatusCurva",
                table: "StatusCurva");

            migrationBuilder.RenameTable(
                name: "StatusCurva",
                newName: "CurvaFisicoFinanceira");

            migrationBuilder.RenameIndex(
                name: "IX_StatusCurva_ProjetoId",
                table: "CurvaFisicoFinanceira",
                newName: "IX_CurvaFisicoFinanceira_ProjetoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurvaFisicoFinanceira",
                table: "CurvaFisicoFinanceira",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurvaFisicoFinanceira_Projeto_ProjetoId",
                table: "CurvaFisicoFinanceira",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurvaFisicoFinanceira_Projeto_ProjetoId",
                table: "CurvaFisicoFinanceira");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurvaFisicoFinanceira",
                table: "CurvaFisicoFinanceira");

            migrationBuilder.RenameTable(
                name: "CurvaFisicoFinanceira",
                newName: "StatusCurva");

            migrationBuilder.RenameIndex(
                name: "IX_CurvaFisicoFinanceira_ProjetoId",
                table: "StatusCurva",
                newName: "IX_StatusCurva_ProjetoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatusCurva",
                table: "StatusCurva",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusCurva_Projeto_ProjetoId",
                table: "StatusCurva",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
