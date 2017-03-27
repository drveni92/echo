using Billing.API.Helpers;
using Billing.API.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebMatrix.WebData;

namespace Billing.API.Controllers
{
    [RoutePrefix("api/agents")]
    public class AgentsController : BaseController
    {

        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            try
            {
                if (name != null)
                {
                    var agents = UnitOfWork.Agents.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => Factory.Create(x)).ToList();
                    if (agents.Count != 0) return Ok(agents);
                    return NotFound();
                }
                return Ok(UnitOfWork.Agents.Get().ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("town/{id}")]
        public IHttpActionResult GetByTown(int id)
        {
            try
            {
                if (UnitOfWork.Towns.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Agents.Get().ToList().Where(x =>
                {
                    foreach (Town town in x.Towns)
                    {
                        if (town.Id == id) return true;
                    }
                    return false;
                }).Select(x => Factory.Create(x)).ToList());
            } 
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            
            try
            {
                Agent agent = UnitOfWork.Agents.Get(id);
                if (agent == null) return NotFound();
                return Ok(Factory.Create(agent));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("")]
        public IHttpActionResult Post([FromBody]AgentModel model)
        {
            try
            {
                Agent agent = Factory.Create(model);
                UnitOfWork.Agents.Insert(agent);
                UnitOfWork.Commit();
                return Ok(Factory.Create(agent));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]AgentModel model)
        {
            try
            {
                Agent agent = Factory.Create(model);
                UnitOfWork.Agents.Update(agent, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(agent));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                UnitOfWork.Agents.Delete(id);
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("profiles")]
        [HttpGet]
        public IHttpActionResult CreateProfiles()
        {
            WebSecurity.InitializeDatabaseConnection("Billing.Database", "Agents", "Id", "Username", autoCreateTables: true);
            foreach (var agent in UnitOfWork.Agents.Get().ToList())
            {
                if (string.IsNullOrWhiteSpace(agent.Username))
                {
                    string name = agent.Name.Split(' ')[0].ToLower();
                    agent.Username = name;
                    UnitOfWork.Agents.Update(agent, agent.Id);
                    UnitOfWork.Commit();
                }
                WebSecurity.CreateAccount(agent.Username, "billing", false);
            }
            return Ok("User profiles created");
        }

    }
}
