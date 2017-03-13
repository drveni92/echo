using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductsRepository : BillingRepository<Product>
    {
        public ProductsRepository(BillingContext _context) : base(_context) { }

        public override void Update(Product entity, int id)
        {
            Product oldEntity = Get(id);
            if(oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Category = entity.Category;
            }
        }
    }
}
