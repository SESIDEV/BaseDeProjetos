using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class IndicadoresProjetoCFF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_Empresa_ProponenteId",
                table: "Projeto");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetoIndicadores_Projeto_IdProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.DropIndex(
                name: "IX_ProjetoIndicadores_IdProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.DropColumn(
                name: "IdProjeto",
                table: "ProjetoIndicadores");

            migrationBuilder.DropColumn(
                name: "MembrosEquipe",
                table: "Projeto");

            migrationBuilder.AddColumn<string>(
                name: "ProjetoId",
                table: "ProjetoIndicadores",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProponenteId",
                table: "Projeto",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CustoHH",
                table: "Projeto",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CustoHM",
                table: "Projeto",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "SatisfacaoClienteFinal",
                table: "Projeto",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SatisfacaoClienteParcial",
                table: "Projeto",
                nullable: true);

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
                    Salario = table.Column<decimal>(nullable: false),
                    HorasSemanais = table.Column<int>(nullable: false),
                    Tributos = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurvaFisicoFinanceira",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    PercentualFisico = table.Column<decimal>(nullable: false),
                    PercentualFinanceiro = table.Column<decimal>(nullable: false),
                    ProjetoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurvaFisicoFinanceira", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurvaFisicoFinanceira_Projeto_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipeProjeto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdTrabalho = table.Column<string>(nullable: true),
                    IdUsuario = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipeProjeto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipeProjeto_Projeto_IdTrabalho",
                        column: x => x.IdTrabalho,
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipeProjeto_AspNetUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PesquisaProjeto",
                columns: table => new
                {
                    IdPesquisa = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<string>(nullable: true),
                    ResultadoFinal = table.Column<double>(nullable: false),
                    Comentarios = table.Column<string>(nullable: true),
                    RepresentacaoTextualQuestionario = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PesquisaProjeto", x => x.IdPesquisa);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoIndicadores_ProjetoId",
                table: "ProjetoIndicadores",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CargoId",
                table: "AspNetUsers",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_CurvaFisicoFinanceira_ProjetoId",
                table: "CurvaFisicoFinanceira",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipeProjeto_IdTrabalho",
                table: "EquipeProjeto",
                column: "IdTrabalho");

            migrationBuilder.CreateIndex(
                name: "IX_EquipeProjeto_IdUsuario",
                table: "EquipeProjeto",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cargo_CargoId",
                table: "AspNetUsers",
                column: "CargoId",
                principalTable: "Cargo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_AspNetUsers_ProponenteId",
                table: "Projeto",
                column: "ProponenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetoIndicadores_Projeto_ProjetoId",
                table: "ProjetoIndicadores",
                column: "ProjetoId",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cargo_CargoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Projeto_AspNetUsers_ProponenteId",
                table: "Projeto");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjetoIndicadores_Projeto_ProjetoId",
                table: "ProjetoIndicadores");

            migrationBuilder.DropTable(
                name: "Cargo");

            migrationBuilder.DropTable(
                name: "CurvaFisicoFinanceira");

            migrationBuilder.DropTable(
                name: "EquipeProjeto");

            migrationBuilder.DropTable(
                name: "PesquisaProjeto");

            migrationBuilder.DropIndex(
                name: "IX_ProjetoIndicadores_ProjetoId",
                table: "ProjetoIndicadores");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CargoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProjetoId",
                table: "ProjetoIndicadores");

            migrationBuilder.DropColumn(
                name: "CustoHH",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "CustoHM",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "SatisfacaoClienteFinal",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "SatisfacaoClienteParcial",
                table: "Projeto");

            migrationBuilder.DropColumn(
                name: "CargoId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "IdProjeto",
                table: "ProjetoIndicadores",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProponenteId",
                table: "Projeto",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MembrosEquipe",
                table: "Projeto",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoIndicadores_IdProjeto",
                table: "ProjetoIndicadores",
                column: "IdProjeto");

            migrationBuilder.AddForeignKey(
                name: "FK_Projeto_Empresa_ProponenteId",
                table: "Projeto",
                column: "ProponenteId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjetoIndicadores_Projeto_IdProjeto",
                table: "ProjetoIndicadores",
                column: "IdProjeto",
                principalTable: "Projeto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
