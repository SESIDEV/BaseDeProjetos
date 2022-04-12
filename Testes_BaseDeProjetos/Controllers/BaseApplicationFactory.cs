using BaseDeProjetos.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Testes_BaseDeProjetos.Controllers
{
    public class BaseApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        public BaseApplicationFactory()
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                ServiceProvider serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkMySql()
                    .BuildServiceProvider();

                // Irei utilizar a base de testes local. NÂO UTILIZAR EM PRODUÇÃO
                services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseMySql("server=localhost;userid=root;password=f9ry4oo2;database=basedb;port=3306").
                        UseLazyLoadingProxies();
                    });

                // Build the service provider.
                ServiceProvider sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (IServiceScope scope = sp.CreateScope())
                {
                    IServiceProvider scopedServices = scope.ServiceProvider;
                    ApplicationDbContext db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    ILogger<BaseApplicationFactory<TStartup>> logger = scopedServices
                        .GetRequiredService<ILogger<BaseApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();
                    try
                    {
                        // Seed the database with test data.
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}