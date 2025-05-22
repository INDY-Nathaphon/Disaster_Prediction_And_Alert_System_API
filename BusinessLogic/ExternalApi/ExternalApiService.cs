using Disaster_Prediction_And_Alert_System_API.Domain;
using Disaster_Prediction_And_Alert_System_API.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Disaster_Prediction_And_Alert_System_API.BusinessLogic.ExternalApi
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;

        public ExternalApiService(HttpClient httpClient, IConfiguration configuration, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = settings.Value.WeatherApi.Key;
        }

        public async Task<Dictionary<string, double>> GetEnvironmentalDataAsync(Region region)
        {
            var url = $"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={region.Latitude},{region.Longitude}&aqi=no";
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
