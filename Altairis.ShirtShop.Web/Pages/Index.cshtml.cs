using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Altairis.ShirtShop.Data;

namespace Altairis.ShirtShop.Web.Pages {
    public class IndexModel : PageModel {

        private readonly ShirtDbContext context;

        public IndexModel(ShirtDbContext dc) {
            this.context = dc;
        }

        public IEnumerable<ShirtSize> ShirtSizes => this.context.ShirtSizes.OrderBy(x => x.SortPriority);

        public void OnGet() {

        }

    }
}