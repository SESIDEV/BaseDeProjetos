using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BaseDeProjetos.Models;

namespace BaseDeProjetos.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            var chefe = new Usuario { UserName = "Antonio Fidalgo", Email = "aaneto@firjan.com.br" };
            var leon = new Usuario { UserName = "Leon Nascimento", Email = "lednascimento@firjan.com.br" };
            //Destinatarios.Add(chefe); 
            Destinatarios.Add(leon); 
        }
        public DbSet<BaseDeProjetos.Models.Projeto> Projeto { get; set; }
        public DbSet<BaseDeProjetos.Models.Prospeccao> Prospeccao { get; set; }
        public DbSet<BaseDeProjetos.Models.FollowUp> FollowUp { get; set; }
        public DbSet<BaseDeProjetos.Models.Empresa> Empresa { get; set; }
        public DbSet<BaseDeProjetos.Models.Pessoa> Pessoa { get; set; }

        public List<Usuario> Destinatarios { get; set; } = new List<Usuario>();
    }
}
