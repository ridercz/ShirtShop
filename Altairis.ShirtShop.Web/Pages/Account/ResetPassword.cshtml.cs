using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ShirtShop.Web.Pages.Account {
    public class ResetPasswordModel : PageModel {
        private readonly UserManager<ShopUser> _userManager;

        public ResetPasswordModel(UserManager<ShopUser> userManager) {
            this._userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel {

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            [Required, DataType(DataType.Password), Compare("Password")]
            public string ConfirmPassword { get; set; }

        }

        public async Task<IActionResult> OnPostAsync(string userId, string token) {
            if (!this.ModelState.IsValid) return this.Page();

            // Try to find user by ID
            var user = await this._userManager.FindByIdAsync(userId);

            // Redirect to done page if user does not exist to block account enumeration
            if (user == null) return this.RedirectToPage("ResetPasswordDone");

            // Try to reset password
            var result = await this._userManager.ResetPasswordAsync(
                user,
                token,
                this.Input.Password);

            if (result.Succeeded) {
                // OK, redirect to confirmation page
                return this.RedirectToPage("ResetPasswordDone");
            }
            else {
                // Failed, show errors
                foreach (var error in result.Errors) {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return this.Page();
        }
    }
}