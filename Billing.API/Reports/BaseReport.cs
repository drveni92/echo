using Billing.API.Helpers.Identity;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.API.Reports
{
    public abstract class BaseReport
    {
        public BaseReport(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _identity = new BillingIdentity(unitOfWork);
            _factory = new FactoryReports();
        }
        protected BillingIdentity _identity;
        protected FactoryReports _factory;
        protected UnitOfWork _unitOfWork;
    }
}