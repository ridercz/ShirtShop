using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ShirtShop.Web.Pages.Account {
    public class LoginExternalCallbackModel : PageModel {
        private readonly SignInManager<ShopUser> _signInManager;

        public LoginExternalCallbackModel(SignInManager<ShopUser> signInManager) {
            _signInManager = signInManager;
        }

        public string MessageTitle { get; set; }

        public string MessageText { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null, string remoteError = null) {
            // Display remote error
            if (!string.IsNullOrEmpty(remoteError)) {
                this.MessageTitle = "Chyba pøihlášení";
                this.MessageText = remoteError;
                return this.Page();
            }

            // Get external login info
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return this.RedirectToPage("Login");

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (result.Succeeded) return this.LocalRedirect(returnUrl);
            this.MessageTitle = "Pøihlášení se nezdaøilo";
            this.MessageText = "Je možné, že vzdálená identita není pøiøazena k žádnému lokálnímu úètu.";
            return this.Page();
        }
    }

}
