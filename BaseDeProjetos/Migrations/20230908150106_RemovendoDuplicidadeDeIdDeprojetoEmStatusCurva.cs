using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class RemovendoDuplicidadeDeIdDeprojetoEmStatusCurva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdProjeto",
                table: "StatusCurva");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdProjeto",
                table: "StatusCurva",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
