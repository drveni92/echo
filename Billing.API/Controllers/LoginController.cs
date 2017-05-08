using Billing.API.Helpers;
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
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Security;
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
            try
            {
                ApiUser apiUser = UnitOfWork.ApiUsers.Get().FirstOrDefault(x => x.AppId == request.ApiKey);
                if (apiUser == null) return NotFound();

                if (Signature.Generate(apiUser.Secret, apiUser.AppId) != request.Signature) return BadRequest("Bad application signature");

                var rawTokenInfo = apiUser.AppId + DateTime.UtcNow.ToString("s");
                var authToken = new AuthToken()
                {
                    Token = rawTokenInfo,
                    Expiration = DateTime.Now.AddMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["TokenTime"])),
                    ApiUser = apiUser,
                    Remember = (request.Remember != null) ? Factory.Create() : null,
                    Agent = UnitOfWork.Agents.Get().FirstOrDefault(x => x.Username == Thread.CurrentPrincipal.Identity.Name)
                };
                UnitOfWork.Tokens.Insert(authToken);
                UnitOfWork.Commit();

                return Ok(Factory.Create(authToken));
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/remember")]
        [HttpPost]
        public IHttpActionResult Remember(TokenRequestModel request)
        {
            AuthToken token = UnitOfWork.Tokens.Get().FirstOrDefault(x => x.Remember == request.Remember);
            if (token == null) return NotFound();

            if (token.ApiUser.AppId != request.ApiKey) return NotFound();

            ApiUser apiUser = UnitOfWork.ApiUsers.Get().FirstOrDefault(x => x.AppId == request.ApiKey);
            if (apiUser == null) return NotFound();
            if (Signature.Generate(apiUser.Secret, apiUser.AppId) != request.Signature) return BadRequest("Bad application signature");

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(token.Agent.Username), Roles.GetRolesForUser(token.Agent.Username));

            string rawTokenInfo = DateTime.Now.Ticks.ToString() + apiUser.AppId;
            byte[] rawTokenByte = Encoding.UTF8.GetBytes(rawTokenInfo);
            var authToken = new AuthToken()
            {
                Token = Convert.ToBase64String(rawTokenByte),
                Expiration = DateTime.Now.AddMinutes(20),
                Remember = Factory.Create(),
                ApiUser = apiUser,
                Agent = token.Agent
            };

            UnitOfWork.Tokens.Delete(token.Id);
            UnitOfWork.Tokens.Insert(authToken);
            UnitOfWork.Commit();
            return Ok(Factory.Create(authToken));
        }

        [TokenAuthorization("user")]
        [Route("api/logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            try
            {
                AuthToken token = UnitOfWork.Tokens.Get().FirstOrDefault(x => x.Agent.Id == Identity.CurrentUser.Id);
                if (token == null) return BadRequest("No user logged in");
                if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing", "Agents", "Id", "Username", autoCreateTables: true);
                WebSecurity.Logout();
                UnitOfWork.Tokens.Delete(token.Id);
                return Ok($"User {Identity.CurrentUser.Name} logged out");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
