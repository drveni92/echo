﻿using Billing.API.Helpers;
using Billing.API.Helpers.Identity;
using Billing.API.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    [TokenAuthorization("user, admin")]
    public class SalesByAgentController : BaseController
    {
       
        public IHttpActionResult Post([FromBody]ReportRequestModel request)
        {
            try
            {
                if (Identity.HasNotAccess(request.Id)) return Unauthorized();
                return Ok(Reports.SalesByRegion.Report(request.StartDate, request.EndDate, request.Id));
            }

            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
