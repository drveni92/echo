using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using WebMatrix.WebData;

namespace Billing.API.Controllers
{
    [RoutePrefix("api/agents")]
    public class AgentsController : BaseController
    {
        [TokenAuthorization("admin")]
        [Route("{name?}")]
        public IHttpActionResult Get(string name = "", int page = 0, int showPerPage = 10, string sortType = "", bool sortReverse = false)
        {
            try
            {
                var query = (name == null) ? UnitOfWork.Agents.Get().ToList() : UnitOfWork.Agents.Get().Where(x => x.Name.Contains(name)).ToList();
                var list = query.OrderBy(sortType + (sortReverse ? " descending" : ""))
                                .Skip(showPerPage * page)
                                .Take(showPerPage)
                                .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<AgentModel>(page, query.Count, list));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
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

        [TokenAuthorization("user")]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {

            try
            {
                if (Identity.HasNotAccess(id)) return Unauthorized();
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

        [TokenAuthorization("admin")]
        [Route("")]
        public IHttpActionResult Post([FromBody]AgentModel model)
        {
            try
            {
                Agent agent = Factory.Create(model);
                UnitOfWork.Agents.Insert(agent);
                UnitOfWork.Commit();
                if(!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing.Database", "Agents", "Id", "Username", autoCreateTables: true);
                WebSecurity.CreateAccount(agent.Username, "billing", false);
                Roles.AddUserToRole(agent.Username, "user");
                return Ok(Factory.Create(agent));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("user")]
        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]AgentModel model)
        {
            try
            {
                if (Identity.HasNotAccess(id)) return Unauthorized();
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

        [TokenAuthorization("user")]
        [Route("changepassword/{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]AgentPasswordModel model)
        {
            try
            {
                if (model.NewPassword != model.NewPasswordAgain) return BadRequest("New password does not match");
                if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("Billing.Database", "Agents", "Id", "Username", autoCreateTables: true);
                if (WebSecurity.ChangePassword(model.Username, model.OldPassword, model.NewPassword)) return Ok("Password changed");
                return BadRequest("Password does not match");
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
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
