using Billing.Database;
using Billing.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
  
    public class AgentsRepository : BillingRepository<Agent>
    {
        public AgentsRepository(BillingContext _context) : base(_context) { }

        public override void Insert(Agent entity)
        {
            context.Agents.Add(entity);
            entity.Towns.ForEach(x => context.Entry(x).State = EntityState.Unchanged);
            context.SaveChanges();
        }

        public override void Update(Agent entity, int id)
        {
            Agent oldEntity = Get(id);
            if (oldEntity != null)
            {
                context.Entry(oldEntity).CurrentValues.SetValues(entity);
                oldEntity.Towns.Clear();
                oldEntity.Towns = entity.Towns;
                entity.Towns.ForEach(x => context.Entry(x).State = EntityState.Unchanged);
                context.SaveChanges();
            }
        }
    }
}
