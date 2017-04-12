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
    [RoutePrefix("api/categories")]
    public class CategoriesController : BaseController
    {
        [TokenAuthorization("user")]
        [Route("{name?}")]
        public IHttpActionResult Get(string name = null, int page = 0)
        {
            try
            {
                List<Category> categories;

                if (name != null)
                {
                    categories = UnitOfWork.Categories.Get().Where(x => x.Name.Contains(name)).ToList();
                    if (categories.Count == 0) return NotFound();
                }
                else categories = UnitOfWork.Categories.Get().ToList();
                var list = categories.Skip(Pagination.PageSize * page)
                                    .Take(Pagination.PageSize)
                                    .Select(x => Factory.Create(x)).ToList();
                return Ok(Factory.Create<CategoryModel>(page, categories.Count, list));
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
