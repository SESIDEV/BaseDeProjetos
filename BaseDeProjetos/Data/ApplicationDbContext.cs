using BaseDeProjetos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BaseDeProjetos.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            /* TODO: Configurar uma lista de destinatarios para passar como dependência */
            Usuario leon = new Usuario { UserName = "Leon Nascimento", Email = "lednascimento@firjan.com.br" };
            Usuario chefe = new Usuario { UserName = "Antonio Fidalgo", Email = "aaneto@firjan.com.br" };
            Destinatarios.Add(leon);
            Destinatarios.Add(chefe);
        }
        public DbSet<BaseDeProjetos.Models.Projeto> Projeto { get; set; }
        public DbSet<BaseDeProjetos.Models.Prospeccao> Prospeccao { get; set; }
        public DbSet<BaseDeProjetos.Models.FollowUp> FollowUp { get; set; }
        public DbSet<BaseDeProjetos.Models.Empresa> Empresa { get; set; }
        public DbSet<BaseDeProjetos.Models.Pessoa> Pessoa { get; set; }

        public List<Usuario> Destinatarios { get; set; } = new List<Usuario>();
    }
}
