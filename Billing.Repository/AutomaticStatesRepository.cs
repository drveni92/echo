using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AutomaticStatesRepository : BillingRepository<AutomaticStates>
    {
        public AutomaticStatesRepository(BillingContext _context) : base(_context) { }

        public override void Update(AutomaticStates entity, int id)
        {
            AutomaticStates oldEntity = Get(id);
            if(oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Invoice = entity.Invoice;
                context.SaveChanges();
            }
        }
    }
}
