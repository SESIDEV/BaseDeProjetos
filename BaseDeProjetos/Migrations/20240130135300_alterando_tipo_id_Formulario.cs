using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class alterando_tipo_id_Formulario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Formulario",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TituloFormulario = table.Column<string>(nullable: true),
                    DescricaoFormulario = table.Column<string>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    Identificador = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formulario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerguntasForm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TituloPergunta = table.Column<string>(nullable: true),
                    Tipo = table.Column<int>(nullable: false),
                    Obrigatorio = table.Column<bool>(nullable: false),
                    FormularioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerguntasForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerguntasForm_Formulario_FormularioId",
                        column: x => x.FormularioId,
                        principalTable: "Formulario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubmissaoForm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomeProjeto = table.Column<string>(nullable: true),
                    EmailRemetente = table.Column<string>(nullable: true),
                    DataSubmissao = table.Column<DateTime>(nullable: false),
                    Respostas = table.Column<string>(nullable: true),
                    FormularioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissaoForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmissaoForm_Formulario_FormularioId",
                        column: x => x.FormularioId,
                        principalTable: "Formulario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerguntasForm_FormularioId",
                table: "PerguntasForm",
                column: "FormularioId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissaoForm_FormularioId",
                table: "SubmissaoForm",
                column: "FormularioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerguntasForm");

            migrationBuilder.DropTable(
                name: "SubmissaoForm");

            migrationBuilder.DropTable(
                name: "Formulario");
        }
    }
}
