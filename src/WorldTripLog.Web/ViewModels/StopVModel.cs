using System;
using System.ComponentModel.DataAnnotations;
using WorldTripLog.Web.DAL;

namespace WorldTripLog.Web.Models.ViewModels
{
    public class StopVModel : IModifiableEntity
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]

        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public DateTime Arrival { get; set; }
    }
}
