using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.ShirtShop.Web {
    public static class OtpHelper {
        private const string URI_FORMAT = "otpauth://{0}/{1}?secret={2}&issuer={3}&digits={4}&period={5}";

        public static string GenerateUri(string issuer, string user, string secret,
                                            int digits = 6,
                                            int period = 30,
                                            string otpType = "totp") {

            return string.Format(URI_FORMAT,
                otpType,                                    // 0
                WebUtility.UrlEncode(issuer + ":" + user),  // 1
                secret,                                     // 2
                WebUtility.UrlEncode(issuer),               // 3
                digits,                                     // 4
                period);                                    // 5
        }

        public static string FormatSecret(string secret) {
            var result = new StringBuilder();

            var currentPosition = 0;
            while (currentPosition + 4 < secret.Length) {
                result.Append(secret.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < secret.Length) result.Append(secret.Substring(currentPosition));

            return result.ToString().ToLowerInvariant();
        }

    }
}
