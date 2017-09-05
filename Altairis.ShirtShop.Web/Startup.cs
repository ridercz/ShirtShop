using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Altairis.ShirtShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Altairis.ShirtShop.Web {
    public class Startup {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: false)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();
            services.Configure<ShopOptions>(this.Configuration);
            services.AddDbContext<ShirtDbContext>(options => options.UseSqlite($"Data Source = {this.Configuration["DbFileName"]}"));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ShirtDbContext dc) {
            // Setup development environment
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            // Setup web handling
            app.UseStaticFiles();
            app.UseMvc();

            // Setup database
            if (dc.Database.EnsureCreated()) dc.Seed();
        }

    }
}
