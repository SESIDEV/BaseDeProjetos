using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Data.Migrations
{
    public partial class TipoEstado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_AspNetUsers_LiderId",
                table: "Projeto");

            migrationBuilder.DropIndex(
                name: "IX_Projeto_LiderId",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "LiderId",
                table: "Projeto");

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Projeto",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Projeto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "LiderId",
                table: "Projeto",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_LiderId",
                table: "Projeto",
                column: "LiderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_AspNetUsers_LiderId",
                table: "Projeto",
                column: "LiderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
