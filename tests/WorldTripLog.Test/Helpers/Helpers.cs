using Moq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WorldTripLog.Domain.Entities;
using WorldTripLog.Web.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using System.Threading;

namespace WorldTripLog.Test.Helpers
{
    public static class DbSetMock
    {
        public static Mock<DbSet<T>> Create<T>(params T[] elements) where T : Entity<int>
        {
            return new List<T>(elements).AsDbSetMock();
        }
    }

    public static class ListExtensions
    {
        public static Mock<DbSet<T>> AsDbSetMock<T>(this List<T> list) where T : Entity<int>
        {
            IQueryable<T> queryableList = list.AsQueryable();
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();

            dbSetMock = dbSetMock.AsyncActive(queryableList);

            return dbSetMock;
        }

        public static Mock<DbSet<T>> AsyncActive<T>(this Mock<DbSet<T>> dbSetMock, IQueryable<T> queryableList) where T : Entity<int>
        {
            dbSetMock.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => new TestAsyncEnumerator<T>(queryableList.GetEnumerator()));

            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(queryableList.Provider));

            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.Expression)
                .Returns(queryableList.Expression);

            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.ElementType)
                .Returns(queryableList.ElementType);

            dbSetMock.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .Returns<object[]>(ids => Task.Run(() => queryableList.FirstOrDefault(t => t.Id == (int)ids[0])));

            // dbSetMock.As<IQueryable<T>>()
            //     .Setup(m => m.Include(It.IsAny<string>()))
            //     .Returns(dbSetMock.Object);

            return dbSetMock;
        }

        public static Mock<DbSet<T>> AsDbSetMockWithCreate<T>(this List<T> list) where T : Entity<int>
        {
            IQueryable<T> queryableList = list.AsQueryable();
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();

            dbSetMock = dbSetMock.AsyncActive(queryableList);

            dbSetMock.Setup(x => x.Add(It.IsAny<T>())).Callback<T>((s) => list.Add(s));
            dbSetMock.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .Returns<object[]>(ids => Task.Run(() => list.FirstOrDefault(t => t.Id == (int)ids[0])));

            return dbSetMock;
        }
    }

    public static class ContextHelpers
    {
        public static Mock<WorldTripDbContext> CreateDbContext(Mock<DbSet<Trip>> trips, Mock<DbSet<Stop>> stops)
        {
            var mockContext = new Mock<WorldTripDbContext>();
            mockContext.Setup(m => m.Trips).Returns(trips.Object);
            mockContext.Setup(m => m.Stops).Returns(stops.Object);

            mockContext.Setup(m => m.Set<Trip>()).Returns(trips.Object);
            mockContext.Setup(m => m.Set<Stop>()).Returns(stops.Object);

            return mockContext;
        }

        public static WorldTripDbContext InitContext()
        {
            var tripsMock = GlobalVariables.GetTrips().AsDbSetMock();
            var stopsMock = GlobalVariables.GetStops().AsDbSetMock();

            var worldTripDbContextMock = CreateDbContext(tripsMock, stopsMock);
            return worldTripDbContextMock.Object;
        }

        public static WorldTripDbContext InitContextWithCreate()
        {
            var tripsMock = GlobalVariables.GetTrips().AsDbSetMockWithCreate();
            var stopsMock = GlobalVariables.GetStops().AsDbSetMockWithCreate();
            var worldTripDbContextMock = CreateDbContext(tripsMock, stopsMock);
            return worldTripDbContextMock.Object;
        }
    }
}
