using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using WorldTripLog.Web.Data;
using WorldTripLog.Web.Helpers;
using WorldTripLog.Web.Models;
using WorldTripLog.Web.Models.ViewModels;
using WorldTripLog.Web.Services;

namespace WorldTripLog.Web.Controllers.Api
{
    [Authorize]
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
        /// <response code="400">
        /// if stops is empty
        /// </response>
        /// <response code="500">
        /// some internal errors
        /// </response>
        [ProducesResponseType(typeof(IEnumerable<StopVModel>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Expression<Func<Stop, bool>> filter = s => s.TripID == TripID && s.CreatedBy == UserID;

                var stops = await _stops.GetAsync(filter: filter);
                return stops.Any() ? Ok(stops.ToVModel()) : throw new InvalidOperationException();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorMessage
                {
                    message = "failed to get stops",
                    reason = "no stops on the given trip or trip do not exist"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to execute GET: {ex.Message}");
                return StatusCode(500, new ErrorMessage
                {
                    reason = ex.Message
                });
            }
        }

        /// <summary>
        /// get the stop with the given id belonging to the trip with tripID
        /// </summary>
        /// <response code="200">
        /// returns the required stop
        /// </response>
        /// <response code="400">
        /// if stops does not exist
        /// </response>
        /// <response code="500">
        /// some internal errors
        /// </response>
        [ProducesResponseType(typeof(StopVModel), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Expression<Func<Stop, bool>> filterOne = (s) => s.TripID == TripID && s.CreatedBy == UserID && s.Id == id;

                var stop = await _stops.GetOneAsync(filter: filterOne);
                return stop != null ? Ok(stop.ToVModel()) : throw new InvalidOperationException();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorMessage
                {
                    message = "no stop with the specified id and tripID",
                    reason = $"given id {id} is invalid"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to get stop: ${ ex.Message}");
                return StatusCode(500, new ErrorMessage
                {
                    reason = ex.Message
                });
            }
        }

        /// <summary>
        /// create a new stop under the trip with tripID
        /// </summary>
        /// <response code="200">
        /// returns the newly created stop
        /// </response>
        /// <response code="400">
        /// request is not a valid stop
        /// </response>
        /// <response code="500">
        /// some internal errors or invalid tripID
        /// </response>
        [ProducesResponseType(typeof(StopVModel), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
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
                        reason = $"stop creation failed due to: {e.Message}"
                    });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// update the existing stop with the given id
        /// </summary>
        /// <response code="200">
        /// returns the just updated stop</response>
        /// <response code="400">
        /// when the request body is invalid
        /// </response>
        /// <response code="500">
        /// internal server error(s)
        /// </response>
        [ProducesResponseType(typeof(StopVModel), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
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
                        reason = $"stop update failed due to: {e.Message}"
                    });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// delete an existing stop with the given id
        /// </summary>
        /// <response code="200">
        /// returns the request status</response>
        /// <response code="400">
        /// when the request body is invalid
        /// </response>
        /// <response code="500">
        /// internal server error(s)
        /// </response>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
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
                        reason = e.Message
                    });
                }
            }
            else
            {
                return BadRequest(new ErrorMessage
                {
                    reason = $"invalid stop id: {id}"
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
