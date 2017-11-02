using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ShirtShop.Data {
    //public class ShopDbContext : DbContext {
    public class ShopDbContext : IdentityDbContext<ShopUser> {

        public ShopDbContext(DbContextOptions<ShopDbContext> options)
            : base(options) {

        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ShirtSize> ShirtSizes { get; set; }

        public DbSet<ShirtType> ShirtTypes { get; set; }

        public void Seed() {
            if (!this.ShirtSizes.Any()) {
                this.ShirtSizes.Add(new ShirtSize { Name = "S", Price = 200, SortPriority = 10 });
                this.ShirtSizes.Add(new ShirtSize { Name = "M", Price = 250, SortPriority = 20 });
                this.ShirtSizes.Add(new ShirtSize { Name = "L", Price = 250, SortPriority = 30 });
                this.ShirtSizes.Add(new ShirtSize { Name = "XL", Price = 250, SortPriority = 40 });
                this.ShirtSizes.Add(new ShirtSize { Name = "XXL", Price = 250, SortPriority = 50 });
                this.ShirtSizes.Add(new ShirtSize { Name = "XXXL", Price = 300, SortPriority = 60 });
            }
            if (!this.ShirtTypes.Any()) {
                this.ShirtTypes.Add(new ShirtType { Name = "Červená", SortPriority = 10 });
                this.ShirtTypes.Add(new ShirtType { Name = "Bílá", SortPriority = 20 });
                this.ShirtTypes.Add(new ShirtType { Name = "Černá", SortPriority = 30 });
            }
            this.SaveChanges();
        }


    }
}
