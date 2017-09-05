using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.ShirtShop.Web.Pages {
    public class IndexModel : PageModel {

        private readonly ShirtDbContext context;

        public IndexModel(ShirtDbContext dc) {
            this.context = dc;
        }

        public IEnumerable<SelectListItem> ShirtSizes => this.context.ShirtSizes
            .OrderBy(x => x.SortPriority)
            .Select(x => new SelectListItem {
                Text = $"{x.Name} ({x.Price} Kè)",
                Value = x.Id.ToString()
            });

        public IEnumerable<SelectListItem> ShirtModels => this.context.ShirtModels
            .OrderBy(x => x.SortPriority)
            .Select(x => new SelectListItem {
                Text = x.Name,
                Value = x.Id.ToString()
            });

        public IEnumerable<SelectListItem> DeliveryMethods => this.context.DeliveryMethods
            .OrderBy(x => x.SortPriority)
            .Select(x => new SelectListItem {
                Text = $"{x.Name} ({x.Price} Kè)",
                Value = x.Id.ToString()
            });

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) return this.Page();

            await this.context.Orders.AddAsync(this.Order);
            await this.context.SaveChangesAsync();

            return this.RedirectToPage("OK", new { OrderId = this.Order.Id });
        }

    }
}