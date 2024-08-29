using SolarWatchORM.Data.CityData;
using SolarWatchORM.Data.SunData;
using System.Text.Json;

namespace SolarWatchORM.Service.SunDataProvider
{
    public class SunDataProvider : ISunDataProvider
    {
        private readonly IConfiguration _config;

        public SunDataProvider(IConfiguration config)
        {
            _config = config;
        }

        async public Task<Sun?> ProvideData(City city, DateOnly date)
        {
            if(date < DateOnly.Parse("0001-01-02") || date > DateOnly.Parse("9999-12-30"))
            {
                return null;
            }
            string? response = await FetchData(city.Latitude, city.Longitude, date);

            if (response != null)
            {
                using JsonDocument json = JsonDocument.Parse(response);
                JsonElement root = json.RootElement;

                if (root.GetProperty("status").GetString() == "OK")
                {
                    JsonElement results = root.GetProperty("results");
                    Sun sun = new Sun
                    {
                        CityId = city.Id,
                        Sunrise = results.GetProperty("sunrise").GetDateTime(),
                        Sunset = results.GetProperty("sunset").GetDateTime(),
                    };

                    return sun;
                }
            }
            return null;
        }

        async private Task<string?> FetchData(double lat, double lng, DateOnly date)
        {
            Console.WriteLine($"{date:yyyy-MM-dd}");
            string url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lng}&date={date:yyyy-MM-dd}&formatted=0";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;
            }
        }
    }
}
