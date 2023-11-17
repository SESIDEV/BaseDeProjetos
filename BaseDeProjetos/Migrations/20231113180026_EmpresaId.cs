using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class EmpresaId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prospeccao_Empresa_EmpresaId",
                table: "Prospeccao");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaId",
                table: "Prospeccao",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Prospeccao_Empresa_EmpresaId",
                table: "Prospeccao",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prospeccao_Empresa_EmpresaId",
                table: "Prospeccao");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaId",
                table: "Prospeccao",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Prospeccao_Empresa_EmpresaId",
                table: "Prospeccao",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
