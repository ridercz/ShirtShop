using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ShirtShop.Web.Services {
    public class UpgradePasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class {
        private readonly IPasswordHasher<TUser> _defaultHasher = new PasswordHasher<TUser>();

        public string HashPassword(TUser user, string password) {
            return _defaultHasher.HashPassword(user, password);
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword,
                                                                string providedPassword) {
            // Base64 decode hashed password
            var hashedPasswordData = Convert.FromBase64String(hashedPassword);

            // If it does not start with our magic number, use default hasher
            if (hashedPasswordData[0] != 0xFF) {
                return _defaultHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
            }

            // It starts with our magic number, so get the original salt and twice-hashed hash
            ParseHashedPasswordStructure(hashedPasswordData, out var salt, out var hashString);

            // Hash provided password with old algorithm
            var providedPasswordData = Encoding.UTF8.GetBytes(providedPassword);
            string oldHashString;
            using (var mac = new HMACSHA256(salt)) {
                var oldHash = mac.ComputeHash(providedPasswordData);
                oldHashString = Convert.ToBase64String(oldHash);
            }

            // Verify the old hash using default hasher
            var result = _defaultHasher.VerifyHashedPassword(user, hashString, oldHashString);

            // Request rehash when needed
            if (result == PasswordVerificationResult.Success) {
                return PasswordVerificationResult.SuccessRehashNeeded;
            }
            else {
                return result;
            }
        }

        private static void ParseHashedPasswordStructure(byte[] data, out byte[] salt, out string passwordHash) {
            // Get raw salt data
            var saltLength = data[1];
            salt = new byte[saltLength];
            Array.Copy(data, 2, salt, 0, saltLength);

            // Get raw hash data
            var hashIndex = 2 + saltLength;
            var hashLength = data.Length - hashIndex;
            var hash = new byte[hashLength];
            Array.Copy(data, hashIndex, hash, 0, hashLength);

            // Base64 encode
            passwordHash = Convert.ToBase64String(hash);
        }
    }
}
