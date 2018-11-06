using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Altairis.Services.Mailing;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ShirtShop.Web.Pages.Account {
    public class RegisterModel : PageModel {
        private readonly UserManager<ShopUser> _userManager;
        private readonly IMailerService _mailerService;

        public RegisterModel(UserManager<ShopUser> userManager, IMailerService mailerService) {
            this._userManager = userManager;
            this._mailerService = mailerService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel {

            [Required, MaxLength(50)]
            public string FullName { get; set; }

            [Required]
            public string UserName { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            [Required, DataType(DataType.Password), Compare("Password")]
            public string ConfirmPassword { get; set; }

        }

        public async Task<ActionResult> OnPostAsync() {
            // Validate form
            if (!this.ModelState.IsValid) return this.Page();

            // Try to create user
            var newUser = new ShopUser {
                UserName = this.Input.UserName,
                Email = this.Input.Email,
                FullName = this.Input.FullName
            };
            var result = await this._userManager.CreateAsync(newUser, this.Input.Password);
            if (!result.Succeeded) {
                foreach (var error in result.Errors) {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
                return this.Page();
            }

            // Get email confirmation token
            var token = await this._userManager.GenerateEmailConfirmationTokenAsync(newUser);

            // Get email confirmation URL
            var url = this.Url.Page("/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = newUser.Id, token = token },
                protocol: this.Request.Scheme);

            // Send email confirmation message
            var msg = new MailMessageDto {
                Subject = "Potvrzení e-mailové adresy",
                BodyText = "Pro registraci potvrďte svůj e-mail na následující adrese:\r\n" + url
            };
            msg.To.Add(new MailAddressDto(newUser.Email));
            await this._mailerService.SendMessageAsync(msg);

            return this.RedirectToPage("RegisterDone");
        }

    }
}