using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
// dodano
using Microsoft.Extensions.DependencyInjection;
using RezerwacjaSal.Data;
using Microsoft.AspNetCore.Builder;

namespace RezerwacjaSal
{
    public class Program
    {


        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<RezerwacjaSalContext>();

                    // inicjalizacja bazy danych wstępnymi danymi. Przekazanie RezerwacjaSalContext.
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .UseSetting("detailedErrors", "true")
                .UseIISIntegration()
                //.UseKestrel()
                .CaptureStartupErrors(true);




    }

}
