using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Altairis.ShirtShop.Web.Services {

    public enum LoginApprovalSessionStatus {
        Waiting = 0,
        Approved = 1,
        DeclinedOrExpired = 2
    }

    public class LoginApprovalManager {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILoginApprovalSessionStore sessionStore;
        private readonly LoginApprovalManagerOptions options;

        public LoginApprovalManager(ILoginApprovalSessionStore sessionStore, IHttpContextAccessor httpContextAccessor, IOptions<LoginApprovalManagerOptions> optionsAccessor = null) {
            this.sessionStore = sessionStore;
            this.httpContextAccessor = httpContextAccessor;
            this.options = optionsAccessor?.Value ?? new LoginApprovalManagerOptions();
        }

        public string RequestLoginApproval() {
            var las = new LoginApprovalSession {
                RequesterIpAddress = this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                RequesterUserAgent = this.httpContextAccessor.HttpContext.Request.Headers?["User-Agent"],
                Expiration = DateTime.Now.Add(this.options.Expiration)
            };
            return this.sessionStore.Create(las);
        }

        public LoginApprovalSession GetLoginApprovalInfo(string lasid) {
            if (lasid == null) throw new ArgumentNullException(nameof(lasid));
            if (string.IsNullOrWhiteSpace(lasid)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(lasid));

            return this.sessionStore.Find(lasid);
        }

        public LoginApprovalSessionStatus CheckLoginApprovalStatus(string lasid, out string userName) {
            if (lasid == null) throw new ArgumentNullException(nameof(lasid));
            if (string.IsNullOrWhiteSpace(lasid)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(lasid));

            userName = null;

            // Get session
            var las = this.sessionStore.Find(lasid);
            if (las == null) return LoginApprovalSessionStatus.DeclinedOrExpired;

            // If session is approved, delete it
            if (las.Approved) {
                this.sessionStore.Delete(lasid);
                userName = las.UserName;
                return LoginApprovalSessionStatus.Approved;
            } else {
                return LoginApprovalSessionStatus.Waiting;
            }
        }

        public void ApproveLogin(string lasid) {
            if (lasid == null) throw new ArgumentNullException(nameof(lasid));
            if (string.IsNullOrWhiteSpace(lasid)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(lasid));

            // Get current user and ensure it's authenticated
            var uid = this.httpContextAccessor.HttpContext.User.Identity;
            if (!uid.IsAuthenticated) throw new InvalidOperationException("Only authenticated user can approve login");

            // Get session
            var las = this.sessionStore.Find(lasid);
            if (las == null) return;

            // Save session as approved by current user
            this.sessionStore.Approve(lasid, uid.Name);
        }

        public void DeclineLogin(string lasid) {
            if (lasid == null) throw new ArgumentNullException(nameof(lasid));
            if (string.IsNullOrWhiteSpace(lasid)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(lasid));

            this.sessionStore.Delete(lasid);
        }

    }
}
