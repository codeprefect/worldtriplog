using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorldTripLog.Data.Repositories;
using WorldTripLog.Domain.Entities;
using WorldTripLog.Web.Data;

namespace WorldTripLog.Test
{
    public static class GlobalVariables
    {
        private static string _user = "testuser";
        private static List<Trip> _trips { get; set; } = new List<Trip> {
            new Trip {
                Id = 1,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _user,
                Name = "Tour of Africa"
            },
            new Trip {
                Id = 2,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _user,
                Name = "Tour of Europe"
            },
            new Trip {
                Id = 3,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _user,
                Name = "Ride of Asia"
            }
        };

        private static List<Stop> _stops { get; set; } = new List<Stop> {
            new Stop {
                Id = 1,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _user,
                Arrival = new DateTime(2017, 10, 1),
                Name = "Lagos, NG",
                TripID = 1
            },
            new Stop {
                Id = 2,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _user,
                Arrival = new DateTime(2017, 10, 2),
                Name = "Addis-Ababa, Ethiopia",
                TripID = 1
            },
            new Stop {
                Id = 3,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _user,
                Arrival = new DateTime(2017, 10, 3),
                Name = "New York, USA",
                TripID = 1
            },
            new Stop {
                Id = 4,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _user,
                Arrival = new DateTime(2017, 10, 4),
                Name = "Ontario, Canada",
                TripID = 2
            },
            new Stop {
                Id = 5,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _user,
                Arrival = new DateTime(2017, 10, 5),
                Name = "Birmingham, UK",
                TripID = 3
            },
            new Stop {
                Id = 6,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _user,
                Arrival = new DateTime(2017, 10, 6),
                Name = "Accra, Ghana",
                TripID = 3
            }
        };

        public static List<Trip> GetTrips()
        {
            var trips = new List<Trip>();
            foreach (var trip in _trips)
            {
                trip.Stops = new List<Stop>();
                foreach (var stop in _stops.Where(s => s.TripID == trip.Id))
                {
                    trip.Stops.Add(stop);
                }
                trips.Add(trip);
            }
            return trips;
        }

        public static List<Stop> GetStops()
        {
            var stops = new List<Stop>();
            foreach (var stop in _stops)
            {
                stop.Trip = _trips.Where(t => t.Id == stop.Id).SingleOrDefault();
                stops.Add(stop);
            }
            return stops;
        }
    }
}
