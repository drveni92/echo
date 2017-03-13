using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SuppliersRepository : BillingRepository<Supplier>
    {
        public SuppliersRepository(BillingContext _context) : base(_context) { }

        public override void Update(Supplier entity, int id)
        {
            Supplier oldEntity = Get(id);
            if(oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Town = entity.Town;
            }
        }
    }
}
