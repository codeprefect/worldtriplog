
using System.Collections.Generic;

namespace WorldTripLog.Domain.Entities
{
    public class ErrorMessage
    {
        public string message { get; set; } = "some unexpected error occured";
        public List<string> reasons { get; set; }
    }
}
