using System;
using System.Web.Http;

namespace Billing.API.Controllers
{
    public class SalesByRegionController : BaseController
    {
        public struct Params
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int AgentId { get; set; }
        }

        public IHttpActionResult Post([FromBody]Params p)
        {
            return Ok(FactoryReport.Report(p.StartDate, p.EndDate, p.AgentId));
        }
    }
}
