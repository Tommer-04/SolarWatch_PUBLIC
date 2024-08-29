using SolarWatchORM.Data.CityData;
using SolarWatchORM.Data.SunData;

namespace SolarWatchORM.Service.SunDataProvider
{
    public interface ISunDataProvider
    {
        Task<Sun?> ProvideData(City city, DateOnly date);
    }
}
