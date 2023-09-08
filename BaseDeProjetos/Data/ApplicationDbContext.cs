﻿using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BaseDeProjetos.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {


        }

        public DbSet<BaseDeProjetos.Models.StatusCurva> StatusCurva { get; set; }
        public DbSet<BaseDeProjetos.Models.Projeto> Projeto { get; set; }
        public DbSet<BaseDeProjetos.Models.Prospeccao> Prospeccao { get; set; }
        public DbSet<BaseDeProjetos.Models.FollowUp> FollowUp { get; set; }
        public DbSet<BaseDeProjetos.Models.Empresa> Empresa { get; set; }
        public DbSet<BaseDeProjetos.Models.Pessoa> Pessoa { get; set; }
        public DbSet<BaseDeProjetos.Models.AtividadesProdutivas> AtividadesProdutivas { get; set; }
        public DbSet<BaseDeProjetos.Models.IndicadoresFinanceiros> IndicadoresFinanceiros { get; set; }
        public DbSet<BaseDeProjetos.Models.Producao> Producao { get; set; }
        public DbSet<BaseDeProjetos.Models.Editais> Editais { get; set; }
        public DbSet<BaseDeProjetos.Models.Submissao> Submissao { get; set; }
        public DbSet<BaseDeProjetos.Models.ProjetoIndicadores> ProjetoIndicadores { get; set; }
        public DbSet<BaseDeProjetos.Models.Cargo> Cargo { get; set; }
        public DbSet<BaseDeProjetos.Models.Maquina> Maquina { get; set; }
		public DbSet<BaseDeProjetos.Models.PesquisaProjeto> PesquisaProjeto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StatusCurva>()
                .HasOne(sc => sc.Projeto)
                .WithMany(p => p.StatusCurva)
                .HasForeignKey(sc => sc.ProjetoId)
                .OnDelete(DeleteBehavior.Restrict); // Esta linha configura a ação de exclusão como RESTRICT
        }


	}
}