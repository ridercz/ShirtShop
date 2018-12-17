using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altairis.ShirtShop.Web.Services {
    public class InMemoryLoginApprovalSessionStore : LoginApprovalSessionStoreBase {
        private readonly object saveLock = new object();
        private readonly List<LoginApprovalSession> store = new List<LoginApprovalSession>();

        protected override void Save(LoginApprovalSession las) {
            if (las == null) throw new ArgumentNullException(nameof(las));

            lock (this.saveLock) {
                if (this.Find(las.SessionId) != null) throw new ArgumentException("Duplicate session ID");
                this.store.Add(las);
            }
        }

        public override LoginApprovalSession Find(string lasid) {
            if (lasid == null) throw new ArgumentNullException(nameof(lasid));
            if (string.IsNullOrWhiteSpace(lasid)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(lasid));

            // Delete expired sessions
            this.store.RemoveAll(x => x.Expiration <= DateTime.Now);

            // Find session
            return this.store.Find(x => x.SessionId.Equals(lasid, StringComparison.OrdinalIgnoreCase));
        }

        public override void Delete(string lasid) {
            if (lasid == null) throw new ArgumentNullException(nameof(lasid));
            if (string.IsNullOrWhiteSpace(lasid)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(lasid));

            this.store.RemoveAll(x => x.SessionId.Equals(lasid, StringComparison.OrdinalIgnoreCase));
        }

        public override void Approve(string lasid, string userName) {
            if (lasid == null) throw new ArgumentNullException(nameof(lasid));
            if (string.IsNullOrWhiteSpace(lasid)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(lasid));
            if (userName == null) throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(userName));

            var las = this.store.Find(x => x.SessionId.Equals(lasid, StringComparison.OrdinalIgnoreCase));
            if (las != null) {
                las.Approved = true;
                las.UserName = userName;
            }
        }
    }
}

