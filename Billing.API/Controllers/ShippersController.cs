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
    public class ShippersController : BaseController
    {
        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            return (name != null) ? Ok(UnitOfWork.Shippers.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => Factory.Create(x)).ToList()) :
                                    Ok(UnitOfWork.Shippers.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("town/{town}")]
        public IHttpActionResult GetShippersByTown(string town)
        {
            return Ok(UnitOfWork.Shippers.Get().Where(x => x.Town.Name.Contains(town)).ToList().Select(x => Factory.Create(x)).ToList());

        }

        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            Shipper shipper = UnitOfWork.Shippers.Get(id);
            if (shipper == null) return NotFound();
            return Ok(Factory.Create(shipper));
        }
    }
}
