using System;
using WorldTripLog.Web.DAL;

namespace WorldTripLog.Web.Models
{
    public class Stop : Entity<int>
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Arrival { get; set; }
        public int Order { get; set; }
    }
}
