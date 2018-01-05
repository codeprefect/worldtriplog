using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorldTripLog.Domain.Entities;
using WorldTripLog.Domain.Interfaces;
using WorldTripLog.Web.Data;

namespace WorldTripLog.Web.Services.Interfaces
{
    public interface IStopService : IDataService<WorldTripDbContext, Stop>
    {

    }
}
