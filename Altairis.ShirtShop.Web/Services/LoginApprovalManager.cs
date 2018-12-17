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

        public LoginApprovalSession GetLoginApprovalInfo(string sessionId) {
            if (sessionId == null) throw new ArgumentNullException(nameof(sessionId));
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(sessionId));

            return this.sessionStore.Find(sessionId);
        }

        public LoginApprovalSessionStatus CheckStatus(string sessionId, out string userName) {
            if (sessionId == null) throw new ArgumentNullException(nameof(sessionId));
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(sessionId));

            userName = null;

            // Get session
            var session = this.sessionStore.Find(sessionId);
            if (session == null) return LoginApprovalSessionStatus.DeclinedOrExpired;

            // If session is approved, delete it
            if (session.Approved) {
                this.sessionStore.Delete(sessionId);
                userName = session.UserName;
                return LoginApprovalSessionStatus.Approved;
            } else {
                return LoginApprovalSessionStatus.Waiting;
            }
        }

        public void ApproveLogin(string sessionId) {
            if (sessionId == null) throw new ArgumentNullException(nameof(sessionId));
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(sessionId));

            // Get session
            var session = this.sessionStore.Find(sessionId);
            if (session == null) return;

            // Get current user and ensure it's authenticated
            var uid = this.httpContextAccessor.HttpContext.User.Identity;
            if (!uid.IsAuthenticated) throw new InvalidOperationException("Only authenticated user can approve login");

            // Save session as approved by current user
            this.sessionStore.Approve(sessionId, uid.Name);
        }

        public void DeclineLogin(string sessionId) {
            if (sessionId == null) throw new ArgumentNullException(nameof(sessionId));
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(sessionId));

            this.sessionStore.Delete(sessionId);
        }

    }
}
