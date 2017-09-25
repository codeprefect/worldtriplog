using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorldTripLog.Web.DAL;
using WorldTripLog.Web.Data;
using WorldTripLog.Web.Models;
using WorldTripLog.Web.Services;

namespace WorldTripLog.Web.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class TripsController : BaseApiController
    {
        private readonly ILogger<TripsController> _logger;

        public IDataService<WorldTripDbContext, Trip> _data { get; }

        public TripsController(ILogger<TripsController> logger, IDataService<WorldTripDbContext, Trip> data)
        {
            _logger = logger;
            _data = data;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var trips = await _data.GetAllAsync();
                return trips.Any() ? Ok(trips) : throw new InvalidOperationException();
            }
            catch (Exception)
            {
                _logger.LogError("Failed to execute GET");
                return BadRequest(new ErrorMessage
                {
                    message = "Failed to get trips",
                    reason = "probably no trips yet."
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation(UserId);
                var trip = await _data.GetByIdAsync(id);
                return trip != null ? Ok(trip) : throw new InvalidOperationException();
            }
            catch (Exception e)
            {
                _logger.LogError("Trip with the specified id does not exist: {0}", e.Message);
                return BadRequest(new ErrorMessage
                {
                    message = "requested resource does not exist",
                    reason = $"given id {id} is invalid"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Trip trip)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    trip.CreatedDate = DateTime.UtcNow;
                    _data.Create(trip, UserId);
                    await _data.SaveAsync();
                    return Created("/api/trips", trip);
                }
                catch (Exception e)
                {
                    return BadRequest($"trip creation failed due to: {e.Message}");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
