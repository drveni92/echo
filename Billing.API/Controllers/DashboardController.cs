using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using System;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [TokenAuthorization("user")]
    public class DashboardController : BaseController
    {
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(Reports.Dashboard.Report());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
    }
}
