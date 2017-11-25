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
    public class GenericRepositoryTest
    {
        private static WorldTripDbContext InitRepo()
        {
            var builder = new DbContextOptionsBuilder<WorldTripDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var context = new WorldTripDbContext(builder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Trips.AddRange(GlobalVariables.GetTrips());
            context.Stops.AddRange(GlobalVariables.GetStops());
            context.SaveChanges();
            return context;
        }

        public class GetAllAsyncTests
        {
            private readonly Repository<WorldTripDbContext> _repository;
            private readonly WorldTripDbContext _context;

            public GetAllAsyncTests()
            {
                _context = GenericRepositoryTest.InitRepo();
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
                var contextTrips = _context.Trips.Include("Stops");

                var firstTrip = trips.First();
                var contextFirstTrip = contextTrips.First();
                Assert.NotNull(firstTrip.Stops);
                Assert.Equal(contextFirstTrip.Stops.Count(), firstTrip.Stops.Count());
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

        public class GetByIdAsyncTests
        {
            private readonly Repository<WorldTripDbContext> _repository;
            private readonly WorldTripDbContext _context;

            public GetByIdAsyncTests()
            {
                _context = GenericRepositoryTest.InitRepo();
                _repository = new Repository<WorldTripDbContext>(_context);
            }

            [Fact]
            public async void GetByIdAsyncReturnsExactItem()
            {
                var id = 1;
                var trip = await _repository.GetByIdAsync<Trip>(id);
                var contextTrip = _context.Trips.Find(id);

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
                _context = GenericRepositoryTest.InitRepo();
                _repository = new Repository<WorldTripDbContext>(_context);
            }

            [Fact]
            public async void CreateTest()
            {
                var trip = new Trip
                {
                    Name = "Lagos Tour",
                    CreatedDate = DateTime.UtcNow
                };
                string creator = "testuser";

                _repository.Create<Trip>(trip, creator);
                await _repository.SaveAsync();

                var trips = await _repository.GetAllAsync<Trip>();
                trips = await _repository.GetAllAsync<Trip>();
                Assert.Equal(4, trips.Count());
            }
        }
    }
}
