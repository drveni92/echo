using Billing.Database.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Database
{
    public class BillingConfiguration : DbConfiguration
    {
        public BillingConfiguration()
        {
            AddInterceptor(new SoftDeleteInterceptor());
        }
    }
}
