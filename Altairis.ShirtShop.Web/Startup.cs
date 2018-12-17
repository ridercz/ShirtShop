using System;
using Altairis.Services.Mailing;
using Altairis.Services.Mailing.Rfc2822;
using Altairis.ShirtShop.Data;
using Altairis.ShirtShop.Web.Services;
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
                .AddUserSecrets<Startup>()
                .AddEnvironmentVariables();
            this._config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) {
            var authenticationBuilder = services.AddAuthentication();
            if (!string.IsNullOrEmpty(this._config["IdentityProviders:Microsoft:ClientId"])) {
                authenticationBuilder.AddMicrosoftAccount(options => {
                    options.ClientId = this._config["IdentityProviders:Microsoft:ClientId"];
                    options.ClientSecret = this._config["IdentityProviders:Microsoft:ClientSecret"];
                });
            }
            if (!string.IsNullOrEmpty(this._config["IdentityProviders:Facebook:ClientId"])) {
                authenticationBuilder.AddFacebook(options => {
                    options.ClientId = this._config["IdentityProviders:Facebook:ClientId"];
                    options.ClientSecret = this._config["IdentityProviders:Facebook:ClientSecret"];
                });
            }
            if (!string.IsNullOrEmpty(this._config["IdentityProviders:Google:ClientId"])) {
                authenticationBuilder.AddGoogle(options => {
                    options.ClientId = this._config["IdentityProviders:Google:ClientId"];
                    options.ClientSecret = this._config["IdentityProviders:Google:ClientSecret"];
                });
            }
            if (!string.IsNullOrEmpty(this._config["IdentityProviders:Twitter:ClientId"])) {
                authenticationBuilder.AddTwitter(options => {
                    options.ConsumerKey = this._config["IdentityProviders:Twitter:ClientId"];
                    options.ConsumerSecret = this._config["IdentityProviders:Twitter:ClientSecret"];
                });
            }
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
            //services.AddScoped<IPasswordHasher<ShopUser>, NullPasswordHasher<ShopUser>>();
            services.AddScoped<IPasswordHasher<ShopUser>, UpgradePasswordHasher<ShopUser>>();
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
                .AddDefaultTokenProviders()
                .AddSignInManager<ShopSignInManager>();
            services.Configure<SecurityStampValidatorOptions>(options => {
                options.ValidationInterval = TimeSpan.Zero;
            });
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

            // Login approval infrastructure
            services.AddHttpContextAccessor();
            services.AddSingleton<ILoginApprovalSessionStore>(new InMemoryLoginApprovalSessionStore());
            services.AddSingleton<LoginApprovalManager>();
        }

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

            // Run bootstrapper
            b.Initialize();
        }
    }
}
