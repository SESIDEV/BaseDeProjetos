using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BaseDeProjetos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .ConfigureKestrel((context, options) => {
                        // ViewComponents possuem um bug em que sem isso eles morrem...
                        // Issue no GitHub: https://github.com/dotnet/aspnetcore/issues/40928
                        options.AllowSynchronousIO = true;
                    });
                });
        }
    }
}