using System;
using System.ComponentModel.DataAnnotations;
using WorldTripLog.Domain.Interfaces;

namespace WorldTripLog.Web.Models.ViewModels
{
    public class TripVModel : IModifiableEntity
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
