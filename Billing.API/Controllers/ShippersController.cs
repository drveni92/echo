using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
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
    [TokenAuthorization("user")]
    [RoutePrefix("api/shippers")]
    public class ShippersController : BaseController
    {
        [Route("{name?}")]
        public IHttpActionResult Get(string name = null, int page = 0)
        {
            try
            {
                List<Shipper> shippers;
                if (name != null)
                {
                    shippers = UnitOfWork.Shippers.Get().Where(x => x.Name.Contains(name)).ToList();
                    if (shippers.Count == 0) return NotFound();
                }
                else shippers = UnitOfWork.Shippers.Get().ToList();
                var list = shippers.Skip(Pagination.PageSize * page)
                                .Take(Pagination.PageSize)
                                .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<ShipperModel>(page, shippers.Count, list));
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
