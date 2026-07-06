using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AddProspecaoPrincipalRelacionamento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProspeccaoPrincipalId",
                table: "prospeccao",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_prospeccao_ProspeccaoPrincipalId",
                table: "prospeccao",
                column: "ProspeccaoPrincipalId");

            migrationBuilder.AddForeignKey(
                name: "FK_prospeccao_prospeccao_ProspeccaoPrincipalId",
                table: "prospeccao",
                column: "ProspeccaoPrincipalId",
                principalTable: "prospeccao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_prospeccao_prospeccao_ProspeccaoPrincipalId",
                table: "prospeccao");

            migrationBuilder.DropIndex(
                name: "IX_prospeccao_ProspeccaoPrincipalId",
                table: "prospeccao");

            migrationBuilder.DropColumn(
                name: "ProspeccaoPrincipalId",
                table: "prospeccao");
        }
    }
}
