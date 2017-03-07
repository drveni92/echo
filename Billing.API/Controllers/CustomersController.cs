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
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        private IBillingRepository<Customer> customers = new BillingRepository<Customer>(new BillingContext());
        private Factory factory = new Factory();

        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            return (name != null) ? Ok(customers.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => factory.Create(x)).ToList()) :
                                    Ok(customers.Get().ToList().Select(x => factory.Create(x)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            Customer customer = customers.Get(id);
            if (customer == null) return NotFound();
            return Ok(factory.Create(customer));
        }
    }
}
