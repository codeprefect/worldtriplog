
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorldTripLog.Data.Interfaces;
using WorldTripLog.Domain.Entities;
using WorldTripLog.Domain.Interfaces;
using WorldTripLog.Web.Data;

namespace WorldTripLog.Web.Services
{
    public class TripService : IDataService<WorldTripDbContext, Trip>
    {
        private readonly IRepository<WorldTripDbContext> _repository;

        public TripService(IRepository<WorldTripDbContext> repository)
        {
            _repository = repository;
        }

        #region just all the getters

        public Task<IEnumerable<Trip>> GetAsync(Expression<Func<Trip, bool>> filter, Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return _repository.GetAsync<Trip>(filter, orderBy, includeProperties, skip, take);
        }

        public Task<IEnumerable<Trip>> GetAllAsync(Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null)
        {
            return _repository.GetAllAsync<Trip>(orderBy, includeProperties, skip, take);
        }

        public Task<Trip> GetByIdAsync(object id)
        {
            return _repository.GetByIdAsync<Trip>(id);
        }

        public Task<int> GetCountAsync(Expression<Func<Trip, bool>> filter)
        {
            return _repository.GetCountAsync<Trip>(filter);
        }

        public Task<bool> GetExistsAsync(Expression<Func<Trip, bool>> filter)
        {
            return _repository.GetExistsAsync<Trip>(filter);
        }

        public Task<Trip> GetFirstAsync(Expression<Func<Trip, bool>> filter = null, Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = null, string includeProperties = null)
        {
            return _repository.GetFirstAsync<Trip>(filter, orderBy, includeProperties);
        }

        public Task<Trip> GetOneAsync(Expression<Func<Trip, bool>> filter = null, string includeProperties = null)
        {
            return _repository.GetOneAsync<Trip>(filter, includeProperties);
        }

        #endregion end of getters

        #region create, update, delete and save
        public Task Create(Trip entity, string createdBy = null)
        {
            _repository.Create<Trip>(entity, createdBy);
            return _repository.SaveAsync();
        }

        public Task Delete(object id)
        {
            _repository.Delete<Trip>(id);
            return _repository.SaveAsync();
        }

        public Task Delete(Trip entity)
        {
            _repository.Delete<Trip>(entity);
            return _repository.SaveAsync();
        }

        public Task Update(Trip entity, string modifiedBy = null)
        {
            _repository.Update<Trip>(entity, modifiedBy);
            return _repository.SaveAsync();
        }

        #endregion
    }
}
