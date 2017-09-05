using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Altairis.ShirtShop.Data;

namespace Altairis.ShirtShop.Web.Pages {
    public class IndexModel : PageModel {

        private readonly ShirtDbContext _context;

        public IndexModel(ShirtDbContext dc) {
            this._context = dc;
        }

        public IEnumerable<ShirtSize> ShirtSizes { get; set; }

        public void OnGet() {
            this.ShirtSizes = this._context.ShirtSizes;

        }
    }
}