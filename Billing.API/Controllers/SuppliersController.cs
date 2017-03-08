
using Billing.API.Models;
using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [RoutePrefix("api/suppliers")]
    public class SuppliersController : BaseController
    {
        [Route("{name?}")]
        public IHttpActionResult Get(string name = null)
        {
            return (name != null) ? Ok(UnitOfWork.Suppliers.Get().Where(x => x.Name.Contains(name)).ToList().Select(a => Factory.Create(a)).ToList()) :
                                    Ok(UnitOfWork.Suppliers.Get().ToList().Select(a => Factory.Create(a)).ToList());
        }
        [Route("town/{town}")]
        public IHttpActionResult GetSuppliersByTown(string town)
        {
            return Ok(UnitOfWork.Suppliers.Get().Where(x => x.Town.Name.Contains(town)).ToList().Select(x => Factory.Create(x)).ToList());

        }

        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            Supplier supplier = UnitOfWork.Suppliers.Get(id);
            if (supplier == null) return NotFound();
            return Ok(Factory.Create(supplier));
        }
    }
}
