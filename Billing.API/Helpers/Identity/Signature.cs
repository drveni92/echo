using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Billing.API.Helpers.Identity
{
    public static class Signature
    {
        public static string Generate(string Secret, string AppId)
        {
            byte[] secret = Convert.FromBase64String(Secret);
            byte[] appId = Convert.FromBase64String(AppId);

            var provider = new System.Security.Cryptography.HMACSHA256(secret);
            string key = System.Text.Encoding.Default.GetString(appId);
            var hash = provider.ComputeHash(Encoding.UTF8.GetBytes(key));

            return Convert.ToBase64String(hash);
        }
    }
}