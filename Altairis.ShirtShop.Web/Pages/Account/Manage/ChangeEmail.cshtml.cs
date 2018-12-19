using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Altairis.ShirtShop.Data;
using System.ComponentModel.DataAnnotations;
using Altairis.Services.Mailing;

namespace Altairis.ShirtShop.Web.Pages.Account.Manage {
    public class ChangeEmailModel : PageModel {
        private readonly UserManager<ShopUser> _userManager;
        private readonly IMailerService _mailerService;

        public ChangeEmailModel(UserManager<ShopUser> userManager, IMailerService mailerService) {
            this._userManager = userManager;
            this._mailerService = mailerService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            [Required, MaxLength(50), EmailAddress]
            public string NewEmail { get; set; }

        }

        public async Task OnGetAsync() {
            var user = await this._userManager.GetUserAsync(this.User);
            this.Input.NewEmail = user.Email;
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) return this.Page();

            // Check if the address is really changed
            var user = await this._userManager.GetUserAsync(this.User);
            if (user.Email.Equals(this.Input.NewEmail, StringComparison.OrdinalIgnoreCase)) return this.Page();

            // Check password
            var passwordCorrect = await this._userManager.CheckPasswordAsync(user, this.Input.Password);
            if (!passwordCorrect) {
                this.ModelState.AddModelError(string.Empty, "Pøihlášení se nezdaøilo");
                return this.Page();
            }

            // Get email change token
            var token = await this._userManager.GenerateChangeEmailTokenAsync(user, this.Input.NewEmail);

            // Get email change confirmation URL
            var url = this.Url.Page("/Account/Manage/ChangeEmailConfirm",
                pageHandler: null,
                values: new {
                    newEmail = this.Input.NewEmail,
                    token = token
                },
                protocol: this.Request.Scheme);

            // Send message
            var msg = new MailMessageDto {
                Subject = "Potvrzení zmìny e-mailové adresy",
                BodyText = "Zmìnu e-mailové adresy prosím potvrïte na následující adrese:\r\n" + url
            };
            msg.To.Add(new MailAddressDto(this.Input.NewEmail));
            await this._mailerService.SendMessageAsync(msg);

            return this.RedirectToPage("ChangeEmailDone");
        }
    }

}
