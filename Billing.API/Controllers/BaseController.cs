using Billing.API.Helpers.Identity;
using Billing.API.Models;
using Billing.API.Reports;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Billing.API.Controllers
{
    public class BaseController : ApiController
    {
        private UnitOfWork _unitOfWork;
        private Factory _factory;
        private BillingIdentity _identity;
        private SetOfReports _reports;

        protected UnitOfWork UnitOfWork { get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork()); } }

        protected Factory Factory { get { return _factory ?? (_factory = new Factory(UnitOfWork)); } }

        protected BillingIdentity Identity { get { return _identity ?? (_identity = new BillingIdentity()); } }

        protected SetOfReports Reports { get { return _reports ?? (_reports = new SetOfReports(UnitOfWork)); } }
    }
}
