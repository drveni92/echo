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
    [RoutePrefix("api/agents")]
    public class AgentsController : ApiController
    {
        private IBillingRepository<Agent> agents = new BillingRepository<Agent>(new BillingContext());
        private Factory factory = new Factory();

        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            return (name != null) ? Ok(agents.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => factory.Create(x)).ToList()) :
                                    Ok(agents.Get().ToList().Select(x => factory.Create(x)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {   
            Agent agent = agents.Get(id);
            if (agent == null) return NotFound();
            return Ok(factory.Create(agent));
        }
    }
}
