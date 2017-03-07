using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Repository
{
    public interface IBillingRepository<Entity>
    {
        Entity Get(int id);
        IQueryable<Entity> Get();

        void Insert(Entity entity);
        void Update(Entity entity, int id);
        void Delete(int id);

        bool Commit();
    }
}
