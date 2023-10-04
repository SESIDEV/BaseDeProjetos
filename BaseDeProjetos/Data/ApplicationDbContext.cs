using BaseDeProjetos.Models;
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

        public DbSet<StatusCurva> StatusCurva { get; set; }
        public DbSet<Projeto> Projeto { get; set; }
        public DbSet<Prospeccao> Prospeccao { get; set; }
        public DbSet<FollowUp> FollowUp { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<AtividadesProdutivas> AtividadesProdutivas { get; set; }
        public DbSet<IndicadoresFinanceiros> IndicadoresFinanceiros { get; set; }
        public DbSet<Producao> Producao { get; set; }
        public DbSet<Editais> Editais { get; set; }
        public DbSet<Submissao> Submissao { get; set; }
        public DbSet<ProjetoIndicadores> ProjetoIndicadores { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<PesquisaProjeto> PesquisaProjeto { get; set; }
        public DbSet<EquipeProjeto> EquipeProjeto { get; set; }
        public DbSet<CodigoAmostraProjeto> CodigoAmostraProjeto { get; set; }
    }
}