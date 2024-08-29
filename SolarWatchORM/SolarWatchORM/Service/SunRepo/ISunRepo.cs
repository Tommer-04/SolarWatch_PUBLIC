using SolarWatchORM.Data.CityData;
using SolarWatchORM.Data.SunData;

namespace SolarWatchORM.Service.SunRepo
{
    public interface ISunRepo
    {
        Task<Sun?> SearchByCityAndDate(City city, DateOnly date);
        Task AddNewSunRecord(Sun sun);
    }
}
