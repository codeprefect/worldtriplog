
using System;
using System.Collections.Generic;

namespace WorldTripLog.Domain.Entities
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            this.status = 500;
            this.message = message;
        }
        public ErrorMessage(int status, string message)
        {
            this.status = status;
            this.message = message;
        }
        public readonly int status;
        public readonly string message;
    }
}
