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
        public string CurrentUser
        {
            get
            {
                if (!Thread.CurrentPrincipal.Identity.IsAuthenticated) return null;
                return Thread.CurrentPrincipal.Identity.Name;
            }
        }
    }
}