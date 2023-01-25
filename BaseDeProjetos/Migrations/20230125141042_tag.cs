using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class tag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissoes_Editais_EditalId1",
                table: "Submissoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissoes_Prospeccao_ProspeccaoId",
                table: "Submissoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submissoes",
                table: "Submissoes");

            migrationBuilder.RenameTable(
                name: "Submissoes",
                newName: "Submissao");

            migrationBuilder.RenameIndex(
                name: "IX_Submissoes_ProspeccaoId",
                table: "Submissao",
                newName: "IX_Submissao_ProspeccaoId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissoes_EditalId1",
                table: "Submissao",
                newName: "IX_Submissao_EditalId1");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Prospeccao",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submissao",
                table: "Submissao",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissao_Editais_EditalId1",
                table: "Submissao",
                column: "EditalId1",
                principalTable: "Editais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissao_Prospeccao_ProspeccaoId",
                table: "Submissao",
                column: "ProspeccaoId",
                principalTable: "Prospeccao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissao_Editais_EditalId1",
                table: "Submissao");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissao_Prospeccao_ProspeccaoId",
                table: "Submissao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Submissao",
                table: "Submissao");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Prospeccao");

            migrationBuilder.RenameTable(
                name: "Submissao",
                newName: "Submissoes");

            migrationBuilder.RenameIndex(
                name: "IX_Submissao_ProspeccaoId",
                table: "Submissoes",
                newName: "IX_Submissoes_ProspeccaoId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissao_EditalId1",
                table: "Submissoes",
                newName: "IX_Submissoes_EditalId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Submissoes",
                table: "Submissoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissoes_Editais_EditalId1",
                table: "Submissoes",
                column: "EditalId1",
                principalTable: "Editais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Submissoes_Prospeccao_ProspeccaoId",
                table: "Submissoes",
                column: "ProspeccaoId",
                principalTable: "Prospeccao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
