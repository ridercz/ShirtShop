using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ShirtShop.Web.Pages.Account {
    public class LoginExternalModel : PageModel {
        private readonly SignInManager<ShopUser> _signInManager;

        public LoginExternalModel(SignInManager<ShopUser> signInManager) {
            _signInManager = signInManager;
        }

        public IActionResult OnGet(string idpName, string returnUrl = "/") {
            // Redirect to the external login provider
            var redirectUrl = Url.Page("LoginExternalCallback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(idpName, redirectUrl);
            var result = new ChallengeResult(idpName, properties) as IActionResult;
            return result ?? this.Page();
        }
    }
}