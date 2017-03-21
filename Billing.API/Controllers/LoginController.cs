using Billing.API.Helper.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using WebMatrix.WebData;

namespace Billing.API.Controllers
{
    public class LoginController : BaseController
    {
        private BillingIdentity identity = new BillingIdentity();

        [Route("api/login")]
        [HttpGet]
        public IHttpActionResult Login(string credentials)
        {
            if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            string[] user = credentials.Split(':');
            if (WebSecurity.Login(user[0], user[1]))
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(user[0]), null);
                return Ok($"Welcome {user[0]}");
            }
            else
            {
                return Ok($"{user[0]} not logged in");
            }

        }

        [Route("api/logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                WebSecurity.Logout();
                return Ok($"User {identity.CurrentUser} logged out");
            }
            else
            {
                return Ok("No user is logged in!!!");
            }

        }
    }
}
