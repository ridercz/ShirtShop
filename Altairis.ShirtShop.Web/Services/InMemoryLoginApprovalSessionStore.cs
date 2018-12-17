using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altairis.ShirtShop.Web.Services {
    public class InMemoryLoginApprovalSessionStore : LoginApprovalSessionStoreBase {
        private readonly object saveLock = new object();
        private readonly List<LoginApprovalSession> store = new List<LoginApprovalSession>();

        protected override void Save(LoginApprovalSession session) {
            if (session == null) throw new ArgumentNullException(nameof(session));

            lock (this.saveLock) {
                if (this.Find(session.SessionId) != null) throw new ArgumentException("Duplicate session ID");
                this.store.Add(session);
            }
        }

        public override LoginApprovalSession Find(string sessionId) {
            if (sessionId == null) throw new ArgumentNullException(nameof(sessionId));
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(sessionId));

            // Delete expired sessions
            this.store.RemoveAll(x => x.Expiration <= DateTime.Now);

            // Find session
            return this.store.Find(x => x.SessionId.Equals(sessionId, StringComparison.OrdinalIgnoreCase));
        }

        public override void Delete(string sessionId) {
            if (sessionId == null) throw new ArgumentNullException(nameof(sessionId));
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(sessionId));

            this.store.RemoveAll(x => x.SessionId.Equals(sessionId, StringComparison.OrdinalIgnoreCase));
        }

        public override void Approve(string sessionId, string userName) {
            if (sessionId == null) throw new ArgumentNullException(nameof(sessionId));
            if (string.IsNullOrWhiteSpace(sessionId)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(sessionId));
            if (userName == null) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(userName));

            var session = this.store.Find(x => x.SessionId.Equals(sessionId, StringComparison.OrdinalIgnoreCase));
            if (session != null) {
                session.Approved = true;
                session.UserName = userName;
            }
        }
    }
}

