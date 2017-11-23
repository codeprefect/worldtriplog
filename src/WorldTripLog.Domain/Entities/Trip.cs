using System;
using System.Collections.Generic;

namespace WorldTripLog.Domain.Entities
{
    public class Trip : Entity<int>
    {
        public virtual ICollection<Stop> Stops { get; set; }
    }
}
