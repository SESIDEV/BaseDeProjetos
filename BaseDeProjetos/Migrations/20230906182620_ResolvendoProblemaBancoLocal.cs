using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseDeProjetos.Migrations
{
    public partial class ResolvendoProblemaBancoLocal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Casa = table.Column<int>(nullable: false),
                    Nivel = table.Column<int>(nullable: false),
                    Matricula = table.Column<int>(nullable: false),
                    Titulacao = table.Column<int>(nullable: false),
                    Vinculo = table.Column<int>(nullable: false),
                    Foto = table.Column<string>(nullable: true),
                    Competencia = table.Column<string>(nullable: true),
                    CargoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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
                name: "Editais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    Local = table.Column<string>(nullable: true),
                    StatusEdital = table.Column<int>(nullable: false),
                    AgenciaFomento = table.Column<int>(nullable: false),
                    PrazoSubmissao = table.Column<DateTime>(nullable: false),
                    ValorEdital = table.Column<decimal>(nullable: false),
                    LinkEdital = table.Column<string>(nullable: true),
                    DataResultado = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RazaoSocial = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true),
                    Segmento = table.Column<int>(nullable: false),
                    Estado = table.Column<int>(nullable: false),
                    Industrial = table.Column<bool>(nullable: false),
                    EmpresaUnique = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndicadoresFinanceiros",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    Receita = table.Column<decimal>(nullable: false),
                    Despesa = table.Column<decimal>(nullable: false),
                    Investimento = table.Column<decimal>(nullable: false),
                    QualiSeguranca = table.Column<float>(nullable: false),
                    Casa = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicadoresFinanceiros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maquina",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    IdentificacaoDaMaquina = table.Column<string>(nullable: true),
                    ManutencaoAnoAnterior = table.Column<bool>(nullable: false),
                    ValorManutenCaoAnoAnterior = table.Column<decimal>(nullable: true),
                    OcupacaoMax = table.Column<decimal>(nullable: false),
                    OcupacaoAtual = table.Column<decimal>(nullable: false),
                    PrecoBase = table.Column<decimal>(nullable: false),
                    CustoHoraMaquina = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquina", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AtividadesProdutivas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Atividade = table.Column<int>(nullable: false),
                    AreaAtividade = table.Column<int>(nullable: false),
                    FonteFomento = table.Column<int>(nullable: false),
                    ProjetoId = table.Column<string>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: true),
                    DescricaoAtividade = table.Column<string>(nullable: true),
                    CargaHoraria = table.Column<double>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadesProdutivas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtividadesProdutivas_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pessoa",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Telefone = table.Column<string>(nullable: true),
                    empresaId = table.Column<int>(nullable: true),
                    Cargo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoa", x => x.id);
                    table.ForeignKey(
                        name: "FK_Pessoa_Empresa_empresaId",
                        column: x => x.empresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projeto",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NomeProjeto = table.Column<string>(nullable: true),
                    EmpresaId = table.Column<int>(nullable: true),
                    ProponenteId = table.Column<int>(nullable: true),
                    AreaPesquisa = table.Column<int>(nullable: false),
                    DataInicio = table.Column<DateTime>(nullable: false),
                    DataEncerramento = table.Column<DateTime>(nullable: false),
                    MembrosEquipe = table.Column<string>(nullable: true),
                    Estado = table.Column<int>(nullable: false),
                    FonteFomento = table.Column<int>(nullable: false),
                    Inovacao = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DuracaoProjetoEmMeses = table.Column<int>(nullable: false),
                    ValorTotalProjeto = table.Column<double>(nullable: false),
                    ValorAporteRecursos = table.Column<double>(nullable: false),
                    Casa = table.Column<int>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projeto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projeto_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projeto_Empresa_ProponenteId",
                        column: x => x.ProponenteId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projeto_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prospeccao",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NomeProspeccao = table.Column<string>(nullable: true),
                    PotenciaisParceiros = table.Column<string>(nullable: true),
                    EmpresaId = table.Column<int>(nullable: true),
                    Contatoid = table.Column<int>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: true),
                    MembrosEquipe = table.Column<string>(nullable: true),
                    TipoContratacao = table.Column<int>(nullable: false),
                    LinhaPequisa = table.Column<int>(nullable: false),
                    Casa = table.Column<int>(nullable: false),
                    ValorProposta = table.Column<decimal>(nullable: false),
                    ValorEstimado = table.Column<decimal>(nullable: false),
                    CaminhoPasta = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    Origem = table.Column<int>(nullable: false),
                    Ancora = table.Column<bool>(nullable: false),
                    Agregadas = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prospeccao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prospeccao_Pessoa_Contatoid",
                        column: x => x.Contatoid,
                        principalTable: "Pessoa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prospeccao_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prospeccao_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Producao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Grupo = table.Column<int>(nullable: false),
                    Casa = table.Column<int>(nullable: false),
                    Titulo = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    Autores = table.Column<string>(nullable: true),
                    StatusPub = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false),
                    Local = table.Column<string>(nullable: true),
                    DOI = table.Column<string>(nullable: true),
                    Imagem = table.Column<string>(nullable: true),
                    ProjetoId = table.Column<string>(nullable: true),
                    EmpresaId = table.Column<int>(nullable: true),
                    Responsavel = table.Column<string>(nullable: true),
                    NumPatente = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producao_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Producao_Projeto_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoIndicadores",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IdProjeto = table.Column<string>(nullable: true),
                    Regramento = table.Column<bool>(nullable: false),
                    Repasse = table.Column<bool>(nullable: false),
                    ComprasServico = table.Column<bool>(nullable: false),
                    ComprasMaterial = table.Column<bool>(nullable: false),
                    Bolsista = table.Column<bool>(nullable: false),
                    SatisfacaoMetadeProjeto = table.Column<bool>(nullable: false),
                    ValorSatisfacaoMetadeProjeto = table.Column<int>(nullable: false),
                    SatisfacaoFimProjeto = table.Column<bool>(nullable: false),
                    ValorSatisfacaoFimProjeto = table.Column<int>(nullable: false),
                    Relatorios = table.Column<bool>(nullable: false),
                    PrestacaoContas = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoIndicadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoIndicadores_Projeto_IdProjeto",
                        column: x => x.IdProjeto,
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FollowUp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrigemID = table.Column<string>(nullable: true),
                    Anotacoes = table.Column<string>(nullable: true),
                    Data = table.Column<DateTime>(nullable: false),
                    AnoFiscal = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    MotivoNaoConversao = table.Column<int>(nullable: false),
                    Vencimento = table.Column<DateTime>(nullable: false),
                    isTratado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUp_Prospeccao_OrigemID",
                        column: x => x.OrigemID,
                        principalTable: "Prospeccao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Submissao",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ComEmpresa = table.Column<bool>(nullable: false),
                    EditalId1 = table.Column<int>(nullable: true),
                    EditalId = table.Column<string>(nullable: true),
                    ProspeccaoId = table.Column<string>(nullable: true),
                    Proponente = table.Column<string>(nullable: true),
                    ProjetoProposto = table.Column<string>(nullable: true),
                    ResponsavelSubmissao = table.Column<string>(nullable: true),
                    StatusSubmissao = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissao_Editais_EditalId1",
                        column: x => x.EditalId1,
                        principalTable: "Editais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Submissao_Prospeccao_ProspeccaoId",
                        column: x => x.ProspeccaoId,
                        principalTable: "Prospeccao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesProdutivas_UsuarioId",
                table: "AtividadesProdutivas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowUp_OrigemID",
                table: "FollowUp",
                column: "OrigemID");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoa_empresaId",
                table: "Pessoa",
                column: "empresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producao_EmpresaId",
                table: "Producao",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Producao_ProjetoId",
                table: "Producao",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_EmpresaId",
                table: "Projeto",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_ProponenteId",
                table: "Projeto",
                column: "ProponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Projeto_UsuarioId",
                table: "Projeto",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoIndicadores_IdProjeto",
                table: "ProjetoIndicadores",
                column: "IdProjeto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prospeccao_Contatoid",
                table: "Prospeccao",
                column: "Contatoid");

            migrationBuilder.CreateIndex(
                name: "IX_Prospeccao_EmpresaId",
                table: "Prospeccao",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prospeccao_UsuarioId",
                table: "Prospeccao",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissao_EditalId1",
                table: "Submissao",
                column: "EditalId1");

            migrationBuilder.CreateIndex(
                name: "IX_Submissao_ProspeccaoId",
                table: "Submissao",
                column: "ProspeccaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AtividadesProdutivas");

            migrationBuilder.DropTable(
                name: "Cargo");

            migrationBuilder.DropTable(
                name: "FollowUp");

            migrationBuilder.DropTable(
                name: "IndicadoresFinanceiros");

            migrationBuilder.DropTable(
                name: "Maquina");

            migrationBuilder.DropTable(
                name: "Producao");

            migrationBuilder.DropTable(
                name: "ProjetoIndicadores");

            migrationBuilder.DropTable(
                name: "Submissao");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Projeto");

            migrationBuilder.DropTable(
                name: "Editais");

            migrationBuilder.DropTable(
                name: "Prospeccao");

            migrationBuilder.DropTable(
                name: "Pessoa");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Empresa");
        }
    }
}
