using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class gestao_conhecimento_patentes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                table: "Producao",
                name: "Empresa",
                nullable: true
               );
            migrationBuilder.AddColumn<string>(
                        table: "Producao",
                        name: "Projeto",
                        nullable: true
                       );
            migrationBuilder.AddColumn<string>(
                        table: "Producao",
                        name: "Responsavel",
                        nullable: true
                       );
            migrationBuilder.AddColumn<string>(
                        table: "Producao",
                        name: "NumPatente",
                        nullable: true
                       );


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "Empresa",
            table: "Producao");
  migrationBuilder.DropColumn(
            name: "Projeto",
            table: "Producao");
  migrationBuilder.DropColumn(
            name: "Responsavel",
            table: "Producao");
              migrationBuilder.DropColumn(
            name: "NumPatente",
            table: "Producao");



        }
    }
}
