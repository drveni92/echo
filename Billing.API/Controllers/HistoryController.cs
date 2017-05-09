using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Models;
using Billing.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [RoutePrefix("api/histories")]
    public class HistoryController : BaseController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                return Ok(UnitOfWork.Histories.Get().ToList().Select(x => Factory.Create(x)).ToList());
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
                Event history = UnitOfWork.Histories.Get(id);
                if (history == null) return NotFound();
                return Ok(Factory.Create(history));
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "ERROR");
                return BadRequest(ex.Message);
            }
        }

        [Route("")]
        public IHttpActionResult Post([FromBody]HistoryModel model)
        {
            try
            {
                Event history = Factory.Create(model);
                UnitOfWork.Histories.Insert(history);
                UnitOfWork.Commit();
                return Ok(Factory.Create(history));
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

        [Route("{id}")]
        public IHttpActionResult Put([FromUri]int id, [FromBody]HistoryModel model)
        {
            try
            {
                Event history = Factory.Create(model);
                UnitOfWork.Histories.Update(history,id);
                UnitOfWork.Commit();
                return Ok(Factory.Create(history));
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

    }
}
