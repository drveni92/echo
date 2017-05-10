using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Models;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [TokenAuthorization("user,admin")]
    [RoutePrefix("api/stocks")]
    public class StocksController : BaseController
    {
        [Route("{name?}")]
        public IHttpActionResult Get(string name = "", int page = 0, int showPerPage = 10, string sortType = "product.name", bool sortReverse = false)
        {
            try
            {

                var query = (name == null) ? UnitOfWork.Stocks.Get().ToList() : UnitOfWork.Stocks.Get().Where(x => x.Product.Name.Contains(name)).ToList();
                var list = query.OrderBy(sortType + (sortReverse ? " descending" : ""))
                                .Skip(showPerPage * page)
                                .Take(showPerPage)
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

    }
}
