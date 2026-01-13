using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class AlterCapitalSocialToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
                //name: "CapitalSocial",
                //table: "Empresa",
                //type: "text", // ou "varchar(255)"
                //nullable: true,
                //oldClrType: typeof(decimal),
                //oldType: "decimal(18,2)",
                //oldNullable: true);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<decimal>(
                //name: "CapitalSocial",
                //table: "Empresa",
                //type: "decimal(18,2)",
                //nullable: true,
                //oldClrType: typeof(string),
                //oldType: "text",
                //oldNullable: true);
        }

    }
}
