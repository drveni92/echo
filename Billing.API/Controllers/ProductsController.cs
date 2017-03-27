
using Billing.Api.Models;
using Billing.API.Controllers;
using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.Database;
using Billing.Repository;
using System;
using System.Linq;
using System.Web.Http;

namespace Billing.Api.Controllers
{
    [TokenAuthorization("user")]
    [RoutePrefix("api/products")]
    public class ProductsController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(UnitOfWork.Products.Get().ToList().Select(x => Factory.Create(x)).ToList());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("{name}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                var products = UnitOfWork.Products.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => Factory.Create(x)).ToList();
                if (products.Count != 0) return Ok(products);
                return NotFound();
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
                if (stock != null && stock.Invertory != 0) return BadRequest("You can not delete product that contains stock");
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