
using Billing.Api.Models;
using Billing.API.Controllers;
using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Linq;
using System.Web.Http;
using System.Data.Entity.Validation;

namespace Billing.Api.Controllers
{
    [TokenAuthorization("user")]
    [RoutePrefix("api/products")]
    public class ProductsController : BaseController
    {
        [Route("{name?}")]
        public IHttpActionResult Get(string name = "", int page = 0, int showPerPage = 10, string sortType = "name", bool sortReverse = false)
        {
            try
            {
                var query = (name == null) ? UnitOfWork.Products.Get().ToList() : UnitOfWork.Products.Get().Where(x => x.Name.Contains(name)).ToList();
                var list = query.OrderBy(sortType + (sortReverse ? " descending" : ""))
                                .Skip(showPerPage * page)
                                .Take(showPerPage)
                                .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<ProductModel>(page, query.Count, list));
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
        public IHttpActionResult Put(int id, ProductModel model)
        {
            try
            {
                Product product = Factory.Create(model);
                UnitOfWork.Products.Update(product, id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(product));
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
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Product product = UnitOfWork.Products.Get(id);
                Stock stock = product.Stock;
                if (stock != null && stock.Inventory != 0) return BadRequest("You can not delete a product that is in stock");
                if (product.Items.Count != 0) return BadRequest("You can not delete a product that is in the invoices");
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