
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorldTripLog.Web.Helpers;
using WorldTripLog.Domain.Entities;
using WorldTripLog.Web.Models;

namespace WorldTripLog.Web.Data
{
    public class WorldTripDbContext : IdentityDbContext<WorldTripUser>
    {
        public WorldTripDbContext(DbContextOptions<WorldTripDbContext> options) : base(options)
        {

        }

        public WorldTripDbContext() { }

        public virtual DbSet<Trip> Trips { get; set; }

        public virtual DbSet<Stop> Stops { get; set; }

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
                throw new DbEntityValidationException(exceptionMessage, null);
            }

            return base.SaveChanges();
        }
    }
}
