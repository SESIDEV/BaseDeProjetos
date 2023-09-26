using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
	public partial class equipeprojeto : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Maquina");

			// Favor não alterar, adicionado manualmente pra consertar o historico de bagunças chamadas migrations
			migrationBuilder.DropForeignKey(
				name: "FK_AspNetUsers_Projeto_ProjetoId",
				table: "AspNetUsers");

			migrationBuilder.DropIndex(
				name: "IX_AspNetUsers_ProjetoId",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "ProjetoId",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "MembrosEquipe",
				table: "Projeto");

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
					Salario = table.Column<decimal>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Cargo", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "EquipeProjeto",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
					IdTrabalho = table.Column<string>(nullable: true),
					UsuarioId = table.Column<string>(nullable: true),
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
						name: "FK_EquipeProjeto_AspNetUsers_UsuarioId",
						column: x => x.UsuarioId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_CargoId",
				table: "AspNetUsers",
				column: "CargoId");

			migrationBuilder.CreateIndex(
				name: "IX_EquipeProjeto_IdTrabalho",
				table: "EquipeProjeto",
				column: "IdTrabalho");

			migrationBuilder.CreateIndex(
				name: "IX_EquipeProjeto_UsuarioId",
				table: "EquipeProjeto",
				column: "UsuarioId");

			migrationBuilder.AddForeignKey(
				name: "FK_AspNetUsers_Cargo_CargoId",
				table: "AspNetUsers",
				column: "CargoId",
				principalTable: "Cargo",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AspNetUsers_Cargo_CargoId",
				table: "AspNetUsers");

			migrationBuilder.DropTable(
				name: "Cargo");

			migrationBuilder.DropTable(
				name: "EquipeProjeto");

			migrationBuilder.DropIndex(
				name: "IX_AspNetUsers_CargoId",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "CargoId",
				table: "AspNetUsers");

			migrationBuilder.AddColumn<string>(
				name: "MembrosEquipe",
				table: "Projeto",
				type: "longtext CHARACTER SET utf8mb4",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "ProjetoId",
				table: "AspNetUsers",
				type: "varchar(255) CHARACTER SET utf8mb4",
				nullable: true);

			migrationBuilder.CreateTable(
				name: "Maquina",
				columns: table => new
				{
					ProjetoId = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.ForeignKey(
						name: "FK_Maquina_Projeto_ProjetoId",
						column: x => x.ProjetoId,
						principalTable: "Projeto",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_AspNetUsers_ProjetoId",
				table: "AspNetUsers",
				column: "ProjetoId");
		}
	}
}
