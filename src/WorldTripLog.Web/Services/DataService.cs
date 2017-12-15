
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorldTripLog.Data.Interfaces;
using WorldTripLog.Domain.Interfaces;

namespace WorldTripLog.Web.Services
{
    public class DataService<TContext, TEntity> : IDataService<TContext, TEntity> where TEntity : class, IEntity
    {
        private readonly IRepository<TContext> _repository;

        public DataService(IRepository<TContext> repository)
        {
            _repository = repository;
        }

        #region just all the getters

        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return _repository.GetAsync<TEntity>(filter, orderBy, includeProperties, skip, take);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return _repository.GetAllAsync<TEntity>(orderBy, includeProperties, skip, take);
        }

        public Task<TEntity> GetByIdAsync(object id)
        {
            return _repository.GetByIdAsync<TEntity>(id);
        }

        public Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.GetCountAsync<TEntity>(filter);
        }

        public Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.GetExistsAsync<TEntity>(filter);
        }

        public Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null)
        {
            return _repository.GetFirstAsync<TEntity>(filter, orderBy, includeProperties);
        }

        public Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null)
        {
            return _repository.GetOneAsync<TEntity>(filter, includeProperties);
        }

        #endregion end of getters

        #region create, update, delete and save
        public Task Create(TEntity entity, string createdBy = null)
        {
            _repository.Create<TEntity>(entity, createdBy);
            return _repository.SaveAsync();
        }

        public Task Delete(object id)
        {
            _repository.Delete<TEntity>(id);
            return _repository.SaveAsync();
        }

        public Task Delete(TEntity entity)
        {
            _repository.Delete<TEntity>(entity);
            return _repository.SaveAsync();
        }

        public Task Update(TEntity entity, string modifiedBy = null)
        {
            _repository.Update<TEntity>(entity, modifiedBy);
            return _repository.SaveAsync();
        }

        #endregion
    }
}
