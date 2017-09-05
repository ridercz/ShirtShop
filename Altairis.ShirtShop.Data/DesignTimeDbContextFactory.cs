using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altairis.ShirtShop.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ShirtDbContext> {
        public ShirtDbContext CreateDbContext(string[] args) {
            var builder = new DbContextOptionsBuilder<ShirtDbContext>();
            builder.UseSqlite(connectionString: "Data Source = shirtshop.db");
            return new ShirtDbContext(builder.Options);
        }
    }
}
