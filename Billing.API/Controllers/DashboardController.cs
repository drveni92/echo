using Billing.API.Helper;
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
    public class DashboardController : BaseController
    {
        public IHttpActionResult Get()
        {
            try
            {
                DashboardModel result = new DashboardModel(5);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
    }
}
