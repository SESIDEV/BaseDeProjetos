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
        }
        public DbSet<BaseDeProjetos.Models.Projeto> Projeto { get; set; }
        public DbSet<BaseDeProjetos.Models.Prospeccao> Prospeccao { get; set; }
        public DbSet<BaseDeProjetos.Models.Empresa> Empresa { get; set; }
    }
}
