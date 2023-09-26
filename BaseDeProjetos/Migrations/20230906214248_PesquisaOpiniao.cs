using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
	public partial class PesquisaOpiniao : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ValorSatisfacaoFimProjeto",
				table: "ProjetoIndicadores");

			migrationBuilder.DropColumn(
				name: "ValorSatisfacaoMetadeProjeto",
				table: "ProjetoIndicadores");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "ValorSatisfacaoFimProjeto",
				table: "ProjetoIndicadores",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddColumn<int>(
				name: "ValorSatisfacaoMetadeProjeto",
				table: "ProjetoIndicadores",
				type: "int",
				nullable: false,
				defaultValue: 0);
		}
	}
}
