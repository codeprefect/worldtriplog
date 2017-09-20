
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorldTripLog.Web.Models;

namespace WorldTripLog.Web.Data
{
    public class WorldTripDbContext : IdentityDbContext<WorldTripUser>
    {
        public WorldTripDbContext(DbContextOptions<WorldTripDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
