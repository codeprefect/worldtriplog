
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorldTripLog.Data.Interfaces;
using WorldTripLog.Domain.Entities;
using WorldTripLog.Domain.Interfaces;
using WorldTripLog.Web.Data;
using WorldTripLog.Web.Services.Interfaces;

namespace WorldTripLog.Web.Services
{
    public class StopService : IStopService
    {
        private readonly IRepository<WorldTripDbContext> _repository;

        public StopService(IRepository<WorldTripDbContext> repository)
        {
            _repository = repository;
        }

        #region just all the getters

        public Task<IEnumerable<Stop>> GetAsync(Expression<Func<Stop, bool>> filter, Func<IQueryable<Stop>, IOrderedQueryable<Stop>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return _repository.GetAsync<Stop>(filter, orderBy, includeProperties, skip, take);
        }

        public Task<IEnumerable<Stop>> GetAllAsync(Func<IQueryable<Stop>, IOrderedQueryable<Stop>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return _repository.GetAllAsync<Stop>(orderBy, includeProperties, skip, take);
        }

        public Task<Stop> GetByIdAsync(object id)
        {
            return _repository.GetByIdAsync<Stop>(id);
        }

        public Task<int> GetCountAsync(Expression<Func<Stop, bool>> filter)
        {
            return _repository.GetCountAsync<Stop>(filter);
        }

        public Task<bool> GetExistsAsync(Expression<Func<Stop, bool>> filter)
        {
            return _repository.GetExistsAsync<Stop>(filter);
        }

        public Task<Stop> GetFirstAsync(Expression<Func<Stop, bool>> filter = null, Func<IQueryable<Stop>, IOrderedQueryable<Stop>> orderBy = null, string includeProperties = null)
        {
            return _repository.GetFirstAsync<Stop>(filter, orderBy, includeProperties);
        }

        public Task<Stop> GetOneAsync(Expression<Func<Stop, bool>> filter = null, string includeProperties = null)
        {
            return _repository.GetOneAsync<Stop>(filter, includeProperties);
        }

        #endregion end of getters

        #region create, update, delete and save
        public Task Create(Stop entity, string createdBy = null)
        {
            _repository.Create<Stop>(entity, createdBy);
            return _repository.SaveAsync();
        }

        public Task Delete(object id)
        {
            _repository.Delete<Stop>(id);
            return _repository.SaveAsync();
        }

        public Task Delete(Stop entity)
        {
            _repository.Delete<Stop>(entity);
            return _repository.SaveAsync();
        }

        public Task Update(Stop entity, string modifiedBy = null)
        {
            _repository.Update<Stop>(entity, modifiedBy);
            return _repository.SaveAsync();
        }

        #endregion
    }
}
