﻿// <auto-generated />
using System;
using BaseDeProjetos.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BaseDeProjetos.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BaseDeProjetos.Models.AtividadesProdutivas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AreaAtividade")
                        .HasColumnType("int");

                    b.Property<int>("Atividade")
                        .HasColumnType("int");

                    b.Property<double>("CargaHoraria")
                        .HasColumnType("double");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DescricaoAtividade")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("FonteFomento")
                        .HasColumnType("int");

                    b.Property<string>("ProjetoId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UsuarioId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("AtividadesProdutivas");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Cargo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("Salario")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("Cargo");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Editais", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AgenciaFomento")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataResultado")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Descricao")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LinkEdital")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Local")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("PrazoSubmissao")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("StatusEdital")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorEdital")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("Editais");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Empresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CNPJ")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("EmpresaUnique")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<bool>("Industrial")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Logo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RazaoSocial")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Segmento")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Empresa");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.EquipeProjeto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("IdTrabalho")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("IdUsuario")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("IdTrabalho");

                    b.HasIndex("IdUsuario");

                    b.ToTable("EquipeProjeto");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.FollowUp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AnoFiscal")
                        .HasColumnType("int");

                    b.Property<string>("Anotacoes")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MotivoNaoConversao")
                        .HasColumnType("int");

                    b.Property<string>("OrigemID")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Vencimento")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("isTratado")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("OrigemID");

                    b.ToTable("FollowUp");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.IndicadoresFinanceiros", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Casa")
                        .HasColumnType("int");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Despesa")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Investimento")
                        .HasColumnType("decimal(65,30)");

                    b.Property<float>("QualiSeguranca")
                        .HasColumnType("float");

                    b.Property<decimal>("Receita")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.ToTable("IndicadoresFinanceiros");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.PesquisaProjeto", b =>
                {
                    b.Property<int>("IdPesquisa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Comentarios")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ProjetoId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RepresentacaoTextualQuestionario")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<double>("ResultadoFinal")
                        .HasColumnType("double");

                    b.HasKey("IdPesquisa");

                    b.ToTable("PesquisaProjeto");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Pessoa", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Cargo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Telefone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("empresaId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("empresaId");

                    b.ToTable("Pessoa");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Producao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Autores")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Casa")
                        .HasColumnType("int");

                    b.Property<string>("DOI")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Descricao")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("EmpresaId")
                        .HasColumnType("int");

                    b.Property<int>("Grupo")
                        .HasColumnType("int");

                    b.Property<string>("Imagem")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Local")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NumPatente")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ProjetoId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Responsavel")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("StatusPub")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.HasIndex("ProjetoId");

                    b.ToTable("Producao");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Projeto", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("AreaPesquisa")
                        .HasColumnType("int");

                    b.Property<int>("Casa")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataEncerramento")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DuracaoProjetoEmMeses")
                        .HasColumnType("int");

                    b.Property<int?>("EmpresaId")
                        .HasColumnType("int");

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<int>("FonteFomento")
                        .HasColumnType("int");

                    b.Property<int>("Inovacao")
                        .HasColumnType("int");

                    b.Property<string>("NomeProjeto")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("ProponenteId")
                        .HasColumnType("int");

                    b.Property<float?>("SatisfacaoCliente")
                        .HasColumnType("float");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<double>("ValorAporteRecursos")
                        .HasColumnType("double");

                    b.Property<double>("ValorTotalProjeto")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.HasIndex("ProponenteId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Projeto");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.ProjetoIndicadores", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<bool>("Bolsista")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ComprasMaterial")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ComprasServico")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("IdProjeto")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<bool>("PrestacaoContas")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Regramento")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Relatorios")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Repasse")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SatisfacaoFimProjeto")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("SatisfacaoMetadeProjeto")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("IdProjeto");

                    b.ToTable("ProjetoIndicadores");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Prospeccao", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Agregadas")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("Ancora")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("CaminhoPasta")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Casa")
                        .HasColumnType("int");

                    b.Property<int?>("Contatoid")
                        .HasColumnType("int");

                    b.Property<int?>("EmpresaId")
                        .HasColumnType("int");

                    b.Property<int>("LinhaPequisa")
                        .HasColumnType("int");

                    b.Property<string>("MembrosEquipe")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NomeProspeccao")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Origem")
                        .HasColumnType("int");

                    b.Property<string>("PotenciaisParceiros")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Tags")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("TipoContratacao")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<decimal>("ValorEstimado")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("ValorProposta")
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("Contatoid");

                    b.HasIndex("EmpresaId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Prospeccao");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.StatusCurva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Financeiro")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("Fisico")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("ProjetoId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("ProjetoId");

                    b.ToTable("StatusCurva");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Submissao", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<bool>("ComEmpresa")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("EditalId")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("EditalId1")
                        .HasColumnType("int");

                    b.Property<string>("ProjetoProposto")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Proponente")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ProspeccaoId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ResponsavelSubmissao")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("StatusSubmissao")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EditalId1");

                    b.HasIndex("ProspeccaoId");

                    b.ToTable("Submissao");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Usuario", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int?>("CargoId")
                        .HasColumnType("int");

                    b.Property<int>("Casa")
                        .HasColumnType("int");

                    b.Property<string>("Competencia")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Foto")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Matricula")
                        .HasColumnType("int");

                    b.Property<int>("Nivel")
                        .HasColumnType("int");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Titulacao")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<int>("Vinculo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CargoId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.AtividadesProdutivas", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.EquipeProjeto", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Projeto", "Projeto")
                        .WithMany("EquipeProjeto")
                        .HasForeignKey("IdTrabalho");

                    b.HasOne("BaseDeProjetos.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.FollowUp", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Prospeccao", "Origem")
                        .WithMany("Status")
                        .HasForeignKey("OrigemID");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Pessoa", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Empresa", "empresa")
                        .WithMany()
                        .HasForeignKey("empresaId");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Producao", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId");

                    b.HasOne("BaseDeProjetos.Models.Projeto", "Projeto")
                        .WithMany()
                        .HasForeignKey("ProjetoId");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Projeto", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId");

                    b.HasOne("BaseDeProjetos.Models.Empresa", "Proponente")
                        .WithMany()
                        .HasForeignKey("ProponenteId");

                    b.HasOne("BaseDeProjetos.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.ProjetoIndicadores", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Projeto", "Projeto")
                        .WithMany("Indicadores")
                        .HasForeignKey("IdProjeto");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Prospeccao", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Pessoa", "Contato")
                        .WithMany()
                        .HasForeignKey("Contatoid");

                    b.HasOne("BaseDeProjetos.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId");

                    b.HasOne("BaseDeProjetos.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.StatusCurva", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Projeto", "Projeto")
                        .WithMany("StatusCurva")
                        .HasForeignKey("ProjetoId");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Submissao", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Editais", "Edital")
                        .WithMany("Submissoes")
                        .HasForeignKey("EditalId1");

                    b.HasOne("BaseDeProjetos.Models.Prospeccao", "Prospeccao")
                        .WithMany()
                        .HasForeignKey("ProspeccaoId");
                });

            modelBuilder.Entity("BaseDeProjetos.Models.Usuario", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Cargo", "Cargo")
                        .WithMany()
                        .HasForeignKey("CargoId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BaseDeProjetos.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BaseDeProjetos.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
