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
    [TokenAuthorization("user,admin")]
    [RoutePrefix("api/stocks")]
    public class StocksController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get(int page = 0)
        {
            try
            {
                var query = UnitOfWork.Stocks.Get().ToList();
                var list = query.Skip(Pagination.PageSize * page)
                                .Take(Pagination.PageSize)
                                .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<StockModel>(page, query.Count, list));

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
                Stock stock = UnitOfWork.Stocks.Get(id);
                if (stock == null) return NotFound();
                return Ok(Factory.Create(stock));
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
                UnitOfWork.Stocks.Delete(id);
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
