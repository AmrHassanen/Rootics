using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Root.API.Models;

namespace Root.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<FavoritePlant> FavoritePlants { get; set; }
        public DbSet<UserPlantActivity> UserPlantActivities { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<SuggestedPlant> SuggestedPlants { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Call the SeedRoles method to seed roles
            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { ConcurrencyStamp = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { ConcurrencyStamp = "2", Name = "User", NormalizedName = "USER" }
            );
        }
    }
}
