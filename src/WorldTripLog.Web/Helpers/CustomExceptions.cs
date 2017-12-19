using System;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;

namespace WorldTripLog.Web.Helpers
{
    [Serializable]
    public class DbEntityValidationException : SystemException
    {
        public DbEntityValidationException()
        {
        }

        public DbEntityValidationException(string message) : base(message)
        {
        }

        public DbEntityValidationException(string message, Exception inner) : base(message, inner) { }

        protected DbEntityValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
