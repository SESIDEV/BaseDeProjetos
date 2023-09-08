using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class InclusaoDeCargoParaUSuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CargoId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CargoId",
                table: "AspNetUsers",
                column: "CargoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cargo_CargoId",
                table: "AspNetUsers",
                column: "CargoId",
                principalTable: "Cargo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cargo_CargoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CargoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CargoId",
                table: "AspNetUsers");
        }
    }
}
