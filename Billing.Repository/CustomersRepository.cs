using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CustomersRepository : BillingRepository<Customer>
    {
        public CustomersRepository(BillingContext _context) : base(_context) { }

        public override void Update(Customer entity, int id)
        {
            Customer oldEntity = Get(id);
            if(oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Town = entity.Town;
            }
        }
    }
}
