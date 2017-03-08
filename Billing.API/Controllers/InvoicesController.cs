using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [RoutePrefix("api/invoices")]
    public class InvoicesController : BaseController
    {
        [Route("{invoiceno?}")]
        public IHttpActionResult Get(string invoiceno = null)
        {
            return (invoiceno != null) ? Ok(UnitOfWork.Invoices.Get().Where(x => x.InvoiceNo.Equals(invoiceno)).ToList().Select(x => Factory.Create(x)).ToList()) :
                                    Ok(UnitOfWork.Invoices.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }
    }
}
