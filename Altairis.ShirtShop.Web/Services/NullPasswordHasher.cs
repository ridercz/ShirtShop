using System;
using Microsoft.AspNetCore.Identity;

namespace Altairis.ShirtShop.Web.Services {
    public class NullPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class {

        public string HashPassword(TUser user, string password) {
            //throw new NotImplementedException();
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword) {
            //throw new NotImplementedException();
            if (hashedPassword.Equals(providedPassword)) {
                return PasswordVerificationResult.Success;
            }
            else {
                return PasswordVerificationResult.Failed;
            }
        }

    }
}
