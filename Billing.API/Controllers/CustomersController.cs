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
    public class CustomersController : BaseController
    {
       
        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            return (name != null) ? Ok(UnitOfWork.Customers.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => Factory.Create(x)).ToList()) :
                                    Ok(UnitOfWork.Customers.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("town/{id}")]
        public IHttpActionResult GetByTown(int id)
        {
            return Ok(UnitOfWork.Customers.Get().ToList().Where(x => x.Town.Id == id).Select(x => Factory.Create(x)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            Customer customer = UnitOfWork.Customers.Get(id);
            if (customer == null) return NotFound();
            return Ok(Factory.Create(customer));
        }

        [Route("")]
        public IHttpActionResult Post([FromBody]CustomerModel model)
        {
            try
            {
                Customer customer = Factory.Create(model);
                UnitOfWork.Customers.Insert(customer);
                UnitOfWork.Commit();
                return Ok(Factory.Create(customer));
            }
            catch (Exception ex)
            {
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
                return BadRequest(ex.Message);
            }
        }

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
                return BadRequest(ex.Message);
            }
        }
    }
}
