using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class Cargo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Não editar, tive de adicionar manualmente
            migrationBuilder.AddColumn<int>(
                name: "CargoId",
                table: "AspNetUsers",
                nullable: true);


            migrationBuilder.CreateTable(
                name: "Cargo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Salario = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargo", x => x.Id);
                });

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

            // Não alterar, tive de adicionar manualmente
            migrationBuilder.DropColumn(
                name: "CargoId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Cargo");

        }
    }
}
