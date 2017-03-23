using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    public class SalesByProductController : BaseController
    {
        public struct Params
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int CategoryId { get; set; }
        }

        public IHttpActionResult Post([FromBody]Params p)
        {
            return Ok(FactoryReport.ReportProduct(p.StartDate, p.EndDate, p.CategoryId));
        }
    }
}
