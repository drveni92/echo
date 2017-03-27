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
    [TokenAuthorization("user")]
    [RoutePrefix("api/invoicereview")]
    public class InvoiceReviewPostController : BaseController
    {
        public IHttpActionResult Post([FromBody]ReportRequestModel request)
        {
            try
            {
                return Ok(Reports.InvoicesReview.Report(request.Id, request.StartDate, request.EndDate));
            }

            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                return Ok(Reports.InvoicesReview.Report(id));
            }

            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
