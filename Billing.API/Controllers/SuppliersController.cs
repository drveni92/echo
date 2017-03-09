
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
    public class SuppliersController : BaseController
    {
        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            return (name != null) ? Ok(UnitOfWork.Suppliers.Get().Where(x => x.Name.Contains(name)).ToList().Select(a => Factory.Create(a)).ToList()) :
                                    Ok(UnitOfWork.Suppliers.Get().ToList().Select(a => Factory.Create(a)).ToList());
        }
        [Route("town/{town}")]
        public IHttpActionResult GetSuppliersByTown(string town)
        {
            return Ok(UnitOfWork.Suppliers.Get().Where(x => x.Town.Name.Contains(town)).ToList().Select(x => Factory.Create(x)).ToList());

        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Supplier supplier = UnitOfWork.Suppliers.Get(id);
            if (supplier == null) return NotFound();
            return Ok(Factory.Create(supplier));
        }

        [Route("")]
        public IHttpActionResult Post([FromBody]SupplierModel model)
        {
            try
            {
                Supplier supplier = Factory.Create(model);
                UnitOfWork.Suppliers.Insert(supplier);
                UnitOfWork.Commit();
                return Ok(Factory.Create(supplier));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]SupplierModel model)
        {
            try
            {
                Supplier supplier = Factory.Create(model);
                UnitOfWork.Suppliers.Update(supplier, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(supplier));
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
                UnitOfWork.Suppliers.Delete(id);
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
