using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altairis.ShirtShop.Web.Components {
    public class MenuViewComponent : ViewComponent {
        private readonly UserManager<ShopUser> _userManager;

        public MenuViewComponent(UserManager<ShopUser> userManager) {
            this._userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            if (!this.User.Identity.IsAuthenticated) {
                // Anonymous user
                return this.View("Anonymous");
            }

            // Get non-anonymous user
            var user = await _userManager.FindByNameAsync(this.User.Identity.Name);
            var model = new MenuModel {
                UserName = user.UserName,
                FullName = user.FullName
            };

            // Return view for user or admin
            var isAdmin = this.User.IsInRole("Administrators");
            return this.View(isAdmin ? "Admin" : "User", model);
        }

    }

    public class MenuModel {

        public string UserName { get; set; }

        public string FullName { get; set; }

    }

}
