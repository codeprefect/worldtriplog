using System;

namespace WorldTripLog.Domain.Entities
{
    public class Stop : Entity<int>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Arrival { get; set; }
        public int Order { get; set; }
        public int TripID { get; set; }
        public Trip Trip { get; set; }
    }
}
