using BaseDeProjetos.Data;
using BaseDeProjetos.Models;
using MailSenderApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BaseDeProjetos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (System.Environment.GetEnvironmentVariable("Ambiente") == "Web")
            {
                string ConStr = Configuration.GetConnectionString("localdb");
                ConStr = ConStr.Replace("Database", "database");
                ConStr = ConStr.Replace("Data Source", "server");
                ConStr = ConStr.Replace("User Id", "user");
                ConStr = ConStr.Replace("Password", "password");
                ConStr = ConStr.Replace("127.0.0.1", "localhost");

                // This line split "server=localhost:[port]" in "server=localhost;port=[port]
                ConStr = ConStr.Replace(":", ";port=");

                Console.WriteLine(ConStr);

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(ConStr, mysqlOptions => { mysqlOptions.ServerVersion(new Version(5, 7, 9), ServerType.MySql); }).UseLazyLoadingProxies());
            }
            else
            {
                string conn = Configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine(conn);
                services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseMySql(conn, mysqlOptions => { mysqlOptions.ServerVersion(new Version(5, 7, 9), ServerType.MySql); }).UseLazyLoadingProxies());
            }
            services.AddDefaultIdentity<Usuario>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDistributedMemoryCache();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ " +
                    "\u00e1\u00e0\u00e2\u00e3\u00e7\u00e9\u00ea\u00ed\u00f3\u00f4\u00f5\u00fa\u00fc" +
                    "\u00c1\u00c0\u00c2\u00c3\u00c7\u00c9\u00ca\u00cd\u00d3\u00d4\u00d5\u00da\u00dc";
            });

            services.Configure<RequestLocalizationOptions>(
                options =>
                    {
                        var supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("pt-BR"),
                        };

                        options.DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR");
                        options.SupportedCultures = supportedCultures;
                        options.SupportedUICultures = supportedCultures;
                    });

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSession();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDistributedMemoryCache();
            services.AddSingleton<DbCache>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    var contentType = context.Response.ContentType;
                    if (!string.IsNullOrWhiteSpace(contentType)
                        && !contentType.Contains("charset=", StringComparison.OrdinalIgnoreCase)
                        && (contentType.StartsWith("text/", StringComparison.OrdinalIgnoreCase)
                            || contentType.StartsWith("application/javascript", StringComparison.OrdinalIgnoreCase)
                            || contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase)))
                    {
                        context.Response.ContentType = $"{contentType}; charset=utf-8";
                    }

                    return System.Threading.Tasks.Task.CompletedTask;
                });

                await next();
            });

            app.UseStaticFiles();

            app.UseRequestLocalization();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });

            
        }
    }
}
