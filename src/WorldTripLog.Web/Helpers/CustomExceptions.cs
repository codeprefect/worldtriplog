using System;
using Microsoft.EntityFrameworkCore;

namespace WorldTripLog.Web.Helpers
{
    public class DbEntityValidationException : DbUpdateException
    {
        public DbEntityValidationException(string message, Exception inner) : base(message, inner) { }
    }
}
