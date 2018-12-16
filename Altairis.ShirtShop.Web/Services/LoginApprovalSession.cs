using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altairis.ShirtShop.Web.Services {

    public class LoginApprovalSession {

        public string SessionId { get; set; }

        public DateTime Expiration { get; set; }

        public string RequesterUserAgent { get; set; }

        public string RequesterIpAddress { get; set; }

        public bool Approved { get; set; }

        public string UserName { get; set; }

    }
}
