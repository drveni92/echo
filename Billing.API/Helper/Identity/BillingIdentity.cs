using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Billing.API.Helper.Identity
{
    public class BillingIdentity
    {
        public string CurrentUser
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.Name;
            }
        }
    }
}