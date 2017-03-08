using Billing.API.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [RoutePrefix("api/shippers")]
    public class ShippersController : ApiController
    {
        private IBillingRepository<Shipper> shippers = new BillingRepository<Shipper>(new BillingContext());
        private Factory factory = new Factory();

        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            return (name != null) ? Ok(shippers.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => factory.Create(x)).ToList()) :
                                    Ok(shippers.Get().ToList().Select(x => factory.Create(x)).ToList());
        }

        [Route("town/{town}")]
        public IHttpActionResult GetShippersByTown(string town = null)
        {
            return Ok(shippers.Get().Where(x => x.Town.Name.Contains(town)).ToList().Select(x => factory.Create(x)).ToList());

        }

        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            Shipper shipper = shippers.Get(id);
            if (shipper == null) return NotFound();
            return Ok(factory.Create(shipper));
        }
    }
}
