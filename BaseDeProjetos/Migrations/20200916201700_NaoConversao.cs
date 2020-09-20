using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class NaoConversao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MotivoNaoConversao",
                table: "FollowUp",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoNaoConversao",
                table: "FollowUp");
        }
    }
}
