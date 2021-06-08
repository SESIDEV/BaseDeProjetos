using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaseDeProjetos.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
                    }
        public DbSet<BaseDeProjetos.Models.Projeto> Projeto { get; set; }
        public DbSet<BaseDeProjetos.Models.Prospeccao> Prospeccao { get; set; }
        public DbSet<BaseDeProjetos.Models.FollowUp> FollowUp { get; set; }
        public DbSet<BaseDeProjetos.Models.Empresa> Empresa { get; set; }
        public DbSet<BaseDeProjetos.Models.Pessoa> Pessoa { get; set; }
        public DbSet<BaseDeProjetos.Models.Entrega> Entrega { get; set; }
        public DbSet<BaseDeProjetos.Models.AtividadesProdutivas> AtividadesProdutivas { get; set; }
        public DbSet<BaseDeProjetos.Models.Producao> Producao { get; set; }
        public DbSet<BaseDeProjetos.Models.IndicadoresFinanceiros> IndicadoresFinanceiros { get; set; }
    }
}
