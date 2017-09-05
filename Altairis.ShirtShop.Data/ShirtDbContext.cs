using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altairis.ShirtShop.Data {
    public class ShirtDbContext : DbContext {

        // Constructors

        public ShirtDbContext(DbContextOptions options) : base(options) { }

        // Tables

        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ShirtModel> ShirtModels { get; set; }

        public DbSet<ShirtSize> ShirtSizes { get; set; }

        // Initial seed

        public void Seed() {
            if (!this.DeliveryMethods.Any()) {
                this.DeliveryMethods.Add(new DeliveryMethod { Name = "Osobní převzetí", Price = 0 });
                this.DeliveryMethods.Add(new DeliveryMethod { Name = "Kurýrní služba PPL", Price = 100 });
            }
            if (!this.ShirtModels.Any()) {
                this.ShirtModels.Add(new ShirtModel { Name = "My jsme ti lidé, před kterými nás rodiče varovali" });
                this.ShirtModels.Add(new ShirtModel { Name = "We are the people our parents warned us about" });
                this.ShirtModels.Add(new ShirtModel { Name = "Život bolí, ale někdo to má rád" });
                this.ShirtModels.Add(new ShirtModel { Name = "I was into BDSM before Fifty shades of Gray" });
                this.ShirtModels.Add(new ShirtModel { Name = "Byl jsem na BDSM předtím, než vydali 50 odstínů šedi" });
                this.ShirtModels.Add(new ShirtModel { Name = "Save the horse, ride a ponyboy!" });
                this.ShirtModels.Add(new ShirtModel { Name = "Save the horse, ride a ponygirl!" });
            }
            if (!this.ShirtSizes.Any()) {
                this.ShirtSizes.Add(new ShirtSize { Name = "Pánské XS", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Pánské S", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Pánské M", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Pánské L", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Pánské XL", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Pánské XXL", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Pánské XXXL", Price = 270 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Pánské XXXXL", Price = 270 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Dámské XS", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Dámské S", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Dámské M", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Dámské L", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Dámské XL", Price = 250 });
                this.ShirtSizes.Add(new ShirtSize { Name = "Dámské XXL", Price = 250 });
            }
            this.SaveChanges();
        }

    }
}
