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

        public LogoutModel(SignInManager<ShopUser> signInManager) {
            _signInManager = signInManager;
        }

        public async Task OnGetAsync() {
            await this._signInManager.SignOutAsync();
        }
    }
}