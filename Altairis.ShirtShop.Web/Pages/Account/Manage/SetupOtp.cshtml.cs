using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Altairis.ShirtShop.Web.Pages.Account.Manage {
    public class SetupOtpModel : PageModel {
        private readonly UserManager<ShopUser> _userManager;

        public SetupOtpModel(UserManager<ShopUser> userManager) {
            this._userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel {
            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            public string OtpCode { get; set; }
        }

        public enum OtpStatus {
            Disabled,
            Enabled,
            Setup
        }

        public OtpStatus CurrentOtpStatus { get; set; }

        public string FormattedSecret { get; set; }

        public string OtpUri { get; set; }

        public IEnumerable<string> RecoveryCodes { get; set; }

        private async Task LoadData(bool reset) {
            var user = await this._userManager.GetUserAsync(this.User);
            var isOtpEnabled = await this._userManager.GetTwoFactorEnabledAsync(user);

            this.CurrentOtpStatus = isOtpEnabled ? OtpStatus.Enabled : OtpStatus.Disabled;

            if (!isOtpEnabled && reset) {
                var result = await this._userManager.ResetAuthenticatorKeyAsync(user);
                if (!result.Succeeded) throw new Exception("Unable to reset OTP secret.");
            }
            var secret = await this._userManager.GetAuthenticatorKeyAsync(user);

            this.FormattedSecret = OtpHelper.FormatSecret(secret);
            this.OtpUri = OtpHelper.GenerateUri("ShirtShop", user.UserName, secret);
        }

        public async Task OnGetAsync() {
            await LoadData(reset: true);
        }

        public async Task<IActionResult> OnPostAsync() {
            await LoadData(reset: false);

            // Validate model
            if (!this.ModelState.IsValid) return this.Page();

            // Validate password
            var user = await this._userManager.GetUserAsync(this.User);
            var passwordValid = await this._userManager.CheckPasswordAsync(user, this.Input.Password);
            if (!passwordValid) {
                this.ModelState.AddModelError(nameof(Input.Password), "Bylo zadáno chybné heslo");
                return this.Page();
            }

            if (this.CurrentOtpStatus == OtpStatus.Enabled) {
                // Disable 2FA
                var disableResult = await this._userManager.SetTwoFactorEnabledAsync(user, enabled: false);
                if (!disableResult.Succeeded) throw new Exception("Unable to disable 2FA for user.");
                return this.RedirectToPage("SetupOtpDisabled");
            }

            // Validate generated OTP
            var otpCode = Regex.Replace(this.Input.OtpCode, @"[^\d]", "");
            var otpValid = await this._userManager.VerifyTwoFactorTokenAsync(
                user,
                _userManager.Options.Tokens.AuthenticatorTokenProvider,
                otpCode);
            if (!otpValid) {
                this.ModelState.AddModelError(nameof(Input.OtpCode), "Byl zadán chybný autentizační kód");
                return this.Page();
            }

            // Enable 2FA for user
            var enableResult = await this._userManager.SetTwoFactorEnabledAsync(user, enabled: true);
            if (!enableResult.Succeeded) throw new Exception("Unable to enable 2FA for user.");

            // Generate and display account recovery codes
            this.RecoveryCodes = await this._userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            this.CurrentOtpStatus = OtpStatus.Setup;
            return this.Page();
        }

    }
}