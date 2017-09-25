
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorldTripLog.Web.Helpers;

namespace WorldTripLog.Web.DAL
{
    public class Repository<TContext> : ReadOnlyRepository<TContext>, IRepository<TContext> where TContext : DbContext
    {
        public Repository(TContext context) : base(context)
        { }

        public virtual void Create<TEntity>(TEntity entity, string createdBy = null) where TEntity : class, IEntity
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedBy = createdBy;
            _context.Set<TEntity>().Add(entity);
        }

        public virtual void Update<TEntity>(TEntity entity, string modifiedBy = null) where TEntity : class, IEntity
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.ModifiedBy = modifiedBy;
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Detached;
        }

        public virtual void Delete<TEntity>(object id) where TEntity : class, IEntity
        {
            TEntity entity = _context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            var dbSet = _context.Set<TEntity>();
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public virtual Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
