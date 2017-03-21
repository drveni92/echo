using Billing.API.Models.Reports;
using System;
using System.Web.Http;

namespace Billing.API.Controllers
{
    public class SalesByRegionController : BaseController
    {
        public IHttpActionResult Post([FromBody]RequestModel request)
        {
            return Ok(FactoryReport.ReportRegion(request.StartDate, request.EndDate));
        }
    }
}
