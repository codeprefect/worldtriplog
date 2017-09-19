
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorldTripLog.Models;

namespace WorldTripLog.Data
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
