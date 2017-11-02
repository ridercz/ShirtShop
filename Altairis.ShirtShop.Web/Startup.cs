using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.Services.Mailing;
using Altairis.Services.Mailing.Rfc2822;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.ShirtShop.Web {
    public class Startup {
        private readonly IConfigurationRoot _config;

        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("config.json", optional: false)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this._config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthorization(options => {
                options.AddPolicy("IsLoggedIn", policy => policy.RequireAuthenticatedUser());
                options.AddPolicy("IsAdmin", policy => policy.RequireRole("Administrators"));
            });
            services.AddMvc()
                .AddRazorPagesOptions(options => {
                    options.Conventions.AuthorizeFolder("/admin", "IsAdmin");
                    options.Conventions.AuthorizeFolder("/account/manage", "IsLoggedIn");
                });
            services.AddDbContext<ShopDbContext>(options => {
                options.UseSqlServer(this._config.GetConnectionString("ShopDb"));
            });
            services.Configure<ShopConfig>(this._config);
            services.AddIdentity<ShopUser, IdentityRole>(options => {
                options.Password.RequiredLength = 12;
                options.Password.RequiredUniqueChars = 4;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ShopDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
            });

            var shopConfig = new ShopConfig();
            this._config.Bind(shopConfig);

            services.AddPickupFolderMailerService(new PickupFolderMailerServiceOptions {
                PickupFolderName = shopConfig.Mailing.PickupFolder,
                DefaultFrom = new MailAddressDto(shopConfig.Mailing.SenderAddress)
            });

            services.AddTransient<Bootstrapper>();
        }

        //public void Configure(IApplicationBuilder app, IHostingEnvironment env, ShopDbContext dc, UserManager<ShopUser> userManager) {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Bootstrapper b) {
            // Setup development server
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            // Add middleware
            app.UseAuthentication();
            app.UseMvc();
            app.UseStaticFiles();

            //// Prepare database
            //dc.Database.Migrate();
            //dc.Seed();

            //// Add first user
            //if (env.IsDevelopment() && !userManager.Users.Any()) {
            //    var adminUser = new ShopUser {
            //        UserName = "admin",
            //        Email = "admin@example.com",
            //        EmailConfirmed = true
            //    };
            //    var r = userManager.CreateAsync(adminUser, "pass.word123").Result;
            //    if (r != IdentityResult.Success) {
            //        var errors = string.Join(", ", r.Errors.Select(x => x.Description));
            //        throw new Exception("Seeding default user failed: " + errors);
            //    }
            //}

            // Run bootstrapper
            b.Initialize();
        }
    }
}
