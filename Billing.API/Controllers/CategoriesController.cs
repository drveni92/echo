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

        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            return (name != null) ? Ok(UnitOfWork.Categories.Get().Where(x => x.Name.Contains(name)).ToList().Select(x => Factory.Create(x)).ToList()) :
                                    Ok(UnitOfWork.Categories.Get().ToList().Select(x => Factory.Create(x)).ToList());
        }


        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            Category category = UnitOfWork.Categories.Get(id);
            if (category == null) return NotFound();
            return Ok(Factory.Create(category));
        }

    }
}
