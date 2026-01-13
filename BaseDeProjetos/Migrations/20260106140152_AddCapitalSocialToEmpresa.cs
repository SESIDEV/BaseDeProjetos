using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AddCapitalSocialToEmpresa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<decimal>(
                //name: "CapitalSocial",
                //table: "Empresa",
                //nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
                //name: "CapitalSocial",
                //table: "Empresa");
        }
    }
}
