﻿using Billing.Repository;
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
using System.Web.Security;
using WebMatrix.WebData;

namespace Billing.API.Helpers.Identity
{
    public class TokenAuthorizationAttribute : AuthorizationFilterAttribute
    {
        private BillingIdentity Identity = new BillingIdentity();
        private string[] _role;

        public TokenAuthorizationAttribute(string role)
        {
            _role = role.Split(',');
        }

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

                    if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing.Database", "Agents", "Id", "Username", autoCreateTables: true);

                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(authToken.Agent.Username), Roles.GetRolesForUser(authToken.Agent.Username));

                    if (authToken != null)
                        if (authToken.ApiUser.AppId == ApiKey.First() && authToken.Expiration > DateTime.UtcNow)
                            foreach (string role in _role)
                                if (Identity.CurrentUser.Roles.Contains(role)) return;
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