using Billing.API.Helper.Identity;
using Billing.API.Models;
using Billing.Database;
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
        [HttpPost]
        public IHttpActionResult Login(string credentials, [FromBody]TokenRequestModel request)
        {
            ApiUser apiUser = UnitOfWork.ApiUsers.Get().FirstOrDefault(x => x.AppId == request.ApiKey);
            if (apiUser == null) return NotFound();

            if (Signature.Generate(apiUser.Secret, apiUser.AppId) != request.Signature) return BadRequest("Bad application signature");


            if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            string[] user = credentials.Split(':');
            if (WebSecurity.Login(user[0], user[1]))
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(user[0]), null);

                var rawTokenInfo = apiUser.AppId + DateTime.UtcNow.ToString("s");
                var authToken = new AuthToken()
                {
                    Token = rawTokenInfo,
                    Expiration = DateTime.Now.AddMinutes(20),
                    ApiUser = apiUser
                };
                UnitOfWork.Tokens.Insert(authToken);
                UnitOfWork.Commit();

                return Ok(Factory.Create(authToken));
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
