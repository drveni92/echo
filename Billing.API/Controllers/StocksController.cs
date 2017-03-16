using Billing.API.Helper;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [RoutePrefix("api/stocks")]
    public class StocksController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(UnitOfWork.Stocks.Get().ToList().Select(x => Factory.Create(x)).ToList());
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
    }
}
