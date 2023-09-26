using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class fixupchavesequipeprojeto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipeProjeto_AspNetUsers_UsuarioId",
                table: "EquipeProjeto");

            migrationBuilder.DropIndex(
                name: "IX_EquipeProjeto_UsuarioId",
                table: "EquipeProjeto");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "EquipeProjeto");

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "EquipeProjeto",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipeProjeto_IdUsuario",
                table: "EquipeProjeto",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipeProjeto_AspNetUsers_IdUsuario",
                table: "EquipeProjeto",
                column: "IdUsuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquipeProjeto_AspNetUsers_IdUsuario",
                table: "EquipeProjeto");

            migrationBuilder.DropIndex(
                name: "IX_EquipeProjeto_IdUsuario",
                table: "EquipeProjeto");

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "EquipeProjeto",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "EquipeProjeto",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipeProjeto_UsuarioId",
                table: "EquipeProjeto",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipeProjeto_AspNetUsers_UsuarioId",
                table: "EquipeProjeto",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
