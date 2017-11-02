using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ShirtShop.Web.Pages.Account {
    public class ResetPasswordModel : PageModel {
        private readonly UserManager<ShopUser> _userManager;

        public ResetPasswordModel(UserManager<ShopUser> userManager) {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel {

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password), Compare("Password")]
            public string ConfirmPassword { get; set; }

        }

        public async Task<IActionResult> OnPostAsync(string userId, string token) {
            if (!ModelState.IsValid) return Page();

            // Try to find user by ID
            var user = await _userManager.FindByIdAsync(userId);

            // Redirect to done page if user does not exist to block account enumeration
            if (user == null) return this.RedirectToPage("ResetPasswordDone");

            // Try to reset password
            var result = await _userManager.ResetPasswordAsync(
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
            return Page();
        }
    }
}