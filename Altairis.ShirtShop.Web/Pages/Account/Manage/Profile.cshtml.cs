using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ShirtShop.Web.Pages.Account.Manage {
    public class ProfileModel : PageModel {
        private readonly UserManager<ShopUser> _userManager;

        public ProfileModel(UserManager<ShopUser> userManager) {
            this._userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, MaxLength(50)]
            public string FullName { get; set; }
        }

        public async Task OnGetAsync() {
            var user = await this._userManager.GetUserAsync(this.User);
            this.Input.FullName = user.FullName;
        }

        public async Task<IActionResult> OnPostAsync() {
            if (this.ModelState.IsValid) {
                var user = await this._userManager.GetUserAsync(this.User);
                user.FullName = this.Input.FullName;
                var result = await this._userManager.UpdateAsync(user);
                if (result.Succeeded) return this.RedirectToPage("ProfileDone");
                foreach (var item in result.Errors) {
                    this.ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return this.Page();
        }

    }
}