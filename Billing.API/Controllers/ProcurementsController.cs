using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Models;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [TokenAuthorization("user")]
    [RoutePrefix("api/procurements")]
    public class ProcurementsController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get(int page = 0)
        {
            try
            {
                var query = UnitOfWork.Procurements.Get().ToList();
                var list = query.Skip(Pagination.PageSize * page)
                                .Take(Pagination.PageSize)
                                .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<ProcurementModel>(page, query.Count, list));
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
                Procurement procurement = UnitOfWork.Procurements.Get(id);
                if (procurement == null) return NotFound();
                return Ok(Factory.Create(procurement));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("document/{document}")]
        public IHttpActionResult GetByDocument(string document)
        {
            try
            {
                var procurements = UnitOfWork.Procurements.Get().Where(x => x.Document.Equals(document)).ToList().Select(x => Factory.Create(x)).ToList();
                if (procurements.Count != 0) return Ok(procurements);
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("product/{id}")]
        public IHttpActionResult GetByProductId(int id)
        {
            try
            {
                if (UnitOfWork.Products.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Procurements.Get().Where(x => x.Product.Id == id).ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("")]
        public IHttpActionResult Post([FromBody]ProcurementModel model)
        {
            try
            {
                Procurement procurement = Factory.Create(model);
                UnitOfWork.Procurements.Insert(procurement);
                UnitOfWork.Commit();
                return Ok(Factory.Create(procurement));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("{id:int}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]ProcurementModel model)
        {
            try
            {
                Procurement procurement = Factory.Create(model);
                UnitOfWork.Procurements.Update(procurement, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(procurement));
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
                UnitOfWork.Procurements.Delete(id);
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
