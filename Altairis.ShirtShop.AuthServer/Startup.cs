using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.ShirtShop.AuthServer {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddIdentityServer()
                .AddInMemoryClients(InMemoryRepository.GetClients())
                .AddTestUsers(InMemoryRepository.GetUsers().ToList())
                .AddInMemoryIdentityResources(InMemoryRepository.GetIdentityResources())
                .AddDeveloperSigningCredential();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();

            //app.Run(async (context) => {
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
