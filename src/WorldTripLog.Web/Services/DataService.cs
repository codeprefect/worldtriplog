
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorldTripLog.Web.DAL;
using WorldTripLog.Web.Models;

namespace WorldTripLog.Web.Services
{
    public class DataService<TEntity> : IDataService<TEntity> where TEntity : class, IEntity
    {
        private readonly IRepository _repository;

        public DataService(IRepository repository)
        {
            _repository = repository;
        }

        #region just all the getters

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties, int? skip, int? take)
        {
            return _repository.Get<TEntity>(filter, orderBy, includeProperties, skip, take);
        }

        public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties, int? skip, int? take)
        {
            return _repository.GetAsync<TEntity>(filter, orderBy, includeProperties, skip, take);
        }

        public IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties, int? skip, int? take)
        {
            return _repository.GetAll<TEntity>(orderBy, includeProperties, skip, take);
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties, int? skip, int? take)
        {
            return _repository.GetAllAsync<TEntity>(orderBy, includeProperties, skip, take);
        }

        public TEntity GetById(object id)
        {
            return _repository.GetById<TEntity>(id);
        }

        public Task<TEntity> GetByIdAsync(object id)
        {
            return _repository.GetByIdAsync<TEntity>(id);
        }

        public int GetCount(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.GetCount<TEntity>(filter);
        }

        public Task<int> GetCountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.GetCountAsync<TEntity>(filter);
        }

        public bool GetExists(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.GetExists<TEntity>(filter);
        }

        public Task<bool> GetExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _repository.GetExistsAsync<TEntity>(filter);
        }

        public TEntity GetFirst(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties)
        {
            return _repository.GetFirst<TEntity>(filter, orderBy, includeProperties);
        }

        public Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties)
        {
            return _repository.GetFirstAsync<TEntity>(filter, orderBy, includeProperties);
        }

        public TEntity GetOne(Expression<Func<TEntity, bool>> filter, string includeProperties)
        {
            return _repository.GetOne<TEntity>(filter, includeProperties);
        }

        public Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter, string includeProperties)
        {
            return _repository.GetOneAsync<TEntity>(filter, includeProperties);
        }

        #endregion end of getters

        #region create, update, delete and save
        public void Create(TEntity TEntity, string createdBy)
        {
            _repository.Create<TEntity>(TEntity, createdBy);
        }

        public void Delete(object id)
        {
            _repository.Delete<TEntity>(id);
        }

        public void Delete(TEntity entity)
        {
            _repository.Delete<TEntity>(entity);
        }

        public void Update(TEntity entity, string modifiedBy)
        {
            _repository.Update<TEntity>(entity, modifiedBy);
        }

        public void Save()
        {
            _repository.Save();
        }

        public Task SaveAsync()
        {
            return _repository.SaveAsync();
        }

        #endregion
    }
}
