using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class InvoicesRepository : BillingRepository<Invoice>
    {
        public InvoicesRepository(BillingContext _context) : base(_context) { }

        public override void Update(Invoice entity, int id)
        {
            Invoice oldEntity = Get(id);
            if(oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Agent = entity.Agent;
                oldEntity.Customer = entity.Customer;
                oldEntity.Shipper = entity.Shipper;
            }
        }
    }
}
