using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// dodatkowe ficzery
using Microsoft.EntityFrameworkCore;
using RezerwacjaSal.Data;
using Microsoft.AspNetCore.Http;

namespace RezerwacjaSal
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
            // zdefiniowanie RezerwacjaSalContext jako us�ugi
            services.AddDbContext<RezerwacjaSalContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); // nazwa domy�lnego po��czenia z baz� danych w appsettings.json
            services.AddMvc();
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseStaticFiles();


            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
