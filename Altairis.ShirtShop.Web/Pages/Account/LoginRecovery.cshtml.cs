using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Altairis.ShirtShop.Data;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ShirtShop.Web.Pages.Account {
    public class LoginRecoveryModel : PageModel {
        private readonly SignInManager<ShopUser> _signInManager;

        public LoginRecoveryModel(SignInManager<ShopUser> signInManager) {
            this._signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel {
            [Required]
            public string RecoveryCode { get; set; }

        }

        public async Task<IActionResult> OnGetAsync() {
            var user = await this._signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null) return this.RedirectToPage("Login");
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            // Verify user is authenticated with password
            var user = await this._signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null) return this.RedirectToPage("Login");

            // Verify recovery code
            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(this.Input.RecoveryCode);

            // Redirect to 2FA setup target page when logged in
            if (result.Succeeded) return this.RedirectToPage("/Account/Manage/SetupOtp");

            // Show error otherwise
            this.ModelState.AddModelError(string.Empty, "Pøihlášení se nezdaøilo");
            return this.Page();
        }

    }

}