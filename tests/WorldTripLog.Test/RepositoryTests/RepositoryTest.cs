using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using WorldTripLog.Data.Interfaces;
using WorldTripLog.Data.Repositories;
using WorldTripLog.Domain.Entities;
using WorldTripLog.Web.Data;
using WorldTripLog.Web.Services;
using Xunit;
using WorldTripLog.Test.Helpers;
using System.Linq.Expressions;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace WorldTripLog.Test.RepositoryTest
{
    public class RepositoryTests
    {
        public class GetAllAsyncTests
        {
            private readonly Repository<WorldTripDbContext> _repository;
            private readonly WorldTripDbContext _context;

            public GetAllAsyncTests()
            {
                _context = ContextHelpers.InitContext();
                _repository = new Repository<WorldTripDbContext>(_context);
            }

            [Fact]
            public async void WithDefaultParamaters()
            {
                var trips = await _repository.GetAllAsync<Trip>();
                Assert.NotNull(trips);
                Assert.Equal(_context.Trips.Count(), trips.Count());
            }

            [Fact]
            public async void WithSkip()
            {
                var skip = 2;
                var trips = await _repository.GetAllAsync<Trip>(skip: skip);
                var contextTrips = _context.Trips.Skip(skip).AsEnumerable();
                Assert.NotNull(trips);
                Assert.Equal(contextTrips.Count(), trips.Count());
                Assert.Equal(contextTrips.First().Id, trips.First().Id);
            }

            [Fact]
            public async void WithTake()
            {
                var take = 2;
                var trips = await _repository.GetAllAsync<Trip>(take: take);
                var contextTrips = _context.Trips.Take(take).AsEnumerable();
                Assert.NotNull(trips);
                Assert.Equal(contextTrips.Count(), trips.Count());
                Assert.Equal(contextTrips.First().Id, trips.First().Id);
                Assert.Equal(contextTrips.Last().Id, trips.Last().Id);
            }

            [Fact]
            public async void WithOrderBy()
            {
                Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = (trip) => trip.OrderByDescending(t => t.Id);
                var trips = await _repository.GetAllAsync<Trip>(orderBy: orderBy);
                var contextTrips = _context.Trips.OrderByDescending(t => t.Id).AsEnumerable();
                Assert.Equal(contextTrips.First().Id, trips.First().Id);
            }

            [Fact]
            public async void WithIncludeProperties()
            {

                var trips = await _repository.GetAllAsync<Trip>(includeProperties: "Stops");
                var firstTrip = trips.First();
                Assert.NotNull(firstTrip.Stops);
                Assert.Equal(_context.Trips.First().Stops.Count(), firstTrip.Stops.Count());
            }

            [Fact]
            public async void WithSkipAndTake()
            {
                int skip1 = 1,
                    skip2 = 2,
                    take1 = 2,
                    take2 = 2;
                var trips1 = await _repository.GetAllAsync<Trip>(skip: skip1, take: take1);
                var trips2 = await _repository.GetAllAsync<Trip>(skip: skip2, take: take2);

                var contextTrip1 = _context.Trips.Skip(skip1).Take(take1).AsEnumerable();
                var contextTrip2 = _context.Trips.Skip(skip2).Take(take2).AsEnumerable();

                // validating trips1
                Assert.NotNull(trips1);
                Assert.Equal(contextTrip1.Count(), trips1.Count());
                Assert.Equal(contextTrip1.First().Id, trips1.First().Id);
                Assert.Equal(contextTrip1.Last().Id, trips1.Last().Id);

                // validating trips2
                Assert.NotNull(trips2);
                Assert.Equal(contextTrip2.Count(), trips2.Count());
                Assert.Equal(contextTrip2.First().Id, trips2.First().Id);
            }

            [Fact]
            public async void WithSkipTakeOrderByAndIncludeProperties()
            {
                Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = (trip) => trip.OrderByDescending(t => t.Id);
                var ppties = "Stops";
                int take = 2,
                    skip = 1;
                var trips = await _repository.GetAllAsync<Trip>(orderBy: orderBy, includeProperties: ppties, skip: skip, take: take);
                var contextTrips = _context.Trips.OrderByDescending(t => t.Id).Skip(skip).Take(take).Include(ppties).AsEnumerable();

                Assert.NotNull(trips);
                Assert.Equal(contextTrips.Count(), trips.Count());
                Assert.Equal(contextTrips.First().Id, trips.First().Id);
                Assert.NotNull(trips.First().Stops);
                Assert.Equal(contextTrips.First().Stops, trips.First().Stops);
                Assert.NotNull(trips.First().CreatedBy);
                Assert.Equal("testuser", trips.Last().CreatedBy);
            }
        }

        public class GetAsyncTests
        {
            private readonly Repository<WorldTripDbContext> _repository;
            private readonly WorldTripDbContext _context;

            public GetAsyncTests()
            {
                _context = ContextHelpers.InitContext();
                _repository = new Repository<WorldTripDbContext>(_context);
            }

            [Fact]
            public async void WithDefaultParamaters()
            {
                var trips = await _repository.GetAsync<Trip>();
                Assert.NotNull(trips);
                Assert.Equal(_context.Trips.Count(), trips.Count());
            }

            [Fact]
            public async void WithFilter()
            {
                Expression<Func<Trip, bool>> filter = (t) => t.CreatedBy == "testusers";
                var trips = await _repository.GetAsync<Trip>(filter: filter);
                Assert.NotNull(trips);
                Assert.Equal(_context.Trips.Where(filter).Count(), trips.Count());
            }

            [Fact]
            public async void WithSkip()
            {
                var skip = 2;
                var trips = await _repository.GetAsync<Trip>(skip: skip);
                var contextTrips = _context.Trips.Skip(skip).AsEnumerable();
                Assert.NotNull(trips);
                Assert.Equal(contextTrips.Count(), trips.Count());
                Assert.Equal(contextTrips.First().Id, trips.First().Id);
            }

            [Fact]
            public async void WithTake()
            {
                var take = 2;
                var trips = await _repository.GetAsync<Trip>(take: take);
                var contextTrips = _context.Trips.Take(take).AsEnumerable();
                Assert.NotNull(trips);
                Assert.Equal(contextTrips.Count(), trips.Count());
                Assert.Equal(contextTrips.First().Id, trips.First().Id);
                Assert.Equal(contextTrips.Last().Id, trips.Last().Id);
            }

            [Fact]
            public async void WithOrderBy()
            {
                Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = (trip) => trip.OrderByDescending(t => t.Id);
                var trips = await _repository.GetAsync<Trip>(orderBy: orderBy);
                var contextTrips = _context.Trips.OrderByDescending(t => t.Id).AsEnumerable();
                Assert.Equal(contextTrips.First().Id, trips.First().Id);
            }

            [Fact]
            public async void WithIncludeProperties()
            {

                var trips = await _repository.GetAsync<Trip>(includeProperties: "Stops");
                var firstTrip = trips.First();
                Assert.NotNull(firstTrip.Stops);
                Assert.Equal(_context.Trips.First().Stops.Count(), firstTrip.Stops.Count());
            }

            [Fact]
            public async void WithSkipAndTake()
            {
                int skip1 = 1,
                    skip2 = 2,
                    take1 = 2,
                    take2 = 2;
                var trips1 = await _repository.GetAsync<Trip>(skip: skip1, take: take1);
                var trips2 = await _repository.GetAsync<Trip>(skip: skip2, take: take2);

                var contextTrip1 = _context.Trips.Skip(skip1).Take(take1).AsEnumerable();
                var contextTrip2 = _context.Trips.Skip(skip2).Take(take2).AsEnumerable();

                // validating trips1
                Assert.NotNull(trips1);
                Assert.Equal(contextTrip1.Count(), trips1.Count());
                Assert.Equal(contextTrip1.First().Id, trips1.First().Id);
                Assert.Equal(contextTrip1.Last().Id, trips1.Last().Id);

                // validating trips2
                Assert.NotNull(trips2);
                Assert.Equal(contextTrip2.Count(), trips2.Count());
                Assert.Equal(contextTrip2.First().Id, trips2.First().Id);
            }

            [Fact]
            public async void WithSkipTakeOrderByAndIncludeProperties()
            {
                Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = (trip) => trip.OrderByDescending(t => t.Id);
                var ppties = "Stops";
                int take = 2,
                    skip = 1;
                var trips = await _repository.GetAsync<Trip>(orderBy: orderBy, includeProperties: ppties, skip: skip, take: take);
                var contextTrips = _context.Trips.OrderByDescending(t => t.Id).Skip(skip).Take(take).Include(ppties).AsEnumerable();

                Assert.NotNull(trips);
                Assert.Equal(contextTrips.Count(), trips.Count());
                Assert.Equal(contextTrips.First().Id, trips.First().Id);
                Assert.NotNull(trips.First().Stops);
                Assert.Equal(contextTrips.First().Stops, trips.First().Stops);
                Assert.NotNull(trips.First().CreatedBy);
                Assert.Equal("testuser", trips.Last().CreatedBy);
            }
        }

        public class GetOneAsyncTests
        {
            private readonly Repository<WorldTripDbContext> _repository;
            private readonly WorldTripDbContext _context;

            public GetOneAsyncTests()
            {
                _context = ContextHelpers.InitContext();
                _repository = new Repository<WorldTripDbContext>(_context);
            }

            [Fact]
            public async void WithFilter()
            {
                Expression<Func<Trip, bool>> filter = (t) => t.Name == "Tour of Europe";
                var trip = await _repository.GetOneAsync<Trip>(filter: filter);
                Assert.NotNull(trip);
                Assert.Equal(_context.Trips.Where(filter).SingleOrDefault(), trip);

                filter = (t) => t.Id == 4;
                trip = await _repository.GetOneAsync<Trip>(filter: filter);
                Assert.Null(trip);
            }

            [Fact]
            public async void WithIncludeProperties()
            {
                Expression<Func<Trip, bool>> filter = (t) => t.Name == "Tour of Europe";
                var trip = await _repository.GetOneAsync<Trip>(filter: filter, includeProperties: "Stops");
                var contextTrip = _context.Trips.Where(filter).SingleOrDefault();
                Assert.NotNull(trip.Stops);
                Assert.Equal(contextTrip.Stops.Count(), trip.Stops.Count());
                Assert.Equal(contextTrip.Stops, trip.Stops);
            }
        }

        public class GetFirstAsyncTests
        {
            private readonly Repository<WorldTripDbContext> _repository;
            private readonly WorldTripDbContext _context;

            public GetFirstAsyncTests()
            {
                _context = ContextHelpers.InitContext();
                _repository = new Repository<WorldTripDbContext>(_context);
            }

            [Fact]
            public async void WithFilter()
            {
                Expression<Func<Trip, bool>> filter = (t) => t.Name.Contains("Tour of");
                var trip = await _repository.GetFirstAsync<Trip>(filter: filter);
                Assert.NotNull(trip);
                Assert.Equal(_context.Trips.Where(filter).FirstOrDefault(), trip);
                Assert.NotEqual(_context.Trips.Where(filter).LastOrDefault(), trip);
            }

            [Fact]
            public async void WithFilterAndOrderBy()
            {
                Expression<Func<Trip, bool>> filter = (t) => t.Name.Contains("Tour of");
                Func<IQueryable<Trip>, IOrderedQueryable<Trip>> orderBy = (t) => t.OrderByDescending(tr => tr.Id);
                var trip = await _repository.GetFirstAsync<Trip>(filter: filter, orderBy: orderBy);
                Assert.NotNull(trip);
                Assert.NotEqual(_context.Trips.Where(filter).FirstOrDefault(), trip);
                Assert.Equal(_context.Trips.Where(filter).OrderByDescending(tr => tr.Id).FirstOrDefault(), trip);
            }

            [Fact]
            public async void WithIncludeProperties()
            {
                Expression<Func<Trip, bool>> filter = (t) => t.Name == "Tour of Europe";
                var trip = await _repository.GetFirstAsync<Trip>(filter: filter, includeProperties: "Stops");
                var contextTrip = _context.Trips.Where(filter).FirstOrDefault();
                Assert.NotNull(trip.Stops);
                Assert.Equal(contextTrip.Stops.Count(), trip.Stops.Count());
                Assert.Equal(contextTrip.Stops, trip.Stops);
            }
        }

        public class GetCountAsyncTests
        {
            private readonly Repository<WorldTripDbContext> _repository;
            private readonly WorldTripDbContext _context;

            public GetCountAsyncTests()
            {
                _context = ContextHelpers.InitContext();
                _repository = new Repository<WorldTripDbContext>(_context);
            }

            [Fact]
            public async void WithFilter()
            {
                Expression<Func<Trip, bool>> filter = (t) => t.Name.Contains("Tour of");
                var noOfTrips = await _repository.GetCountAsync<Trip>(filter: filter);
                Assert.NotEqual(0, noOfTrips);
                Assert.Equal(_context.Trips.Where(filter).Count(), noOfTrips);
            }
        }

        public class GetExistsAsyncTests
        {
            private readonly Repository<WorldTripDbContext> _repository;
            private readonly WorldTripDbContext _context;

            public GetExistsAsyncTests()
            {
                _context = ContextHelpers.InitContext();
                _repository = new Repository<WorldTripDbContext>(_context);
            }

            [Fact]
            public async void WithFilter()
            {
                Expression<Func<Trip, bool>> filter = (t) => t.Name == "Tope";
                var tripExists = await _repository.GetExistsAsync<Trip>(filter: filter);
                Assert.Equal(false, tripExists);

                filter = (t) => t.Name.Contains("Tour");
                tripExists = await _repository.GetExistsAsync<Trip>(filter: filter);
                Assert.Equal(true, tripExists);

                filter = (t) => t.Id == 4;
                tripExists = await _repository.GetExistsAsync<Trip>(filter: filter);
                Assert.Equal(false, tripExists);
            }
        }

        public class GetByIdAsyncTests
        {
            private readonly Repository<WorldTripDbContext> _repository;
            private readonly WorldTripDbContext _context;

            public GetByIdAsyncTests()
            {
                _context = ContextHelpers.InitContext();
                _repository = new Repository<WorldTripDbContext>(_context);
            }

            [Fact]
            public async void GetByIdAsyncReturnsExactItem()
            {
                var id = 1;
                var trip = await _repository.GetByIdAsync<Trip>(id);
                var contextTrip = _context.Trips.FindAsync(id).Result;

                Assert.NotNull(trip);
                Assert.IsType<Trip>(trip);
                Assert.Equal(id, trip.Id);
                Assert.Equal(contextTrip.Id, trip.Id);
                Assert.Equal(contextTrip.Name, trip.Name);
            }
        }

        public class CreateTests
        {
            private readonly Repository<WorldTripDbContext> _repository;
            private readonly WorldTripDbContext _context;

            public CreateTests()
            {
                _context = ContextHelpers.InitContextWithCreate();
                _repository = new Repository<WorldTripDbContext>(_context);
            }

            [Fact]
            public async void CreateTest()
            {
                var trip = new Trip
                {
                    Id = 4,
                    Name = "Lagos Tour",
                    CreatedDate = DateTime.UtcNow
                };
                string creator = "testuser";

                _repository.Create<Trip>(trip, creator);

                var trips = await _repository.GetAllAsync<Trip>();
                trips = await _repository.GetAllAsync<Trip>();
                Assert.Equal(4, trips.Count());

                var gotTrip = await _repository.GetByIdAsync<Trip>(trip.Id);
                Assert.NotNull(gotTrip);
                Assert.Equal(trip.Name, gotTrip.Name);
            }
        }
    }
}
