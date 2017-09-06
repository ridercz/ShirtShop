using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Altairis.Services.Mailing;
using Microsoft.Extensions.Options;

namespace Altairis.ShirtShop.Web.Pages {
    public class IndexModel : PageModel {

        private readonly ShirtDbContext context;
        private readonly ShopOptions options;
        private readonly IMailerService mailer;

        public IndexModel(ShirtDbContext context, IOptions<ShopOptions> options, IMailerService mailer) {
            this.context = context;
            this.options = options.Value;
            this.mailer = mailer;
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

            // Save to database
            await this.context.Orders.AddAsync(this.Order);
            await this.context.SaveChangesAsync();

            // Send mail to buyer
            var msg = new MailMessageDto {
                From = new MailAddressDto(this.options.Mailing.SenderEmail, this.options.Mailing.SenderName),
                Subject = $"[{this.Request.Host}] Nová objednávka trièka",
                BodyText = "Byla pøijata nová objednávka"
            };
            msg.To.Add(new MailAddressDto(this.Order.Email, this.Order.Name));
            await this.mailer.SendMessageAsync(msg);

            return this.RedirectToPage("OK", new { OrderId = this.Order.Id });
        }

    }
}