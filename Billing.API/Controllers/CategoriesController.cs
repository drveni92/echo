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
    [RoutePrefix("api/categories")]
    public class CategoriesController : BaseController
    {
        [TokenAuthorization("user")]
        [Route("{name?}")]
        public IHttpActionResult Get(string name = "", int page = 0, int showPerPage = 10, string sortType = "", bool sortReverse = false)
        {
            try
            {
                var query = (name == null) ? UnitOfWork.Categories.Get().ToList() : UnitOfWork.Categories.Get().Where(x => x.Name.Contains(name)).ToList();
                var list = query.OrderBy(sortType + (sortReverse ? " descending" : ""))
                                .Skip(showPerPage * page)
                                .Take(showPerPage)
                                .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<CategoryModel>(page, query.Count, list));
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
                Category category = UnitOfWork.Categories.Get(id);
                if (category == null) return NotFound();
                return Ok(Factory.Create(category));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }
            
        [TokenAuthorization("admin")]
        [Route("")]
        public IHttpActionResult Post([FromBody]CategoryModel model)
        {
            try
            {
                Category category = Factory.Create(model);
                UnitOfWork.Categories.Insert(category);
                UnitOfWork.Commit();
                return Ok(Factory.Create(category));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [TokenAuthorization("admin")]
        [Route("{id:int}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]CategoryModel model)
        {
            try
            {
                Category category = Factory.Create(model);
                UnitOfWork.Categories.Update(category,id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(category));
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
                if (UnitOfWork.Categories.Get(id).Products.Count > 0) return BadRequest("Category contains products.");
                UnitOfWork.Categories.Delete(id);
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
