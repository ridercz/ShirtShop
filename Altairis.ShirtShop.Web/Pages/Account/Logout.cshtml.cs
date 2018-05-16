using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ShirtShop.Web.Pages.Account {
    public class LogoutModel : PageModel {
        private readonly SignInManager<ShopUser> _signInManager;

        [BindProperty]
        public bool AllDevices { get; set; }

        public LogoutModel(SignInManager<ShopUser> signInManager) {
            _signInManager = signInManager;
        }

        //public async Task OnGetAsync() {
        //    await this._signInManager.SignOutAsync();
        //}

        public async Task<IActionResult> OnPostAsync() {
            if (this.AllDevices) {
                var user = await _signInManager.UserManager.GetUserAsync(this.User);
                await _signInManager.UserManager.UpdateSecurityStampAsync(user);
            }
            await _signInManager.SignOutAsync();
            return this.RedirectToPage("LogoutDone");
        }

        public IActionResult OnPostCancel() {
            return this.LocalRedirect("/");
        }

    }
}