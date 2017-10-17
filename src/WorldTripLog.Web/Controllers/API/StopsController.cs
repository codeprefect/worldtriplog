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

        public IDataService<WorldTripDbContext, Stop> _stops { get; }

        private readonly GeoCoordsService _coordService;

        public StopsController(ILogger<StopsController> logger, IDataService<WorldTripDbContext, Stop> stops, GeoCoordsService coordService)
        {
            _logger = logger;
            _stops = stops;
            _coordService = coordService;
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
                Expression<Func<Stop, bool>> filter = s => s.TripID == TripID && s.CreatedBy == UserID;

                var stops = await _stops.GetAsync(filter: filter);
                return stops.Any() ? Ok(stops.ToVModel()) : throw new InvalidOperationException(message: "current user has not trips yet");
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to execute GET: {ex.Message}");
                return StatusCode(500, new ErrorMessage
                {
                    reasons = { ex.Message }
                });
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
                Expression<Func<Stop, bool>> filterOne = (s) => s.TripID == TripID && s.CreatedBy == UserID && s.Id == id;

                var stop = await _stops.GetOneAsync(filter: filterOne);
                return stop != null ? Ok(stop.ToVModel()) : throw new InvalidOperationException(message: "invalid tripID and stopID combination");
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to get stop: ${ ex.Message}");
                return StatusCode(500, new ErrorMessage
                {
                    reasons = { ex.Message }
                });
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
                    var _stop = await _coordService.AddGeoCoords(stop.ToModel());
                    _stop.TripID = TripID;
                    _stops.Create(_stop, UserID);
                    await _stops.SaveAsync();
                    return Created($"/api/trips/{TripID}/stops", _stop.ToVModel());
                }
                catch (Exception e)
                {
                    return StatusCode(500, new ErrorMessage
                    {
                        reasons = { $"stop creation failed due to: {e.Message}" }
                    });
                }
            }
            else
            {
                return StatusCode(500, new ErrorMessage
                {
                    reasons = ModelState.ToStringResponse()
                });
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
                    _stops.Update(stop.ToModel(), UserID);
                    await _stops.SaveAsync();
                    return Created($"/api/trips/{TripID}/stops/{stop.Id}", stop);
                }
                catch (Exception e)
                {
                    return StatusCode(500, new ErrorMessage
                    {
                        reasons = { $"stop update failed due to: {e.Message}" }
                    });
                }
            }
            else
            {
                return StatusCode(500, new ErrorMessage
                {
                    reasons = ModelState.ToStringResponse()
                });
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
                    _stops.Delete(id);
                    await _stops.SaveAsync();
                    return Ok($"Deleted Successfully");
                }
                catch (Exception e)
                {
                    return StatusCode(500, new ErrorMessage
                    {
                        message = "Stop deletion failed",
                        reasons = { e.Message }
                    });
                }
            }
            else
            {
                return StatusCode(500, new ErrorMessage
                {
                    reasons = { $"invalid stop id: {id}" }
                });
            }
        }

        #region helpers
        private int TripID
        {
            get => Convert.ToInt32(RouteData.Values["tripID"]);
        }

        #endregion
    }
}
