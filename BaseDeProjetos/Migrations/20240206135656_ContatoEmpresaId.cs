using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class ContatoEmpresaId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pessoa_Empresa_empresaId",
                table: "Pessoa");

            migrationBuilder.RenameColumn(
                name: "empresaId",
                table: "Pessoa",
                newName: "EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_Pessoa_empresaId",
                table: "Pessoa",
                newName: "IX_Pessoa_EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pessoa_Empresa_EmpresaId",
                table: "Pessoa",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pessoa_Empresa_EmpresaId",
                table: "Pessoa");

            migrationBuilder.RenameColumn(
                name: "EmpresaId",
                table: "Pessoa",
                newName: "empresaId");

            migrationBuilder.RenameIndex(
                name: "IX_Pessoa_EmpresaId",
                table: "Pessoa",
                newName: "IX_Pessoa_empresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pessoa_Empresa_empresaId",
                table: "Pessoa",
                column: "empresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
