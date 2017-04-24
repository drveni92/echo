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
    public class SalesByCustomerController : BaseController
    {
        public IHttpActionResult Post([FromBody]ReportRequestModel request)
        {
            try
            {
                if (request.Page < 0) request.Page = 0;
                var data = Reports.SalesByCustomer.Report(request.StartDate, request.EndDate);
                var customersCount = data.Customers.Count;
                data.Customers = data.Customers.Skip(Pagination.PageSize * request.Page).Take(Pagination.PageSize).ToList();
                return Ok(Factory.Create<SalesByCustomerModel>(request.Page, customersCount, data));
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
