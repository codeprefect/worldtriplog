using System;
using System.Collections.Generic;
using WorldTripLog.Web.DAL;

namespace WorldTripLog.Web.Models
{
    public class Trip : Entity<int>
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public ICollection<Stop> Stops { get; set; }
    }
}
