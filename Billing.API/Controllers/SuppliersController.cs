
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
    [RoutePrefix("api/suppliers")]
    public class SuppliersController : ApiController
    {
        IBillingRepository<Supplier> suppliers = new BillingRepository<Supplier>(new BillingContext());
        Factory factory = new Factory();


        [Route("{name?}")]
        public IHttpActionResult Get(string name)
        {
            return (name != null) ? Ok(suppliers.Get().Where(x => x.Name.Contains(name)).ToList().Select(a => factory.Create(a)).ToList()) :
                                    Ok(suppliers.Get().ToList().Select(a => factory.Create(a)).ToList());
        }
        [Route("town/{town}")]
        public IHttpActionResult GetSuppliersByTown(string town = null)
        {
            return Ok(suppliers.Get().Where(x => x.Town.Name.Contains(town)).ToList().Select(x => factory.Create(x)).ToList());

        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Supplier supplier = suppliers.Get(id);
            if (supplier == null) return NotFound();
            return Ok(factory.Create(supplier));
        }
    }
}
