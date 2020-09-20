using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BaseDeProjetos.Areas.Identity.IdentityHostingStartup))]
namespace BaseDeProjetos.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}