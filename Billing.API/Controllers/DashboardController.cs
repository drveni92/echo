using Billing.API.Helper;
using Billing.API.Helper.Identity;
using System;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [BillingAuthorization]
    public class DashboardController : BaseController
    {
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(FactoryReport.Report());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
    }
}
