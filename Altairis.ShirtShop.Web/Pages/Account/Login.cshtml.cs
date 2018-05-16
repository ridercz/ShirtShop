using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ShirtShop.Web.Pages.Account {
public class LoginModel : PageModel {
    private readonly SignInManager<ShopUser> _signInManager;

    public LoginModel(SignInManager<ShopUser> signInManager) {
        _signInManager = signInManager;

        this.IdentityProviders = _signInManager.GetExternalAuthenticationSchemesAsync().Result.ToList();
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> IdentityProviders { get; set; }

    public class InputModel {
        [Required]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = "/") {
        if (this.ModelState.IsValid) {
            var result = await _signInManager.PasswordSignInAsync(
                this.Input.UserName,
                this.Input.Password,
                this.Input.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded) {
                return this.LocalRedirect(returnUrl);
            }
            else if (result.RequiresTwoFactor) {
                return RedirectToPage("LoginOtp", new {
                    ReturnUrl = returnUrl,
                    RememberMe = this.Input.RememberMe
                });
            }
            else {
                this.ModelState.AddModelError(string.Empty, "Přihlášení se nezdařilo");
            }
        }
        return Page();
    }
}
}