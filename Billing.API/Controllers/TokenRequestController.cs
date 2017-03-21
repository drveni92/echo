using Billing.API.Helper.Identity;
using Billing.API.Models;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    public class TokenRequestController : BaseController
    {
        public IHttpActionResult Post(TokenRequestModel request)
        {
            try
            {
                ApiUser apiUser = UnitOfWork.ApiUsers.Get().FirstOrDefault(x => x.AppId == request.ApiKey);
                if (apiUser == null) return NotFound();

                if (Signature.Generate(apiUser.Secret, apiUser.AppId) != request.Signature) return BadRequest("Bad application signature");

                var rawTokenInfo = apiUser.AppId + DateTime.UtcNow.ToString("s");
                //var rawTokenByte = Encoding.UTF8.GetBytes(rawTokenInfo);
                //var token = provider.ComputeHash(rawTokenByte);
                var authToken = new AuthToken()
                {
                    //Token = Convert.ToBase64String(token),
                    Token = rawTokenInfo,
                    Expiration = DateTime.Now.AddMinutes(20),
                    ApiUser = apiUser
                };
                UnitOfWork.Tokens.Insert(authToken);
                UnitOfWork.Commit();

                return Ok(Factory.Create(authToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
