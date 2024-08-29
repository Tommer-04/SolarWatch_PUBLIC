using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatchORM.Data.CityData;
using SolarWatchORM.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using SolarWatchORM.Model;
using SolarWatchORM.Data.SunData;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new SolarWatchContext(
            serviceProvider.GetRequiredService<DbContextOptions<SolarWatchContext>>()))
        {
            if (context.Cities.Any())
            {
                return;
            }

            context.Cities.AddRange(
                new City { Id = 1001, Name = "Sample City 1", Longitude = 20, Latitude = 20, Country = "HU"},
                new City { Id = 1002, Name = "Sample City 2", Longitude = 40, Latitude = 40, Country = "US" }
            );

            context.SunRecords.AddRange(
                new Sun { Id = 1001, CityId = 1001, Sunrise = DateTime.Parse("2000-01-01"), Sunset = DateTime.Parse("2000-01-02")},
                new Sun { Id = 1002, CityId = 1001, Sunrise = DateTime.Parse("2000-02-01"), Sunset = DateTime.Parse("2000-02-02")}
            );


            await context.SaveChangesAsync();
        }
    }
}
