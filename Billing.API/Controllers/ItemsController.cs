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
    [RoutePrefix("api/items")]
    public class ItemsController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(UnitOfWork.Items.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }

        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            try
            {
                Item item = UnitOfWork.Items.Get(id);
                if (item == null) return NotFound();
                return Ok(Factory.Create(item));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("")]
        public IHttpActionResult Post([FromBody]ItemModel model)
        {
            try
            {
                Item item = Factory.Create(model);
                UnitOfWork.Items.Insert(item);
                UnitOfWork.Commit();
                return Ok(Factory.Create(item));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]ItemModel model)
        {
            try
            {
                Item item = Factory.Create(model);
                UnitOfWork.Items.Update(item, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(item));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                UnitOfWork.Items.Delete(id);
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("invoice/{id}")]
        public IHttpActionResult GetItemsByInvoice(int id)
        {
            var items = UnitOfWork.Items.Get().Where(x => x.Invoice.Id == id).ToList().Select(x => Factory.Create(x)).ToList();
            if (items.Count != 0) return Ok(items);
            return NotFound();
        }

        [Route("product/{id}")]
        public IHttpActionResult GetItemByProduct(int id)
        {
            var items = UnitOfWork.Items.Get().Where(x => x.Product.Id == id).ToList().Select(x => Factory.Create(x)).ToList();
            if (items.Count != 0) return Ok(items);
            return NotFound();
        }
    }
}
