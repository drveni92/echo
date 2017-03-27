using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [TokenAuthorization("user")]
    [RoutePrefix("api/inventory")]
    public class StockLevelController : BaseController
    {
        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                return Ok(Reports.StockLevel.Report(id));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
