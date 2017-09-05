using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Altairis.ShirtShop.Web {
    public class ShopOptions {

        public MailingOptions Mailing { get; set; }

        public string OrderEmailAddress { get; set; }

        public string DbFileName { get; set; }

        public class MailingOptions {
            public string SenderName { get; set; }

            public string SenderEmail { get; set; }

            public string PickupFolder { get; set; }
        }

    }
}
