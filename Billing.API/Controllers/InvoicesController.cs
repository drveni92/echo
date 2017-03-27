using Billing.API.Helpers;
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
    [RoutePrefix("api/invoices")]   
    public class InvoicesController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(UnitOfWork.Invoices.Get().ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("invoiceno/{invoiceno}")]
        public IHttpActionResult GetByInvoiceNo(string invoiceno)
        {
            try
            {
                var invoices = UnitOfWork.Invoices.Get().Where(x => x.InvoiceNo.Equals(invoiceno)).ToList().Select(x => Factory.Create(x)).ToList();
                if (invoices.Count != 0) return Ok(invoices);
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("customer/{id}")]
        public IHttpActionResult GetByInvoicesByCustomerId(int id)
        {
            try
            {
                if (UnitOfWork.Customers.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Invoices.Get().Where(x => x.Customer.Id == id).ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("agent/{id}")]
        public IHttpActionResult GetByInvoicesByAgentId(int id)
        {
            try
            {
                var agents = UnitOfWork.Invoices.Get().Where(x => x.Agent.Id == id).ToList().Select(x => Factory.Create(x)).ToList();
                if (agents.Count != 0) return Ok(agents);
                return NotFound();
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
                Invoice invoice = UnitOfWork.Invoices.Get(id);
                if (invoice == null) return NotFound();
                return Ok(Factory.Create(invoice));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("")]
        public IHttpActionResult Post([FromBody]InvoiceModel model)
        {
            try
            {
                Invoice invoice = Factory.Create(model);
                UnitOfWork.Invoices.Insert(invoice);
                UnitOfWork.Commit();
                return Ok(Factory.Create(invoice));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]InvoiceModel model)
        {
            try
            {
                Invoice invoice = Factory.Create(model);
                UnitOfWork.Invoices.Update(invoice, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(invoice));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var invoice = UnitOfWork.Invoices.Get(id);
                if (invoice.Items.Count != 0) return BadRequest($"Invoice {invoice.InvoiceNo} has items.");
                UnitOfWork.Invoices.Delete(id);
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("history/{id}")]
        public IHttpActionResult GetHistoryInvoicesById(int id)
        {
            try
            {
                if (UnitOfWork.Histories.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Histories.Get().Where(x => x.Id == id).ToList().Select(x => Factory.Create(x)).ToList());

            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{id}/next/{cancel}")]
        public IHttpActionResult GetNext(int id, bool cancel = false)
        {
            try
            {
                InvoiceHelper helper = new InvoiceHelper();
                Invoice entity = helper.NextStep(UnitOfWork, id, cancel);
                return Ok(Factory.Create(entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
