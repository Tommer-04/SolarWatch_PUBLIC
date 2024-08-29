using Microsoft.EntityFrameworkCore;
using SolarWatchORM.Data.CityData;
using SolarWatchORM.Data.SunData;

namespace SolarWatchORM.Data
{
    public class SolarWatchContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Sun> SunRecords { get; set; }

        private readonly IConfiguration _config;

        public SolarWatchContext(DbContextOptions<SolarWatchContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>()
                .HasMany(c => c.SunRecords)
                .WithOne()
                .HasForeignKey(s => s.CityId);


            modelBuilder.Entity<City>()
                .HasIndex(u => u.Name)
                .IsUnique();
        }
    }
}
