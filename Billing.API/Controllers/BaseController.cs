using Billing.API.Helper.Identity;
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
        private FactoryReports _factoryReport;
        private BillingIdentity _identity;

        protected UnitOfWork UnitOfWork { get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork()); } }

        protected Factory Factory { get { return _factory ?? (_factory = new Factory(UnitOfWork)); } }

        protected FactoryReports FactoryReport { get { return _factoryReport ?? (_factoryReport = new FactoryReports(UnitOfWork, Identity)); } }

        protected BillingIdentity Identity { get { return _identity ?? (_identity = new BillingIdentity()); } }
    }
}
