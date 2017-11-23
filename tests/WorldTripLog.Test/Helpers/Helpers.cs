using Moq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WorldTripLog.Domain.Entities;

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
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryableList.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryableList.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(queryableList.GetEnumerator());
            dbSetMock.Setup(x => x.Find(It.IsAny<object[]>())).Returns<object[]>(ids => queryableList.SingleOrDefault(d => d.Id == (int)ids[0]));

            return dbSetMock;
        }

        public static Mock<DbSet<T>> AsDbSetMockWithCreate<T>(this List<T> list) where T : Entity<int>
        {
            IQueryable<T> queryableList = list.AsQueryable();
            //Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();
            Mock<DbSet<T>> dbSetMock = AsDbSetMock(list);

            dbSetMock.Setup(x => x.Add(It.IsAny<T>())).Callback<T>((s) => list.Append(s));

            return dbSetMock;
        }
    }
}
