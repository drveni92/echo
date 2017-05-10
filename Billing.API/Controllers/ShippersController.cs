using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [TokenAuthorization("user")]
    [RoutePrefix("api/shippers")]
    public class ShippersController : BaseController
    {
        [Route("{name?}")]
        public IHttpActionResult Get(string name = "", int page = 0, int showPerPage = 10, string sortType = "name", bool sortReverse = false)
        {
            try
            {

                var query = (name == null) ? UnitOfWork.Shippers.Get().ToList() : UnitOfWork.Shippers.Get().Where(x => x.Name.Contains(name)).ToList();
                var list = query.OrderBy(sortType + (sortReverse ? " descending" : ""))
                                .Skip(showPerPage * page)
                                .Take(showPerPage)
                                .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<ShipperModel>(page, query.Count, list));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
        [Route("town/{id}")]
        public IHttpActionResult GetShippersByTown(int id)
        {
            try
            {
                if (UnitOfWork.Towns.Get(id) == null) return NotFound();
                return Ok(UnitOfWork.Shippers.Get().Where(x => x.Town.Id == id).ToList().Select(x => Factory.Create(x)).ToList());
              
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
                Shipper shipper = UnitOfWork.Shippers.Get(id);
                if (shipper == null) return NotFound();
                return Ok(Factory.Create(shipper));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("")]
        public IHttpActionResult Post([FromBody]ShipperModel model)
        {
            try
            {
                Shipper shipper = Factory.Create(model);
                UnitOfWork.Shippers.Insert(shipper);
                UnitOfWork.Commit();
                return Ok(Factory.Create(shipper));
            }
            catch (DbEntityValidationException ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ErrorGeneratorMessage.Generate(ex));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]ShipperModel model)
        {
            try
            {
                Shipper shipper = Factory.Create(model);
                UnitOfWork.Shippers.Update(shipper, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(shipper));
            }
            catch (DbEntityValidationException ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ErrorGeneratorMessage.Generate(ex));
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
                if (UnitOfWork.Shippers.Get(id).Invoices.Count > 0)
                {
                    var invoices = UnitOfWork.Shippers.Get(id).Invoices;
                    foreach ( var invoice in invoices  )
                    {
                        if((int)invoice.Status < 7)
                            return BadRequest("Shipper contains not shipped invoices.");           
                    }

                }
                Shipper shipper = UnitOfWork.Shippers.Get(id);
                shipper.Town = null;
                UnitOfWork.Shippers.Delete(id);
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
