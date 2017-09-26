using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WorldTripLog.Web.Models;

namespace WorldTripLog.Web.Services
{
    public class GeoCoordsService
    {
        private ILogger<GeoCoordsService> _logger;
        private IConfiguration _config;

        public GeoCoordsService(ILogger<GeoCoordsService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<GeoCoordsResult> GetCoordsAsync(string name)
        {
            var result = new GeoCoordsResult()
            {
                Success = false,
                Message = "failed to get coordinates"
            };

            var apiKey = _config["Bing_Key"];
            var encodedName = WebUtility.UrlEncode(name);
            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={apiKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);
            var results = JObject.Parse(json);
            var resources = results["resourceSets"][0]["resources"];
            if (!results["resourceSets"][0]["resources"].HasValues)
            {
                result.Message = $"Could not find '{name}' as a location";
            }
            else
            {
                var confidence = (string)resources[0]["confidence"];
                if (confidence != "High")
                {
                    result.Message = $"Could not find a confident match for name as a location";
                }
                else
                {
                    var coords = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double)coords[0];
                    result.Longitude = (double)coords[1];
                    result.Success = true;
                    result.Message = "Success";
                }
            }
            return result;
        }

        public async Task<Stop> AddGeoCoords(Stop stop)
        {
            var result = await GetCoordsAsync(stop.Name);
            if (!result.Success)
            {
                _logger.LogError(result.Message);
                throw new NotSupportedException("error occured when getting the location");
            }
            else
            {
                stop.Longitude = result.Longitude;
                stop.Latitude = result.Latitude;
            }
            return stop;
        }
    }
}
