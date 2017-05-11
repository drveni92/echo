using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Billing.API.Controllers
{
    public class SalesByTownController : BaseController
    {
        [TokenAuthorization("user")]
        public IHttpActionResult Post([FromBody]TownRequest town)
        {
            try
            {
                return Ok(Reports.SalesByRegion.Report(town.Name));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return BadRequest("Town not found");
            }
        }
    }
}