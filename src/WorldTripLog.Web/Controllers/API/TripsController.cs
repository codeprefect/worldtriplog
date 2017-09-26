using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorldTripLog.Web.Data;
using WorldTripLog.Web.Models;
using WorldTripLog.Web.Services;

namespace WorldTripLog.Web.Controllers.Api
{
    //[Authorize]
    [Route("api/[controller]")]
    public class TripsController : BaseApiController
    {
        private readonly ILogger<TripsController> _logger;

        public IDataService<WorldTripDbContext, Trip> _trips { get; }

        public TripsController(ILogger<TripsController> logger, IDataService<WorldTripDbContext, Trip> trips)
        {
            _logger = logger;
            _trips = trips;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var trips = await _trips.GetAsync(filter: (t) => t.CreatedBy == UserID);
                return trips.Any() ? Ok(trips) : throw new InvalidOperationException();
            }
            catch (Exception)
            {
                _logger.LogError("failed to execute GET");
                return BadRequest(new ErrorMessage
                {
                    message = "failed to get trips",
                    reason = "probably no trips yet."
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var trip = await _trips.GetOneAsync(filter: (t) => t.CreatedBy == UserID && t.Id == id);
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
        public async Task<IActionResult> Post([FromBody]Trip trip)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _trips.Create(trip, UserID);
                    await _trips.SaveAsync();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]Trip trip)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _trips.Update(trip, UserID);
                    await _trips.SaveAsync();
                    return Created($"/api/trips/{trip.Id}", trip);
                }
                catch (Exception e)
                {
                    return BadRequest($"trip update failed due to: {e.Message}");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    _trips.Delete(id);
                    await _trips.SaveAsync();
                    return Ok($"Deleted Successfully");
                }
                catch (Exception e)
                {
                    return BadRequest($"trip deletion failed due to: {e.Message}");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
