using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
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
    [RoutePrefix("api/towns")]
    public class TownsController : BaseController
    {
        [TokenAuthorization("user")]
        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            try
            {
                if (name != null)
                {
                    var towns = UnitOfWork.Towns.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => Factory.Create(x)).ToList();
                    if (towns.Count != 0) return Ok(towns);
                    return NotFound();
                }
                return Ok(UnitOfWork.Towns.Get().ToList().Select(x => Factory.Create(x)).ToList());
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
                Town town = UnitOfWork.Towns.Get(id);
                if (town == null) return NotFound();
                return Ok(Factory.Create(town));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("")]
        public IHttpActionResult Post([FromBody]TownModel model)
        {
            try
            {
                Town town = Factory.Create(model);
                UnitOfWork.Towns.Insert(town);
                UnitOfWork.Commit();
                return Ok(Factory.Create(town));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]TownModel model)
        {
            try
            {
                Town town = Factory.Create(model);
                UnitOfWork.Towns.Update(town, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(town));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("{id:int}")]
        public IHttpActionResult Delete([FromUri]int id)
        {
            try
            {
                UnitOfWork.Towns.Delete(id);
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
    }
}
