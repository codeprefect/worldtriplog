
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorldTripLog.Web.Helpers;
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

        public override int SaveChanges()
        {
            var validationErrors = ChangeTracker
                .Entries<IValidatableObject>()
                .SelectMany(e => e.Entity.Validate(null))
                .Where(r => r != ValidationResult.Success);

            var fullErrorMessage = string.Join(";\t", validationErrors);

            var exceptionMessage = string.Concat("The validation error are; ", fullErrorMessage);

            if (validationErrors.Any())
            {
                // Possibly throw an exception here
                throw new DbEntityValidationException(fullErrorMessage, null);
            }

            return base.SaveChanges();
        }
    }
}
