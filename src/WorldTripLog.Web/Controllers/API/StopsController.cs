using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorldTripLog.Domain.Entities;
using WorldTripLog.Web.Helpers;
using WorldTripLog.Web.Models;
using WorldTripLog.Web.Data;
using WorldTripLog.Web.Models.ViewModels;
using WorldTripLog.Web.Services;

namespace WorldTripLog.Web.Controllers.Api
{
    [Authorize(policy: "Authenticated")]
    [Route("api/trips/{tripID}/[controller]")]
    public class StopsController : BaseApiController
    {
        private readonly ILogger<StopsController> _logger;

        private readonly IDataService<WorldTripDbContext, Stop> _stops;

        private readonly GeoCoordsService _coordService;
        private readonly int _tripID;

        public StopsController(ILogger<StopsController> logger, IDataService<WorldTripDbContext, Stop> stops, GeoCoordsService coordService)
        {
            _logger = logger;
            _stops = stops;
            _coordService = coordService;
            _tripID = Convert.ToInt32(RouteData.Values["tripID"]);
        }

        /// <summary>
        /// get list of all the stops belonging to the trip with tripID
        /// </summary>
        /// <response code="200">
        /// returns list of all the stops
        /// </response>
        /// <response code="401">
        /// unauthorized
        /// </response>
        /// <response code="500">
        /// some internal errors
        /// </response>
        [ProducesResponseType(typeof(IEnumerable<StopVModel>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 401)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Expression<Func<Stop, bool>> filter = s => s.TripID == _tripID && s.CreatedBy == UserID;

                var stops = await _stops.GetAsync(filter: filter);
                return stops.Any() ? Ok((stops.Select(Mappings.ToStopVModel))) : throw new InvalidOperationException(message: $"current trip: {_tripID} has not stops yet");
            }
            catch (Exception e)
            {
                _logger.LogError($"failed to execute GET: {e.Message}");
                return StatusCode(500, new ErrorMessage(500, e.Message));
            }
        }

        /// <summary>
        /// get the stop with the given id belonging to the trip with tripID
        /// </summary>
        /// <response code="200">
        /// returns the required stop
        /// </response>
        /// <response code="401">
        /// unauthorized
        /// </response>
        /// <response code="500">
        /// some internal errors
        /// </response>
        [ProducesResponseType(typeof(StopVModel), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 401)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Expression<Func<Stop, bool>> filterOne = (s) => s.TripID == _tripID && s.CreatedBy == UserID && s.Id == id;

                var stop = await _stops.GetOneAsync(filter: filterOne);
                return stop != null ? Ok(Mappings.ToStopVModel(stop)) : throw new InvalidOperationException(message: $"invalid trip: {_tripID} and stop: {id} combination");
            }
            catch (Exception e)
            {
                _logger.LogError($"failed to get stop: { e.Message}");
                return StatusCode(500, new ErrorMessage(500, e.Message));
            }
        }

        /// <summary>
        /// create a new stop under the trip with tripID
        /// </summary>
        /// <response code="201">
        /// returns the newly created stop
        /// </response>
        /// <response code="401">
        /// unauthorized
        /// </response>
        /// <response code="500">
        /// some internal errors or invalid tripID
        /// </response>
        [ProducesResponseType(typeof(StopVModel), 201)]
        [ProducesResponseType(typeof(ErrorMessage), 401)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]StopVModel stop)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var _stop = await _coordService.AddGeoCoords(Mappings.ToStopModel(stop));
                    _stop.TripID = _tripID;
                    await _stops.Create(_stop, UserID);
                    _logger.LogInformation($"{UserID} created a new stop on trip: {_tripID}");
                    return Created(Request.Path.Value, Mappings.ToStopVModel(_stop));
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{UserID} failed to create a new stop on trip: {_tripID}");
                    return StatusCode(500, new ErrorMessage(500, $"stop creation failed due to: {e.Message}"));
                }
            }
            else
            {
                _logger.LogError("invalid stop model");
                return StatusCode(500, new ErrorMessage(500, string.Join(", ", ModelState.ToStringResponse().ToArray())));
            }
        }

        /// <summary>
        /// update the existing stop with the given id
        /// </summary>
        /// <response code="200">
        /// returns the just updated stop</response>
        /// <response code="401">
        /// unauthorized
        /// </response>
        /// <response code="500">
        /// other errors
        /// </response>
        [ProducesResponseType(typeof(StopVModel), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 401)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]StopVModel stop)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _stops.Update(Mappings.ToStopModel(stop), UserID);
                    _logger.LogInformation($"{UserID} modified an existing stop: {stop.Id} on trip: {_tripID}");
                    return Created(Request.Path.Value, stop);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{UserID} failed in modifying an existing stop: {stop.Id} on trip: {_tripID}");
                    return StatusCode(500, new ErrorMessage(500, $"stop update failed due to: {e.Message}"));
                }
            }
            else
            {
                _logger.LogError($"input stop is not valid");
                return StatusCode(500, new ErrorMessage(500, string.Join(", ", ModelState.ToStringResponse().ToArray())));
            }
        }

        /// <summary>
        /// delete an existing stop with the given id
        /// </summary>
        /// <response code="200">
        /// returns the request status</response>
        /// <response code="401">
        /// unauthorized
        /// </response>
        /// <response code="500">
        /// internal server error(s)
        /// </response>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 401)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    await _stops.Delete(id);
                    _logger.LogInformation($"stop: {id} deleted successfully by {UserID}");
                    return Ok($"Deleted Successfully");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"delete for stop: {id} failed");
                    return StatusCode(500, new ErrorMessage(500, "Stop deletion failed {e.Message}"));
                }
            }
            else
            {
                _logger.LogError($"invalid stop id: {id}");
                return StatusCode(500, new ErrorMessage(500, $"invalid stop id: {id}"));
            }
        }
    }
}
