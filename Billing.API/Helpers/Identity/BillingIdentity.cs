using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Billing.API.Helpers.Identity
{
    public class BillingIdentity
    {
        private UnitOfWork _unitOfWork;

        public BillingIdentity(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public BillingIdentity()
        {

        }

        public Agent CurrentUser
        {
            get
            {
                if (!Thread.CurrentPrincipal.Identity.IsAuthenticated) return null;
                string name = Thread.CurrentPrincipal.Identity.Name;
                return _unitOfWork.Agents.Get().FirstOrDefault(x => x.Username == name);
            }
        }

        public bool HasRole(string role)
        {
            return Thread.CurrentPrincipal.IsInRole(role);
        }

    }
}