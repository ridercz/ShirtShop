using System.Collections.Generic;
using System.Linq;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.ShirtShop.Web.Pages.Admin {
    public class IndexModel : PageModel {

        private readonly ShopDbContext _context;

        public IndexModel(ShopDbContext context) {
            this._context = context;
            this.Orders = this._context.Orders
                .Include(x => x.ShirtSize)
                .Include(x => x.ShirtType)
                .OrderByDescending(x => x.DateCreated)
                .ToList();
        }

        public IList<Order> Orders { get; }

        public void OnGet() {
        }

    }
}