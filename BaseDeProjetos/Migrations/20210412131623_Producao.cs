using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace BaseDeProjetos.Migrations
{
    public partial class Producao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IndicadoresFinanceiros",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    Receita = table.Column<decimal>(nullable: false),
                    Despeita = table.Column<decimal>(nullable: false),
                    Investimento = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicadoresFinanceiros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<int>(nullable: false),
                    DataDaRealizacao = table.Column<DateTime>(nullable: false),
                    RealizadoEm = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    publico = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producao", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndicadoresFinanceiros");

            migrationBuilder.DropTable(
                name: "Producao");
        }
    }
}
