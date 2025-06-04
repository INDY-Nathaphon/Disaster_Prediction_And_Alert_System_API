using Disaster_Prediction_And_Alert_System_API.Domain;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.Common.ExternalApi
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ExternalApiService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _apiKey = settings.Value.WeatherApi.Key;
        }

        public async Task<Dictionary<string, double>> GetEnvironmentalDataAsync(double latitude, double longitude)
        {
            var url = $"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={latitude},{longitude}&aqi=no";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(content);
            var current = json.GetProperty("current");

            return new Dictionary<string, double>
            {
                { "rainfall", current.GetProperty("precip_mm").GetDouble() },
                { "temperature", current.GetProperty("temp_c").GetDouble() },
                { "humidity", current.GetProperty("humidity").GetDouble() }
            };
        }
    }

}
