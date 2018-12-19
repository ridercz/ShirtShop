using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Altairis.ShirtShop.Data;

namespace Altairis.ShirtShop.Web.Pages.Account {
    public class ConfirmEmailModel : PageModel {
        private readonly UserManager<ShopUser> _userManager;

        public ConfirmEmailModel(UserManager<ShopUser> userManager) {
            this._userManager = userManager;
        }

        public string Message { get; set; }

        public async Task OnGetAsync(string userId, string token) {
            // Try to find user by ID
            var user = await this._userManager.FindByIdAsync(userId);
            if (user != null) {
                // Try to confirm e-mail using the token
                var result = await this._userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded) {
                    this.Message = "Váš účet byl úspěšně ověřen. Nyní se můžete přihlásit.";
                    return;
                }
            }
            this.Message = "Ověření e-mailové adresy se nezdařilo.";
        }
    }
}