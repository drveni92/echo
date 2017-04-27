using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Models;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Linq.Dynamic;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [TokenAuthorization("user")]
    [RoutePrefix("api/procurements")]
    public class ProcurementsController : BaseController
    {
        [Route("{product?}")]
        public IHttpActionResult Get(string product = "",int page = 0, int showPerPage = 10, string sortType = "", bool sortReverse = false)
        {
            try
            {
                var query = (product == null) ? UnitOfWork.Procurements.Get().ToList() : UnitOfWork.Procurements.Get().Where(x => x.Product.Name.Contains(product)).ToList();
                var list = query.OrderBy(sortType + (sortReverse ? " descending" : ""))
                                .Skip(showPerPage * page)
                                .Take(showPerPage)
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
                Update(procurement.Product.Id);
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
                Update(procurement.Product.Id);
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


        private List<string> Update(int productId)
        {
            InvoiceHelper states = new InvoiceHelper();
            List<string> invoices = new List<string>();
            var items = UnitOfWork.AutomaticStates.Get().Where(x => (x.Completed == false)).ToList()
                                                  .Where(x => x.Invoice.Items.Where(y => productId == y.Product.Id).ToList().Count > 0)
                                                  .ToList();
            foreach (var item in items)
            {
                try
                {
                    while(item.Invoice.Status != Status.InvoiceShipped)
                        states.NextStep(UnitOfWork, item.Invoice.Id, false);
                }
                catch(Exception ex)
                {
                    Logger.Log(ex.Message, "INFO");
                }
            }
            return invoices;
        }

    }
}
