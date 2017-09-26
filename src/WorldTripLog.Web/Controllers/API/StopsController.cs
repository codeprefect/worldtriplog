using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                Expression<Func<Stop, bool>> filter = s => s.TripID == TripID && s.CreatedBy == UserID;

                var stops = await _stops.GetAsync(filter: filter);
                return stops.Any() ? Ok(stops.ToVModel()) : throw new InvalidOperationException();
            }
            catch (Exception)
            {
                _logger.LogError("failed to execute GET");
                return BadRequest(new ErrorMessage
                {
                    message = "failed to get stops",
                    reason = "probably no stops yet."
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Expression<Func<Stop, bool>> filterOne = (s) => s.TripID == TripID && s.CreatedBy == UserID && s.Id == id;

                var stop = await _stops.GetOneAsync(filter: filterOne);
                return stop != null ? Ok(stop.ToVModel()) : throw new InvalidOperationException();
            }
            catch (Exception e)
            {
                _logger.LogError("stop with the specified id does not exist: {0}", e.Message);
                return BadRequest(new ErrorMessage
                {
                    message = "requested resource does not exist",
                    reason = $"given id {id} is invalid"
                });
            }
        }

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
                    return Created($"/api/trips/{TripID}/stops", stop);
                }
                catch (Exception e)
                {
                    return BadRequest($"stop creation failed due to: {e.Message}");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

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
                    return BadRequest($"stop update failed due to: {e.Message}");
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
                    _stops.Delete(id);
                    await _stops.SaveAsync();
                    return Ok($"Deleted Successfully");
                }
                catch (Exception e)
                {
                    return BadRequest($"Stop update failed due to: {e.Message}");
                }
            }
            else
            {
                return BadRequest(ModelState);
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
