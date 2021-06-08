using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace BaseDeProjetos.Migrations
{
    public partial class Entrega_Nome_DataEntrega_alterado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEntrega",
                table: "Entrega");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicioEntrega",
                table: "Entrega",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataInicioEntrega",
                table: "Entrega");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEntrega",
                table: "Entrega",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
