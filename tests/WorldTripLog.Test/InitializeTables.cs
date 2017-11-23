using System;
using Xunit;

namespace WorldTripLog.Test
{
    public class InitializeMockData
    {
        [Fact]
        public void TripsIsNotEmpty()
        {
            Assert.NotEmpty(GlobalVariables.GetTrips());
        }

        [Fact]
        public void StopsIsNotEmpty()
        {
            Assert.NotEmpty(GlobalVariables.GetStops());
        }
    }
}
