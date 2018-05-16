using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Altairis.ShirtShop.Web.Services {
    public class ShopSignInManager : SignInManager<ShopUser> {
        public ShopSignInManager(UserManager<ShopUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<ShopUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<ShopUser>> logger, IAuthenticationSchemeProvider schemes)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes) {
        }

        public override async Task<bool> CanSignInAsync(ShopUser user) {
            if (!user.Enabled) {
                Logger.LogWarning(0, "User {userId} cannot sign in because is not enabled.",
                    await this.UserManager.GetUserIdAsync(user));
                return false;
            }
            return await base.CanSignInAsync(user);
        }

        public override async Task SignInAsync(ShopUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null) {
            await base.SignInAsync(user, authenticationProperties, authenticationMethod);

            // Update last login date
            user.LastLoginDate = DateTimeOffset.Now;
            var updateResult = await this.UserManager.UpdateAsync(user);
            if (!updateResult.Succeeded) {
                var errorList = updateResult.Errors.Select(x => $"{x.Code}: {x.Description}");
                throw new Exception("Failed to update user last login date: " + string.Join("; ", errorList));
            }
        }

    }
}
