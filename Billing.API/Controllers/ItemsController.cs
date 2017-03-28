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
    [RoutePrefix("api/items")]
    public class ItemsController : BaseController
    {
        [TokenAuthorization("user")]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(UnitOfWork.Items.Get().ToList().Select(x => Factory.Create(x)).ToList());
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
                Item item = UnitOfWork.Items.Get(id);
                if (item == null) return NotFound();
                return Ok(Factory.Create(item));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("user")]
        [Route("invoice/{id}")]
        public IHttpActionResult GetItemsByInvoice(int id)
        {
            try
            {
                if (UnitOfWork.Invoices.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Items.Get().Where(x => x.Invoice.Id == id).ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("user")]
        [Route("product/{id}")]
        public IHttpActionResult GetItemByProduct(int id)
        {
            try
            {
                if (UnitOfWork.Products.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Items.Get().Where(x => x.Product.Id == id).ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("user,admin")]
        [Route("")]
        public IHttpActionResult Post([FromBody]ItemModel model)
        {
            try
            {
                int agentId = UnitOfWork.Invoices.Get(model.Invoice.Id).Agent.Id;
                if (Identity.HasAccess(agentId)) return Unauthorized();
                Item item = Factory.Create(model);
                UnitOfWork.Items.Insert(item);
                UnitOfWork.Commit();
                return Ok(Factory.Create(item));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("user,admin")]
        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]ItemModel model)
        {
            try
            {
                int agentId = UnitOfWork.Invoices.Get(model.Invoice.Id).Agent.Id;
                if (Identity.HasAccess(agentId)) return Unauthorized();
                Item item = Factory.Create(model);
                UnitOfWork.Items.Update(item, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(item));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("user,admin")]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                int agentId = UnitOfWork.Items.Get().ToList().FirstOrDefault().Invoice.Agent.Id;
                if (Identity.HasAccess(agentId)) return Unauthorized();
                UnitOfWork.Items.Delete(id);
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
