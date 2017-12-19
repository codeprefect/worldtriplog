
using System;
using System.Collections.Generic;
using System.Linq;
using WorldTripLog.Domain.Entities;
using WorldTripLog.Web.Models.ViewModels;

namespace WorldTripLog.Web.Helpers
{
    public static class Mappings
    {
        #region trip mapping helpers

        public readonly static Func<Trip, TripVModel> ToTripVModel = (trip) =>
        {
            return new TripVModel
            {
                Id = trip.Id,
                Name = trip.Name,
                DateCreated = trip.CreatedDate
            };
        };

        public readonly static Func<TripVModel, Trip> ToTripModel = (trip) =>
        {
            return new Trip
            {
                Id = trip.Id,
                Name = trip.Name,
            };
        };

        #endregion trip mappings

        #region stop mapping helpers
        public readonly static Func<Stop, StopVModel> ToStopVModel = (stop) =>
        {
            return new StopVModel
            {
                Id = stop.Id,
                Name = stop.Name,
                Arrival = stop.Arrival,
                Latitude = stop.Latitude,
                Longitude = stop.Longitude,
                Order = stop.Order
            };
        };

        public readonly static Func<StopVModel, Stop> ToStopModel = (stop) =>
        {
            return new Stop
            {
                Id = stop.Id,
                Name = stop.Name,
                Arrival = stop.Arrival,
                Latitude = stop.Latitude,
                Longitude = stop.Longitude,
                Order = stop.Order
            };
        };

        #endregion stop mappings
    }
}
