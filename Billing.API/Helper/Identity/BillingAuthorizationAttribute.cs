using System;
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
    public class BillingAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated) return;

            var authHeader = actionContext.Request.Headers.Authorization;
            if (authHeader != null)
            {
                if (authHeader.Scheme.ToLower() == "basic" && !string.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    var rawCredentials = authHeader.Parameter;
                    var encoding = Encoding.GetEncoding("utf-8");
                    string credentials = encoding.GetString(Convert.FromBase64String(rawCredentials));

                    string[] split = credentials.Split(':');
                    string username = split[0];
                    string password = split[1];

                    if (!(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)))
                    {
                        if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing.Database", "UserProfile", "UserId", "UserName", autoCreateTables: true);

                        if (WebSecurity.Login(username, password))
                        {
                            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                            return;
                        }
                    }

                }
            }
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Billing' location=''");
            // location='http://localhost:444/accounts/login'
        }
    }
}