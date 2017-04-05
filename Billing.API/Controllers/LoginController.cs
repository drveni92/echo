using Billing.API.Helpers.Identity;
using Billing.API.Models;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        [BillingAuthorization]
        [Route("api/login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody]TokenRequestModel request)
        {
            ApiUser apiUser = UnitOfWork.ApiUsers.Get().FirstOrDefault(x => x.AppId == request.ApiKey);
            if (apiUser == null) return NotFound();

            if (Signature.Generate(apiUser.Secret, apiUser.AppId) != request.Signature) return BadRequest("Bad application signature");

            var rawTokenInfo = apiUser.AppId + DateTime.UtcNow.ToString("s");
            var authToken = new AuthToken()
            {
                Token = rawTokenInfo,
                Expiration = DateTime.Now.AddMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TokenTime"])),
                ApiUser = apiUser
            };
            UnitOfWork.Tokens.Insert(authToken);
            UnitOfWork.Commit();

            return Ok(Factory.Create(authToken, Identity));
        }

        [Route("api/logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            try
            {
                if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing.Database", "Agents", "Id", "Username", autoCreateTables: true);
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    WebSecurity.Logout();
                    return Ok($"User {Identity.CurrentUser.Name} logged out");
                }
                else
                {
                    return Ok("No user is logged in!!!");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
