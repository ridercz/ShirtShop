using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Altairis.ShirtShop.Web {
    public class Bootstrapper {
        private const string DEFAULT_PASSWORD = "pass.word123";
        private const string ROLE_NAME = "Administrators";

        private readonly UserManager<ShopUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ShopDbContext _context;

        public Bootstrapper(UserManager<ShopUser> userManager, RoleManager<IdentityRole> roleManager, ShopDbContext context) {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._context = context;
        }

        public void Initialize() {
            this.InitDatabase();
            this.InitIdentity().Wait();
        }

        private void InitDatabase() {
            // Migrate DB to current version
            this._context.Database.Migrate();

            // Seed initial record
            this._context.Seed();
        }

private async Task InitIdentity() {
    // Create Administrators role if not exists
    var roleExists = await this._roleManager.RoleExistsAsync(ROLE_NAME);
    if (!roleExists) {
        EnsureIdentitySuccess(() => this._roleManager.CreateAsync(new IdentityRole(ROLE_NAME)));
    }

    // Create Admin user if not exists
    var adminUser = await this._userManager.FindByNameAsync("admin");
    if (adminUser == null) {
        adminUser = new ShopUser {
            UserName = "admin",
            Email = "admin@example.com",
            EmailConfirmed = true,
            FullName = "System Administrator"
        };
        EnsureIdentitySuccess(() => this._userManager.CreateAsync(adminUser, DEFAULT_PASSWORD));
    }

    // Add Admin user to Administrators role if not already member
    var isInRole = await this._userManager.IsInRoleAsync(adminUser, ROLE_NAME);
    if (!isInRole) {
        EnsureIdentitySuccess(() => this._userManager.AddToRoleAsync(adminUser, ROLE_NAME));
    }
}

        private static void EnsureIdentitySuccess(Func<Task<IdentityResult>> op) {
            var result = op().Result;
            if (result == IdentityResult.Success) return;
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new Exception("Seeding default user failed: " + errors);
        }
    }
}
