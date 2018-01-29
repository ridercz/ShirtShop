using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ShirtShop.Web.Pages.Account.Manage {
public class ExternalLoginsModel : PageModel {
    private readonly SignInManager<ShopUser> _signInManager;
    private readonly UserManager<ShopUser> _userManager;

    public ExternalLoginsModel(SignInManager<ShopUser> signInManager, UserManager<ShopUser> userManager) {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public IEnumerable<UserLoginInfo> CurrentLogins { get; set; }

    public IEnumerable<AuthenticationScheme> AvailableLogins { get; set; }

    public async Task OnGetAsync() {
        var user = await _userManager.GetUserAsync(this.User);
        this.CurrentLogins = await _userManager.GetLoginsAsync(user);
        this.AvailableLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
            .Where(x => !this.CurrentLogins.Any(y => x.Name.Equals(y.LoginProvider)));
    }

    public async Task<IActionResult> OnGetInitiateAsync(string idpName) {
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // Request a redirect to the external login provider to link a login for the current user
        var redirectUrl = Url.Page("ExternalLogins", pageHandler: "Callback");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(idpName, redirectUrl, _userManager.GetUserId(this.User));
        return new ChallengeResult(idpName, properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync() {
        // Get and assing external login info
        var user = await _userManager.GetUserAsync(User);
        var info = await _signInManager.GetExternalLoginInfoAsync(user.Id);
        if (info == null) throw new Exception("Cannot get external login info.");
        var result = await _userManager.AddLoginAsync(user, info);
        if (!result.Succeeded) throw new Exception("Cannot add external login to user.");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        return RedirectToPage();
    }

}
}