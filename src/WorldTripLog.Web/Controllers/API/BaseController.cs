using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorldTripLog.Web.DAL;
using WorldTripLog.Web.Models;
using WorldTripLog.Web.Services;

namespace WorldTripLog.Web.Controllers.Api
{
    public class BaseApiController : Controller
    {

        #region some helpers
        protected string UserId { get => User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value; }

        #endregion
    }
}
