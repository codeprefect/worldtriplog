using System;
using System.Collections.Generic;
using WorldTripLog.Web.DAL;

namespace WorldTripLog.Web.Models
{
    public class Trip : Entity<int>
    {
        public ICollection<Stop> Stops { get; set; }
    }
}
