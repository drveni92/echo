using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
     [RoutePrefix("api/procurements")]
      public class ProcurementsController : BaseController
        {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Procurement.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("{id:int}")]
            public IHttpActionResult GetById(int id)
            {
                Procurement procurement = UnitOfWork.Procurements.Get(id);
                if (procurement == null) return NotFound();
                return Ok(Factory.Create(procurement));
            }
        }
}
