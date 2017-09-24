using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorldTripLog.Web.DAL;
using WorldTripLog.Web.Models;
using WorldTripLog.Web.Services;

namespace WorldTripLog.Web.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class TripsController : BaseApiController
    {
        private readonly ILogger<TripsController> _logger;

        public IDataService<Trip> _data { get; }

        public TripsController(ILogger<TripsController> logger, IDataService<Trip> data)
        {
            _logger = logger;
            _data = data;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_data.GetAll());
            }
            catch (Exception)
            {
                _logger.LogError("Failed to execute GET");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                _logger.LogInformation(UserId);
                return Ok(_data.GetById(id));
            }
            catch (Exception e)
            {
                _logger.LogError("Trip with the specified id does not exist: {0}", e.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Trip trip)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    trip.CreatedBy = UserId;
                    trip.CreatedDate = DateTime.UtcNow;
                    _data.Create(trip);
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
