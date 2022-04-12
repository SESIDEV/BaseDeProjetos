using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace BaseDeProjetos.Migrations
{
    public partial class FormatoIdEntrega : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrega_Projeto_projetoId",
                table: "Entrega");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Entrega");

            migrationBuilder.RenameColumn(
                name: "projetoId",
                table: "Entrega",
                newName: "ProjetoId");

            migrationBuilder.RenameIndex(
                name: "IX_Entrega_projetoId",
                table: "Entrega",
                newName: "IX_Entrega_ProjetoId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Entrega",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEntrega",
                table: "Entrega",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Entrega_Projeto_ProjetoId",
                table: "Entrega",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrega_Projeto_ProjetoId",
                table: "Entrega");

            migrationBuilder.DropColumn(
                name: "DataEntrega",
                table: "Entrega");

            migrationBuilder.RenameColumn(
                name: "ProjetoId",
                table: "Entrega",
                newName: "projetoId");

            migrationBuilder.RenameIndex(
                name: "IX_Entrega_ProjetoId",
                table: "Entrega",
                newName: "IX_Entrega_projetoId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Entrega",
                type: "int",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "Entrega",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Entrega_Projeto_projetoId",
                table: "Entrega",
                column: "projetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}