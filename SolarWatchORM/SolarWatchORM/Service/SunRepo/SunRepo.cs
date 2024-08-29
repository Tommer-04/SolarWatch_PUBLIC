using Microsoft.EntityFrameworkCore;
using SolarWatchORM.Data;
using SolarWatchORM.Data.CityData;
using SolarWatchORM.Data.SunData;

namespace SolarWatchORM.Service.SunRepo
{
    public class SunRepo : ISunRepo
    {
        private readonly SolarWatchContext _context;

        public SunRepo(SolarWatchContext context)
        {
            _context = context;
        }

        public async Task<Sun?> SearchByCityAndDate(City city, DateOnly date)
        {
            return await _context.SunRecords.FirstOrDefaultAsync(s => s.CityId == city.Id && date == DateOnly.FromDateTime(s.Sunrise));
        }

        public async Task AddNewSunRecord(Sun sun)
        {
            _context.SunRecords.Add(sun);
            await _context.SaveChangesAsync();
        }
    }
}
