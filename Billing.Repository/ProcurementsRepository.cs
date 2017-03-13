using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProcurementsRepository : BillingRepository<Procurement>
    {
        public ProcurementsRepository(BillingContext _context) : base(_context) { }

        public override void Update(Procurement entity, int id)
        {
            Procurement oldEntity = Get(id);
            if(oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Product = entity.Product;
            }
        }
    }
}
