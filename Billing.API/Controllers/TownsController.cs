using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
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
        public IHttpActionResult Get(string name = "", int page = 0, int showPerPage = 10, string sortType = "name", bool sortReverse = false)
        {
            try
            {

                var query = (name == null) ? UnitOfWork.Towns.Get().ToList() : UnitOfWork.Towns.Get().Where(x => x.Name.Contains(name)).ToList();
                var list = query.OrderBy(sortType + (sortReverse ? " descending" : ""))
                                .Skip(showPerPage * page)
                                .Take(showPerPage)
                                .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<TownModel>(page, query.Count, list));
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
            catch (DbEntityValidationException ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ErrorGeneratorMessage.Generate(ex));
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
            catch (DbEntityValidationException ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ErrorGeneratorMessage.Generate(ex));
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
                if (UnitOfWork.Towns.Get(id).Shippers.Count > 0) return BadRequest("Town contains Shippers.");
                if (UnitOfWork.Towns.Get(id).Customers.Count > 0) return BadRequest("Town contains customers.");
                if (UnitOfWork.Towns.Get(id).Suppliers.Count > 0) return BadRequest("Town contains suppliers.");

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
