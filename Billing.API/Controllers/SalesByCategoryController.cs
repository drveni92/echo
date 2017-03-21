using Billing.API.Helper;
using Billing.API.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    public class SalesByCategoryController : BaseController
    {
        public IHttpActionResult Post([FromBody]RequestModel request)
        {
            try
            {
                return Ok(FactoryReport.ReportCategory(request.StartDate, request.EndDate));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
