using System;
using Xunit;
using Shouldly;

namespace WorldTripLog.Test
{
    public class InitializeMockData
    {
        [Fact]
        public void TripsIsNotEmpty()
        {
            var trips = GlobalVariables.GetTrips();
            trips.ShouldNotBeNull();
            trips.ShouldNotBeEmpty();
        }

        [Fact]
        public void StopsIsNotEmpty()
        {
            var stops = GlobalVariables.GetStops();
            stops.ShouldNotBeNull();
            stops.ShouldNotBeEmpty();
        }
    }
}
