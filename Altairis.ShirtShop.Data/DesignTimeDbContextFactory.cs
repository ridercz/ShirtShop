using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Altairis.ShirtShop.Data {
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ShopDbContext> {
    public ShopDbContext CreateDbContext(string[] args) {
        var builder = new DbContextOptionsBuilder<ShopDbContext>();
        builder.UseSqlServer("SERVER=.\\SqlExpress;TRUSTED_CONNECTION=yes;DATABASE=ShirtShop_design");
        return new ShopDbContext(builder.Options);
    }
}
}
