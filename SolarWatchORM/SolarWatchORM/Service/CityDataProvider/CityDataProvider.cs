using Azure;
using Microsoft.IdentityModel.Tokens;
using SolarWatchORM.Data.CityData;
using System.Net.Http;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace SolarWatchORM.Service.CityDataProvider
{
    public class CityDataProvider : ICityDataProvider
    {
        private readonly IConfiguration _config;

        public CityDataProvider(IConfiguration config)
        {
            _config = config;
        }

        async public Task<City?> ProvideData(string cityName)
        {
            string? response = await FetchData(cityName);

            if (response != null)
            {
                using JsonDocument json = JsonDocument.Parse(response);

                if (json.RootElement.GetArrayLength() != 0)
                {  
                    JsonElement cityElement = json.RootElement[0];

                    City city = new City
                    {
                        Name = cityElement.GetProperty("name").GetString(),
                        Latitude = cityElement.GetProperty("lat").GetDouble(),
                        Longitude = cityElement.GetProperty("lon").GetDouble(),
                        Country = cityElement.GetProperty("country").GetString(),
                        State = cityElement.TryGetProperty("state", out JsonElement stateElement)
                        ? stateElement.GetString()
                        : null
                    };

                    return city;
                }
            }

            return null;
        }

        async private Task<string?> FetchData(string cityName)
        {
            string url = $"https://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={_config["ApiKeys:CityApiKey"]}";

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
