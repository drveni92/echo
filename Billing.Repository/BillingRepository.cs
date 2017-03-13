using Billing.Database;
using System;
using System.Data.Entity;
using System.Linq;

namespace Billing.Repository
{
    public class BillingRepository<Entity> : IBillingRepository<Entity> where Entity : class
    {
        protected BillingContext context;
        protected DbSet<Entity> dbSet;

        public BillingRepository(BillingContext _context)
        {
            context = _context;
            dbSet = context.Set<Entity>();
        }

        public bool Commit()
        {
            return (context.SaveChanges() > 0);
        }

        public void Delete(int id)
        {
            Entity entity = Get(id);
            if (entity == null) throw new ArgumentException("Entity not found");
            dbSet.Remove(entity);
        }

        public IQueryable<Entity> Get()
        {
            return dbSet;
        }

        public Entity Get(int id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(Entity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Update(Entity entity, int id)
        {
            Entity oldEntity = Get(id);
            context.Entry(oldEntity).CurrentValues.SetValues(entity);
        }
    }
}
