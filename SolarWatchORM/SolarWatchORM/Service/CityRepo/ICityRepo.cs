using SolarWatchORM.Data.CityData;

namespace SolarWatchORM.Service.CityRepo
{
    public interface ICityRepo
    {
        Task<City?> SearchByName(string name);
        Task AddNewCity(City city);

        Task<bool> UpdateCity(int id, City updatedCity);

        Task<bool> DeleteCity(int id);

        Task<City?> GetCityById(int id);
    }
}
