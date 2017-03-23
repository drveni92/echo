using Billing.API.Helpers;
using Billing.API.Models.Reports;
using System;
using System.Web.Http;

namespace Billing.API.Controllers
{
    public class SalesByRegionController : BaseController
    {
        public IHttpActionResult Post([FromBody]ReportRequestModel request)
        {
            try
            {
                return Ok(Reports.SalesByRegion.Report(request.StartDate, request.EndDate));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
