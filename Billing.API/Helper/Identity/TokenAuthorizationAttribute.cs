using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebMatrix.WebData;

namespace Billing.API.Helper.Identity
{
    public class TokenAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                IEnumerable<string> ApiKey = new List<string>();
                IEnumerable<string> Token = new List<string>();
                actionContext.Request.Headers.TryGetValues("ApiKey", out ApiKey);
                actionContext.Request.Headers.TryGetValues("Token", out Token);

                if (!(ApiKey == null || Token == null))
                {
                    var authToken = new UnitOfWork().Tokens.Get().FirstOrDefault(x => x.Token == Token.FirstOrDefault());
                    if (authToken != null)
                        if (authToken.ApiUser.AppId == ApiKey.First() && authToken.Expiration > DateTime.UtcNow) return;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
    }
}