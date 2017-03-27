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
    [TokenAuthorization("user")]
    [RoutePrefix("api/categories")]
    public class CategoriesController : BaseController
    {

        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            try
            {
                if (name != null)
                {
                    var categories = UnitOfWork.Categories.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => Factory.Create(x)).ToList();
                    if (categories.Count != 0) return Ok(categories);
                    return NotFound();
                }
                return Ok(UnitOfWork.Categories.Get().ToList().Select(x => Factory.Create(x)).ToList());
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
