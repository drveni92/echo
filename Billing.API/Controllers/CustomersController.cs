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
    [TokenAuthorization("user")]
    [RoutePrefix("api/customers")]
    public class CustomersController : BaseController
    {
       
        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            try
            {
                if (name != null)
                {
                    var customers = UnitOfWork.Customers.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => Factory.Create(x)).ToList();
                    if (customers.Count != 0) return Ok(customers);
                    return NotFound();
                }
                return Ok(UnitOfWork.Customers.Get().ToList().Select(x => Factory.Create(x)).ToList());
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
                return Ok(UnitOfWork.Customers.Get().ToList().Where(x => x.Town.Id == id).Select(x => Factory.Create(x)).ToList());
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
                Customer customer = UnitOfWork.Customers.Get(id);
                if (customer == null) return NotFound();
                return Ok(Factory.Create(customer));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

   
        [Route("")]
        public IHttpActionResult Post([FromBody]CustomerModel model)
        {
            try
            {
                if (UnitOfWork.Towns.Get(model.Town.Id) == null) return BadRequest("Town not found");
                Customer customer = Factory.Create(model);
                UnitOfWork.Customers.Insert(customer);
                UnitOfWork.Commit();
                return Ok(Factory.Create(customer));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]CustomerModel model)
        {
            try
            {
                Customer customer = Factory.Create(model);
                UnitOfWork.Customers.Update(customer, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(customer));
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
                UnitOfWork.Customers.Delete(id);
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
