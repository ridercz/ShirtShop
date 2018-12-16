using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altairis.ShirtShop.Web.Services {
    public interface ILoginApprovalSessionStore {

        /// <summary>Creates the specified session.</summary>
        /// <param name="session">The session.</param>
        /// <returns>ID of newly created session</returns>
        string Create(LoginApprovalSession session);

        /// <summary>Finds the specified session.</summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>The session found or <c>null</c> if no such session is found.</returns>
        LoginApprovalSession Find(string sessionId);

        /// <summary>Approves the specified session.</summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="userName">Name of the user who approved the session.</param>
        void Approve(string sessionId, string userName);

        /// <summary>Deletes the specified session.</summary>
        /// <param name="sessionId">The session identifier.</param>
        void Delete(string sessionId);

    }
}
