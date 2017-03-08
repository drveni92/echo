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
    public class AgentsController : BaseController
    {
     
        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            return (name != null) ? Ok(UnitOfWork.Agents.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => Factory.Create(x)).ToList()) :
                                    Ok(UnitOfWork.Agents.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {   
            Agent agent = UnitOfWork.Agents.Get(id);
            if (agent == null) return NotFound();
            return Ok(Factory.Create(agent));
        }
    }
}
