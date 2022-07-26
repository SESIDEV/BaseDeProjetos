using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class LimpezaClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
     
            migrationBuilder.DropTable(
                name: "Entrega");

            migrationBuilder.DropTable(
                name: "Producao");

            migrationBuilder.DropTable(
                name: "RegistroEPI");

            migrationBuilder.DropTable(
                name: "DadosBasicos");

            migrationBuilder.DropTable(
                name: "Detalhamento");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProducaoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProducaoId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProducaoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DadosBasicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    FlagRelevancia = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Idioma = table.Column<int>(type: "int", nullable: false),
                    MeioDivulgacao = table.Column<int>(type: "int", nullable: false),
                    Pais = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosBasicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Detalhamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalhamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entrega",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Concluida = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataInicioEntrega = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DescricaoEntrega = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    NomeEntrega = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ProjetoId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrega", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entrega_Projeto_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistroEPI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataEntrega = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Justificativa = table.Column<int>(type: "int", nullable: false),
                    UnidadeOperacional = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroEPI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroEPI_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Producao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AreasDoConhecimento = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DadosBasicosId = table.Column<int>(type: "int", nullable: true),
                    DetalhamentoId = table.Column<int>(type: "int", nullable: true),
                    InformacoesAdicionais = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PalavrasChaves = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SetorDeAtividade = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producao_DadosBasicos_DadosBasicosId",
                        column: x => x.DadosBasicosId,
                        principalTable: "DadosBasicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Producao_Detalhamento_DetalhamentoId",
                        column: x => x.DetalhamentoId,
                        principalTable: "Detalhamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProducaoId",
                table: "AspNetUsers",
                column: "ProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrega_ProjetoId",
                table: "Entrega",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Producao_DadosBasicosId",
                table: "Producao",
                column: "DadosBasicosId");

            migrationBuilder.CreateIndex(
                name: "IX_Producao_DetalhamentoId",
                table: "Producao",
                column: "DetalhamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroEPI_UsuarioId",
                table: "RegistroEPI",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Producao_ProducaoId",
                table: "AspNetUsers",
                column: "ProducaoId",
                principalTable: "Producao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
