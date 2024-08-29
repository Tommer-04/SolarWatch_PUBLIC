using SolarWatchORM.Data.CityData;

namespace SolarWatchORM.Service.CityDataProvider
{
    public interface ICityDataProvider
    {
        Task<City?> ProvideData(string cityName);
    }
}
