using SolarWatchORM.Data.CityData;

namespace SolarWatchORM.Data.SunData
{
    public class Sun
    {
        public int Id { get; init; }

        public int CityId { get; init; }

        public DateTime Sunrise { get; init; }

        public DateTime Sunset { get; init; }

    }
}
