using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [TokenAuthorization("admin")]
    [RoutePrefix("api/invoicereport")]
    public class InvoiceReportController : BaseController
    {
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                return Ok(Reports.InvoiceReport.Report(id));
            }

            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
