using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.Services.Mailing;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Altairis.ShirtShop.Web.Pages {
    public class IndexModel : PageModel {
        private readonly ShopDbContext _context;
        private readonly IMailerService _mailer;
        private readonly ShopConfig _config;

        public IndexModel(ShopDbContext context, IMailerService mailer, IOptions<ShopConfig> config) {
            this._context = context;
            this._mailer = mailer;
            this._config = config.Value;
        }

        public IEnumerable<SelectListItem> ShirtSizes => this._context.ShirtSizes
            .OrderBy(x => x.SortPriority)
            .Select(x => new SelectListItem {
                Text = $"{x.Name} ({x.Price} Kč)",
                Value = x.Id.ToString()
            });

        public IEnumerable<SelectListItem> ShirtTypes => this._context.ShirtTypes
            .OrderBy(x => x.SortPriority)
            .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() });

[BindProperty]
public Order Order { get; set; }

        public async Task<IActionResult> OnPostAsync() {
            // Validate input
            if (!this.ModelState.IsValid) return this.Page();

            // Set date created (overposting protection)
            this.Order.DateCreated = DateTime.Now;

            // Save to database
            await this._context.Orders.AddAsync(this.Order);
            await this._context.SaveChangesAsync();

            // Send order confirmation message to customer
            var custMsg = new MailMessageDto {
                Subject = "Potvrzení vaší objednávky",
                BodyText = $"Potvrzujeme, že vaše objednávka byla přijata pod číslem {this.Order.Id}."
            };
            custMsg.To.Add(new MailAddressDto(this.Order.EmailAddress));
            await this._mailer.SendMessageAsync(custMsg);

            // Send new order notification to shop operator
            var operMsg = new MailMessageDto {
                Subject = "Nová objednávka na e-shopu",
                BodyText = $"E-shop přijal novou objednávku číslo {this.Order.Id}."
            };
            operMsg.To.Add(new MailAddressDto(this.Order.EmailAddress));
            await this._mailer.SendMessageAsync(operMsg);

            // Redirect user to confirmation page
            return this.RedirectToPage("OrderConfirmation", new { OrderId = this.Order.Id });
        }

    }
}