using Billing.API.Helpers;
using Billing.API.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    public class SalesByCustomerController : BaseController
    {
        public IHttpActionResult Post([FromBody]ReportRequestModel request)
        {
            try
            {
                return Ok(Reports.SalesByCategory.Report(request.StartDate, request.EndDate));
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
