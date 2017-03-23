using Billing.Database;
using Billing.Repository;
using Billing.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed
{
    public static class ApiUserGenerator
    {
        public static void Get()
        {
            IBillingRepository<ApiUser> apiUsers = new BillingRepository<ApiUser>(Help.Context);
            apiUsers.Insert(new ApiUser()
            {
                Name = "Billing",
                AppId = "R2lnaVNjaG9vbA==",
                Secret = "TWlzdHJhbFRhbGVudHM="
            });
            apiUsers.Commit();
        }
    }
}
