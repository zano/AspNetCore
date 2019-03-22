using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHostBuilder webHostBuilder = CreateWebHostBuilder(args);

            IWebHost webHost = webHostBuilder.Build();
            // instantiates Startup
            // calls Startup.ConfigureServices(serviceCollection)

            Prepare(webHost);

            webHost.Run();
            // calls Startup.Configure().
        }

        private static void Prepare(IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    context.Database.EnsureCreated();                    
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occured migrating the DB:\n{0}", ex.Message);
                    throw ex;
                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
