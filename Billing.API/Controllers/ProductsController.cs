
using Billing.Api.Models;
using Billing.API.Controllers;
using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [TokenAuthorization("user")]
    [RoutePrefix("api/products")]
    public class ProductsController : BaseController
    {
        [Route("{name?}")]
        public IHttpActionResult Get(string name = null, int page = 0)
        {
            try
            {
                List<Product> products;
                if (name != null)
                {
                    products = UnitOfWork.Products.Get().Where(x => x.Name.Contains(name)).ToList();
                    if (products.Count == 0) return NotFound();
                }
                else products = UnitOfWork.Products.Get().ToList();
                var list = products.Skip(Pagination.PageSize * page)
                                    .Take(Pagination.PageSize)
                                    .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<ProductModel>(page, products.Count, list));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Product product = UnitOfWork.Products.Get(id);
                if (product == null) return NotFound();
                return Ok(Factory.Create(product));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("")]
        public IHttpActionResult Post(ProductModel model)
        {
            try
            {
                Product product = Factory.Create(model);
                UnitOfWork.Products.Insert(product);
                UnitOfWork.Commit();
                return Ok(Factory.Create(product));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, ProductModel model)
        {
            try
            {
                Product product = Factory.Create(model);
                UnitOfWork.Products.Update(product, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(product));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Stock stock = UnitOfWork.Products.Get(id).Stock;
                if (stock != null && stock.Inventory != 0) return BadRequest("You can not delete product that contains stock");
                UnitOfWork.Products.Delete(id);
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