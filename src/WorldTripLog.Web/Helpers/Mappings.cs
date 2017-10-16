
using System.Collections.Generic;
using System.Linq;
using WorldTripLog.Domain.Entities;
using WorldTripLog.Web.Models.ViewModels;

namespace WorldTripLog.Web.Helpers
{
    public static class Mappings
    {
        #region trip mapping helpers
        public static TripVModel ToVModel(this Trip trip) =>
            new TripVModel
            {
                Id = trip.Id,
                Name = trip.Name,
                DateCreated = trip.CreatedDate
            };

        public static IEnumerable<TripVModel> ToVModel(this IEnumerable<Trip> trips) => trips.Select(ToVModel);

        public static Trip ToModel(this TripVModel trip) =>
            new Trip
            {
                Id = trip.Id,
                Name = trip.Name,
            };

        public static IEnumerable<Trip> ToModel(this IEnumerable<TripVModel> trips) => trips.Select(ToModel);

        #endregion trip mappings

        #region stop mapping helpers
        public static StopVModel ToVModel(this Stop stop) =>
            new StopVModel
            {
                Id = stop.Id,
                Name = stop.Name,
                Arrival = stop.Arrival,
                Latitude = stop.Latitude,
                Longitude = stop.Longitude,
                Order = stop.Order
            };

        public static IEnumerable<StopVModel> ToVModel(this IEnumerable<Stop> Stops) => Stops.Select(ToVModel);

        public static Stop ToModel(this StopVModel stop) =>
            new Stop
            {
                Id = stop.Id,
                Name = stop.Name,
                Arrival = stop.Arrival,
                Latitude = stop.Latitude,
                Longitude = stop.Longitude,
                Order = stop.Order
            };

        public static IEnumerable<Stop> ToModel(this IEnumerable<StopVModel> Stops) => Stops.Select(ToModel);

        #endregion stop mappings
    }
}
