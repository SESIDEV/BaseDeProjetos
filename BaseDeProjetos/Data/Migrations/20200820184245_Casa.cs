using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Data.Migrations
{
    public partial class Casa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Casa",
                table: "Prospeccao",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FollowUp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrigemID = table.Column<string>(nullable: true),
                    Anotacoes = table.Column<string>(nullable: true),
                    Data = table.Column<DateTime>(nullable: false),
                    AnoFiscal = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUp_Prospeccao_OrigemID",
                        column: x => x.OrigemID,
                        principalTable: "Prospeccao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_OrigemID",
                table: "FollowUp",
                column: "OrigemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowUp");

            migrationBuilder.DropColumn(
                name: "Casa",
                table: "Prospeccao");
        }
    }
}
