using Microsoft.EntityFrameworkCore;
using SolarWatchORM.Data;
using SolarWatchORM.Data.CityData;

namespace SolarWatchORM.Service.CityRepo
{
    public class CityRepo : ICityRepo
    {
        private readonly SolarWatchContext _context;

        public CityRepo(SolarWatchContext context)
        {
            _context = context;
        }

        public async Task<City?> SearchByName(string name)
        {
            return await _context.Cities.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task AddNewCity(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateCity(int id, City updatedCity)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return false;
            }

            city.Name = updatedCity.Name;
            city.Latitude = updatedCity.Latitude;
            city.Longitude = updatedCity.Longitude;
            city.Country = updatedCity.Country;
            city.State = updatedCity.State;

            _context.Cities.Update(city);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return false;
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<City?> GetCityById(int id)
        {
            return await _context.Cities.FindAsync(id);
        }
    }
}
