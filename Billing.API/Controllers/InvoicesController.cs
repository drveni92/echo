using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Helpers.PDFGenerator;
using Billing.API.Models;
using Billing.Database;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Billing.API.Controllers
{

    [RoutePrefix("api/invoices")]   
    public class InvoicesController : BaseController
    {
        [TokenAuthorization("user")]
        [Route("{invoiceno?}")]
        public IHttpActionResult Get(string invoiceno = "", int page = 0)
        {
            try
            {
                var query = (invoiceno == null) ? UnitOfWork.Invoices.Get().ToList() : UnitOfWork.Invoices.Get().Where(x => x.InvoiceNo.Contains(invoiceno)).ToList();
                var list = query.Skip(Pagination.PageSize * page)
                                .Take(Pagination.PageSize)
                                .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<InvoiceModel>(page, query.Count, list));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("user")]
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

        [TokenAuthorization("user")]
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

        [TokenAuthorization("user")]
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

        [TokenAuthorization("user")]
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

        [TokenAuthorization("user")]
        [HttpGet]
        [Route("automatic/{id}")]
        public IHttpActionResult AutomaticallyChangeState(int id)
        {
            try
            {
                if (UnitOfWork.AutomaticStates.Get().Where(x => x.Invoice.Id == id).ToList().Count != 0) return Ok("Invoice is already in list");
                UnitOfWork.AutomaticStates.Insert(Factory.Create(id));
                UnitOfWork.Commit();
                return Ok("Invoice added to list");
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("user,admin")]
        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]InvoiceModel model)
        {
            try
            {
                if (Identity.HasNotAccess(model.Agent.Id)) return Unauthorized();
                /* Check if user owns invoice */
                if (UnitOfWork.Invoices.Get(model.Id).Agent.Id != Identity.CurrentUser.Id && !Identity.HasRole("admin")) return Unauthorized();
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

        [TokenAuthorization("user,admin")]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var invoice = UnitOfWork.Invoices.Get(id);
                if (Identity.HasNotAccess(invoice.Agent.Id)) return Unauthorized();
                // If admin delete invoice
                if (Identity.HasRole("admin"))
                {
                    if (invoice.Items.Count != 0)
                    {
                        // Delete all items
                        foreach (var item in invoice.Items.ToList())
                        {
                            UnitOfWork.Items.Delete(item.Id);
                        }
                    }
                    if (invoice.History.Count != 0)
                    {
                        // Delete all history
                        foreach (var history in invoice.History.ToList())
                        {
                            UnitOfWork.Histories.Delete(history.Id);
                        }
                    }
                    UnitOfWork.Invoices.Delete(id);
                }
                // User can only change state of invoice
                else
                {
                    invoice.Status = Status.Canceled;
                    UnitOfWork.Invoices.Update(invoice, invoice.Id);
                }
                UnitOfWork.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("user")]
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

        [TokenAuthorization("user,admin")]
        [Route("{id}/next/{cancel}")]
        public IHttpActionResult GetNext(int id, bool cancel = false)
        {
            try
            {
                int agentId = UnitOfWork.Invoices.Get(id).Agent.Id;
                if (Identity.HasNotAccess(agentId)) return Unauthorized();
                InvoiceHelper helper = new InvoiceHelper();
                Invoice entity = helper.NextStep(UnitOfWork, id, cancel);
                return Ok(Factory.Create(entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("user")]
        [HttpPost]
        [Route("mail")]
        public IHttpActionResult SendMail([FromBody]MailRequest model)
        {
            Invoice invoice = UnitOfWork.Invoices.Get(model.InvoiceId);
            Helper.SendEmail(invoice, Identity.CurrentUser.Username, model.MailTo);
            return Ok("Email sent");
        }

        [TokenAuthorization("user")]
        [Route("download/{id}")]
        public IHttpActionResult GetDownload(int id)
        {
            /* Get Invoice PDF */
            Invoice invoice = UnitOfWork.Invoices.Get(id);
            PDFInvoice pdf = new PDFInvoice(invoice);
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);
            pdfRenderer.Document = pdf.CreateDocument();
            pdfRenderer.RenderDocument();
            MemoryStream stream = new MemoryStream();
            pdfRenderer.Save(stream, false);

            /* Send PDF file */
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            var response = ResponseMessage(result);

            return response;
        }
    }
}
