using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altairis.ShirtShop.Web.Services {
    public abstract class LoginApprovalSessionStoreBase : ILoginApprovalSessionStore {
        private const int SESSION_ID_LENGTH = 32; // 32 bytes = 256 bits

        /// <summary>Creates the specified session.</summary>
        /// <param name="las">The session.</param>
        /// <returns>ID of newly created session</returns>
        /// <exception cref="ArgumentNullException">session</exception>
        public virtual string Create(LoginApprovalSession las) {
            if (las == null) throw new ArgumentNullException(nameof(las));

            // Create new session ID
            var lasidRaw = new byte[SESSION_ID_LENGTH];
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(lasidRaw);
            las.SessionId = string.Join(string.Empty, lasidRaw.Select(x => x.ToString("X2")));

            // Save to store
            this.Save(las);

            // Return generated session ID
            return las.SessionId;
        }

        /// <summary>Approves the specified session.</summary>
        /// <param name="lasid">The session identifier.</param>
        /// <param name="userName">Name of the user who approved the session.</param>
        public abstract void Approve(string lasid, string userName);

        /// <summary>
        /// Deletes the specified session.
        /// </summary>
        /// <param name="lasid">The session identifier.</param>
        public abstract void Delete(string lasid);

        /// <summary>
        /// Finds the specified session.
        /// </summary>
        /// <param name="lasid">The session identifier.</param>
        /// <returns>
        /// The session found or <c>null</c> if no such session is found.
        /// </returns>
        public abstract LoginApprovalSession Find(string lasid);

        /// <summary>
        /// Saves the specified session to store.
        /// </summary>
        /// <param name="las">The session to be stored.</param>
        protected abstract void Save(LoginApprovalSession las);

    }
}
