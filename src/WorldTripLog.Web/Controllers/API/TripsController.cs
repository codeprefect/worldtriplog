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
    /// <summary>
    /// Trips Controller, providing restful crud operations on trips
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class TripsController : BaseApiController
    {
        private readonly ILogger<TripsController> _logger;

        private readonly IDataService<WorldTripDbContext, Trip> _trips;

        public TripsController(ILogger<TripsController> logger, IDataService<WorldTripDbContext, Trip> trips)
        {
            _logger = logger;
            _trips = trips;
        }

        /// <summary>
        /// get list of all the trips belonging to the authenticated user
        /// </summary>
        /// <response code="200">
        /// returns list of all the trips belonging to the authenticated user
        /// </response>
        /// <response code="400">
        /// if trips is empty
        /// </response>
        /// <response code="500">
        /// some internal errors
        /// </response>
        [ProducesResponseType(typeof(IEnumerable<TripVModel>), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Expression<Func<Trip, bool>> filter = t => t.CreatedBy == UserID;
                var trips = await _trips.GetAsync(filter: filter);
                return trips.Any() ? Ok(trips.ToVModel()) : throw new InvalidOperationException(message: "no trips yet");
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, new ErrorMessage
                {
                    message = "failed to get trips",
                    reason = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorMessage
                {
                    reason = ex.Message
                });
            }
        }

        /// <summary>
        /// get the trip with the given id
        /// </summary>
        /// <response code="200">
        /// returns a trip with the given id</response>
        /// <response code="400">
        /// if trip with given id does not exist
        /// </response>
        /// <response code="500">
        /// some unexpected error
        /// </response>
        [ProducesResponseType(typeof(TripVModel), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Expression<Func<Trip, bool>> filter = t => t.CreatedBy == UserID && t.Id == id;
                var trip = await _trips.GetOneAsync(filter: filter);
                return trip != null ? Ok(trip.ToVModel()) : throw new InvalidOperationException(message: $"trip with id: {id} does not exist");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ErrorMessage
                {
                    message = "requested resource does not exist",
                    reason = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorMessage
                {
                    reason = ex.Message
                });
            }
        }

        /// <summary>
        /// create a new trip instance
        /// </summary>
        /// <response code="201">
        /// returns the just created trip</response>
        /// <response code="400">
        /// when the request body is invalid
        /// </response>
        /// <response code="500">
        /// when the entity validation fails or some internal server errors
        /// </response>
        [ProducesResponseType(typeof(TripVModel), 201)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TripVModel trip)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _trips.Create(trip.ToModel(), UserID);
                    await _trips.SaveAsync();
                    return Created("/api/trips", trip);
                }
                catch (Exception e)
                {
                    return StatusCode(500, new ErrorMessage
                    {
                        message = "trip creation failed",
                        reason = e.Message
                    });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// update the existing trip with the given id
        /// </summary>
        /// <response code="200">
        /// returns the just updated trip</response>
        /// <response code="400">
        /// when the request body is invalid
        /// </response>
        /// <response code="500">
        /// internal server error(s)
        /// </response>
        [ProducesResponseType(typeof(TripVModel), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody]TripVModel trip)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _trips.Update(trip.ToModel(), UserID);
                    await _trips.SaveAsync();
                    return Ok(trip);
                }
                catch (Exception e)
                {
                    return StatusCode(500, new ErrorMessage
                    {
                        message = "trip update failed",
                        reason = e.Message
                    });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// delete an existing trip with the given id
        /// </summary>
        /// <response code="200">
        /// returns a message about the deletion status</response>
        /// <response code="400">
        /// when the given id is not valid
        /// </response>
        /// <response code="500">
        /// when an unexpected occurs
        /// </response>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(typeof(ErrorMessage), 500)]
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
                    return StatusCode(500, new ErrorMessage
                    {
                        reason = e.Message
                    });
                }
            }
            else
            {
                return BadRequest(new ErrorMessage
                {
                    message = "invalid id",
                    reason = "id must be greater than 0"
                });
            }
        }
    }
}
