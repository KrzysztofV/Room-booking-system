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
using Microsoft.AspNetCore.Identity;
using RezerwacjaSal.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

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
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var dbInitializerLogger = services.GetRequiredService<ILogger<DbInitializer>>();
                    DbInitializer.InitializeAsync(context, userManager, roleManager, dbInitializerLogger).Wait();
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
                .ConfigureKestrel((context, options) =>
                {
                    options.Limits.MaxConcurrentConnections = 100;
                    options.Limits.MaxConcurrentUpgradedConnections = 100;
                    options.Limits.MaxRequestBodySize = 10 * 1024;
                    options.Limits.MinRequestBodyDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Limits.MinResponseDataRate =
                        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                    options.Listen(IPAddress.Parse("0.0.0.0"), 80);
                    options.Listen(IPAddress.Parse("0.0.0.0"), 443, listenOptions =>
                    {
                        listenOptions.UseHttps("bulbulator.pfx", Environment.GetEnvironmentVariable("CertPassword"));
                    });
                })
                .UseApplicationInsights()
                .UseStartup<Startup>()
                .UseSetting("detailedErrors", "true")
                .UseIISIntegration()
                .CaptureStartupErrors(true);




    }

}
