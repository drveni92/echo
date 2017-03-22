using Billing.Repository;
using System;
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
                string ApiKey = actionContext.Request.Headers.GetValues("ApiKey").ToString();
                string Token = actionContext.Request.Headers.GetValues("Token").ToString();

                UnitOfWork unitOfWork = new UnitOfWork();
                var token = unitOfWork.Tokens.Get().FirstOrDefault(x => x.Token == Token);

                if (token != null)
                {
                    if (token.ApiUser.AppId == ApiKey && token.Expiration > DateTime.UtcNow) return;
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