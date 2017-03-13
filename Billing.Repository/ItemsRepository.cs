using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ItemsRepository : BillingRepository<Item>
    {
        public ItemsRepository(BillingContext _context) : base(_context) { }

        public override void Update(Item entity, int id)
        {
            Item oldEntity = Get(id);
            if (oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Product = entity.Product;
                oldEntity.Invoice = entity.Invoice;
            }
        }
    }
}
