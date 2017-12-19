using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WorldTripLog.Web.Controllers.Api
{
    public class BaseApiController : Controller
    {

        #region some helpers
        protected string UserID => User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value;

        #endregion
    }
}
