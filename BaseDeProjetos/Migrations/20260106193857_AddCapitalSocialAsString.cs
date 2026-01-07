using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AddCapitalSocialAsText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
                //name: "CapitalSocial",
                //table: "Empresa",
                //type: "text", // MySQL: use 'text' para strings longas
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
