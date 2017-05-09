using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AuthTokenRepository : BillingRepository<AuthToken>
    {
        public AuthTokenRepository(BillingContext _context) : base(_context) { }

        public override void Update(AuthToken entity, int id)
        {
            AuthToken oldEntity = Get(id);
            if (oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Agent = entity.Agent;
                oldEntity.ApiUser = entity.ApiUser;
                context.SaveChanges();
            }
        }
    }
}
