
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

namespace Billing.API.Controllers
{
    [TokenAuthorization("user")]
    [RoutePrefix("api/suppliers")]
    public class SuppliersController : BaseController
    {
        [Route("{name?}")]
        public IHttpActionResult Get(string name = "", int page = 0, int showPerPage = 10, string sortType = "", bool sortReverse = false)
        {
            try
            {

                var query = (name == null) ? UnitOfWork.Suppliers.Get().ToList() : UnitOfWork.Suppliers.Get().Where(x => x.Name.Contains(name)).ToList();
                var list = query.OrderBy(sortType + (sortReverse ? " descending" : ""))
                                .Skip(showPerPage * page)
                                .Take(showPerPage)
                                .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<SupplierModel>(page, query.Count, list));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("town/{id}")]
        public IHttpActionResult GetSuppliersByTown(int id)
        {
            try
            {
                if (UnitOfWork.Towns.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Suppliers.Get().Where(x => x.Town.Id == id).ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Supplier supplier = UnitOfWork.Suppliers.Get(id);
                if (supplier == null) return NotFound();
                return Ok(Factory.Create(supplier));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
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
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
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
                UnitOfWork.Suppliers.Delete(id);
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
