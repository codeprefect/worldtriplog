
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorldTripLog.Models;
using WorldTripLog.Web.Models;

namespace WorldTripLog.Web.Data
{
    public class WorldTripDbContext : IdentityDbContext<WorldTripUser>
    {
        public WorldTripDbContext(DbContextOptions<WorldTripDbContext> options) : base(options)
        {

        }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<Stop> Stops { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
