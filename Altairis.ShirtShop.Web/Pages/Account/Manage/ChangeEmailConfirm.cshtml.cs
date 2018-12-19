using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Altairis.ShirtShop.Data;

namespace Altairis.ShirtShop.Web.Pages.Account.Manage {
    public class ChangeEmailConfirmModel : PageModel {
    private readonly UserManager<ShopUser> _userManager;

    public ChangeEmailConfirmModel(UserManager<ShopUser> userManager) {
        this._userManager = userManager;
    }

    public string Message { get; set; }

    public async Task OnGetAsync(string newEmail, string token) {
        // Get user
        var user = await this._userManager.GetUserAsync(this.User);

        // Try to change e-mail address
        var result = await this._userManager.ChangeEmailAsync(user, newEmail, token);
        if (result.Succeeded) {
            this.Message = "Zmìna e-mailu byla úspìšnì potvrzena.";
        }
        else {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            this.Message = "Nepodaøilo se potvrdit zmìnu e-mailu: " + errors;
        }
    }
}
}