using System;
using System.IO;
using System.Linq;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Altairis.ShirtShop.UserImportUtility {
    class Program {

        static void Main(string[] args) {
            // Create DbContext
            var builder = new DbContextOptionsBuilder<ShopDbContext>();
            builder.UseSqlServer("SERVER=.\\SqlExpress;TRUSTED_CONNECTION=yes;DATABASE=ShirtShop_final");
            var dc = new ShopDbContext(builder.Options);

            // Process all lines except first (headers)
            var lines = File.ReadAllLines("OldUsers.csv");
            foreach (var line in lines.Skip(1)) {
                var lineData = line.Split(';');
                var newUser = CreateUser(lineData[0], lineData[1], lineData[2],
                                         lineData[3], lineData[4]);
                dc.Users.Add(newUser);
            }
            dc.SaveChanges();
        }

        static ShopUser CreateUser(string userName, string fullName, string emailAddress, string oldPasswordSalt, string oldPasswordHash) {
            // Create new user based on imported information
            var newUser = new ShopUser {
                AccessFailedCount = 0,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                Email = emailAddress,
                EmailConfirmed = true,
                FullName = fullName,
                Id = Guid.NewGuid().ToString(),
                LockoutEnabled = false,
                LockoutEnd = null,
                NormalizedEmail = emailAddress.ToUpperInvariant(),
                NormalizedUserName = userName.ToUpperInvariant(),
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = false,
                UserName = userName
            };

            // Hash old password hash again with default algorithm
            var defaultHasher = new PasswordHasher<ShopUser>();
            var defPasswordHash = defaultHasher.HashPassword(newUser, oldPasswordHash);
            var defPasswordHashData = Convert.FromBase64String(defPasswordHash);

            // Decode old password salt
            var oldPasswordSaltData = Convert.FromBase64String(oldPasswordSalt);

            // Create password hash data structure
            using (var ms = new MemoryStream()) {
                ms.WriteByte(0xFF); // Magic number - our custom hash type
                ms.WriteByte((byte)oldPasswordSaltData.Length);
                ms.Write(oldPasswordSaltData, 0, oldPasswordSaltData.Length);
                ms.Write(defPasswordHashData, 0, defPasswordHashData.Length);

                // Encode the data structure using Base64
                newUser.PasswordHash = Convert.ToBase64String(ms.ToArray());
            }

            return newUser;
        }
    }
}
